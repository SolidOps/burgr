# burgr

A source code generator driven by YAML model specifications and DDD principles. Define your domain in YAML, point burgr at a set of templates, and get generated C#, TypeScript, SQL, or PlantUML output.

## Installation

```bash
dotnet tool install --global Burgr
```

Or build and install locally (from `src/`):

```bash
make install
```

## Running the generator

```bash
dotnet burgr <path-to-specs-directory>
```

The specs directory must contain a `burgr.json` file. burgr reads every YAML file in the directory tree and generates output files next to your source code.

---

## burgr.json

Each specs directory has a `burgr.json` that tells burgr what to generate and where to put it.

```json
{
    "ModuleName": "UM",
    "NamespaceName": "SolidOps",
    "ModelParserEngineType": "SolidOps.Burgr.Essential.Yaml.ModelParserEngine, SolidOps.Burgr.Essential",
    "TemplateParserEngineType": "SolidOps.Burgr.Essential.Yaml.TemplateParserEngine, SolidOps.Burgr.Essential",
    "BuildingDirectory": "../",
    "TemplateSpecDirectory": "../um_templates",
    "ModelMonitored": false,
    "Templates": [
        "Contracts",
        "Domain",
        "Application",
        "Infrastructure",
        "EF",
        "Tests"
    ],
    "Generators": [
        "SolidOps.Burgr.Essential.Generators.Enums.EnumGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Services.ServiceGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Rights.RightGenerator, SolidOps.Burgr.Essential"
    ]
}
```

| Field | Description |
|---|---|
| `ModuleName` | Short identifier for the module (used in generated namespaces and file names) |
| `NamespaceName` | Root C# namespace, e.g. `SolidOps` |
| `ModelParserEngineType` | Assembly-qualified type of the model parser — always the YAML one above |
| `TemplateParserEngineType` | Assembly-qualified type of the template parser — always the YAML one above |
| `BuildingDirectory` | Root output path, relative to the specs directory |
| `TemplateSpecDirectory` | Directory containing the template `burgr.yaml` files |
| `Templates` | Ordered list of template names to apply |
| `Generators` | List of generator types to activate |
| `ModelMonitored` | Set to `true` to watch for file changes and regenerate automatically |
| `OverrideDestination` | Optional. Redirect a template's output to a custom path: `"NgClient=../../angular/projects/lib"` |

### Multi-template project example

When generating several layers (domain + Angular client + PlantUML diagrams), `OverrideDestination` separates outputs that don't belong in the main `BuildingDirectory`:

```json
{
    "ModuleName": "Lab",
    "NamespaceName": "SolidOps",
    "ModelParserEngineType": "SolidOps.Burgr.Essential.Yaml.ModelParserEngine, SolidOps.Burgr.Essential",
    "TemplateParserEngineType": "SolidOps.Burgr.Essential.Yaml.TemplateParserEngine, SolidOps.Burgr.Essential",
    "BuildingDirectory": "../../../source/dotnet/4.Lab",
    "TemplateSpecDirectory": "../../../../templates",
    "Templates": [
        "Template.Contracts",
        "Template.Domain",
        "Template.Application",
        "Template.Infrastructure",
        "Template.EF",
        "Template.NgClient",
        "Template.Diagrams"
    ],
    "Generators": [
        "SolidOps.Burgr.Essential.Generators.Enums.EnumGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Services.ServiceGenerator, SolidOps.Burgr.Essential",
        "SolidOps.Burgr.Essential.Generators.Rights.RightGenerator, SolidOps.Burgr.Essential"
    ],
    "OverrideDestination": "NgClient=../../angular/norad/projects/solidops-lab-lib;Diagrams=../../../specs/Lab"
}
```

---

## YAML model files

All YAML files start with:

```yaml
burgr-model-version: 1.0.0
```

A specs directory can contain any combination of these file types. Files can be organised in sub-directories — burgr walks the whole tree.

### Type system

| Syntax | Meaning |
|---|---|
| `text` / `string` | String |
| `int`, `double`, `decimal` | Numeric |
| `bool` | Boolean |
| `datetime` | Date/time |
| `guid` | GUID/UUID |
| `type?` | Nullable |
| `type[]` | Array / collection |
| `->entity[]` | Navigation property (collection, loaded from DB) |
| `$enum_name` | Reference to a defined enum |
| `=type` | Calculated property (mapped/computed on read) |
| `-type` | Non-persisted property (not stored in DB) |

---

### aggregate_roots.yaml

Aggregate roots are the main persistable entities. They support full CRUD APIs, events, rules, and components.

```yaml
burgr-model-version: 1.0.0

aggregate_roots:

  # Simple entity with a unique label field and an enum property
  item:
    properties:
      name:
        is_unique: true
        field_size: 50
      due_date: datetime
      remaining_days: =int        # calculated on read, not persisted in DB
      status: $item_status        # references an enum defined below

  # Entity with navigation, ownership control, lifecycle rules, and UI components
  user:
    properties:
      email:
        is_unique: true
        field_size: 50
      provider:
        field_size: 50
      technical_user: bool
      user_rights: ->user_right[] # eager-loaded child collection
      rights: =string[]           # calculated from user_rights
    api:
      query:                      # GET /users
      get:                        # GET /users/{id}
      add:
        ownership_override_right: ManageUsers
      update:
        ownership_override_right: ManageUsers
    dependencies:
      local_user: read
    components:
      list:
      details:
    views:
      list:
      details:
    rules:
      before_add:
        - create_local_user       # custom rule handler name

  # Entity with event production and full CRUD
  background_task:
    properties:
      name:
        is_unique: true
        field_size: 50
      assembly:
      runner_class_name:
      parameters:
      schedules: schedule[]       # embedded value objects
      results: ->background_task_result[]
      previous_event: -datetime   # not persisted, derived at runtime
      enabled: bool
    api:
      add:
      query:
      get:
      update:
      remove:
    event_produced: add|update|remove

enums:
  item_status:
    values:
      Todo: 0
      InProgress: 1
      Done: 2
```

**Property attributes**

| Attribute | Description |
|---|---|
| `is_unique` | Adds a unique constraint |
| `field_size` | Max column length |
| `is_label` | Marks the display name field |
| `is_queryable` | Allows filtering on this field via the query API |
| `is_private` | Not exposed in the API |
| `multiple_unique_constraint_with` | Composite unique key with the named property |
| `enable_change_tracking` | Audits changes to this entity |

**API access control**

| Attribute | Description |
|---|---|
| `is_anonymous: true` | No authentication required |
| `mandatory_right: RightName` | Caller must have this right |
| `ownership_override_right: RightName` | Bypasses ownership check when caller has this right |

**`event_produced`** can be a pipe-separated list (`add|update|remove`) or a mapping to custom event names:

```yaml
event_produced:
  add: UserAdded
  update: UserUpdated
```

---

### entities.yaml

Regular entities — persistable, but not aggregate roots. No top-level API of their own; accessed through their aggregate root.

```yaml
burgr-model-version: 1.0.0

entities:

  right:
    properties:
      name:
        field_size: 50
        is_unique: true

  user_right:
    properties:
      user: user        # foreign key to aggregate root
      right: right      # foreign key to entity
```

---

### value_objects.yaml

Immutable objects embedded inside entities or aggregates. Not stored in their own table.

```yaml
burgr-model-version: 1.0.0

value_objects:

  schedule:
    properties:
      cron_expression:

  address:
    properties:
      street:
      city:
        is_label: true
      zip_code:
        field_size: 10
```

---

### transients.yaml

Data transfer objects — used as inputs or outputs for services. Not persisted.

```yaml
burgr-model-version: 1.0.0

transients:

  login_request:
    properties:
      login:
      password:

  invite_result:
    properties:
      email:
      creator:
      message:

  user_creation_info:
    properties:
      user_email:
      password:
      rights: user_right[]
```

---

### services.yaml

Services group related methods that implement business logic. A service marked `api: true` gets REST endpoints generated. Methods can declare inputs (typed or untyped), a result type, and per-method access control.

```yaml
burgr-model-version: 1.0.0

services:

  # Anonymous service — no auth required on any method
  authentication:
    api_description:
      is_anonymous: true
    methods:
      login:
        inputs:
          request: login_request      # typed input (transient)
      set_initial_password:
        inputs:
          email:                      # untyped → defaults to string
          password:
      need_initial_password:
        result: bool
        inputs:
          email:
    dependencies:
      user:
      local_user:

  # Service with mixed access — default authenticated, one method requires a right
  user_creation:
    api: true
    methods:
      create_user:
        result: identity
        inputs:
          user_creation_info: user_creation_info
        api_description:
          mandatory_right: CreateUser
    dependencies:
      local_user:
      user:

  # Read-only service
  get_items:
    methods:
      execute:
        result: item[]
    dependencies:
      item:
    api:
      is_anonymous: true

  # Simple command service
  add_item:
    methods:
      execute:
        inputs:
          item: item
    dependencies:
      item:
    api:
      is_anonymous: true
```

---

### enums.yaml

Enums can be declared in a dedicated file or inline in any other spec file.

```yaml
burgr-model-version: 1.0.0

enums:

  card_type:
    values:
      Player: 0
      Maintenance: 3
      Skill: 5

  invite_status:
    values:
      Sent: 0
      Used: 1
      Declined: 2

  transaction_status:
    values:
      Initiated: 0
      Pending: 1
      Completed: 2
      Canceled: 3
```

---

## Directory layout

A typical project organises specs by module:

```
specs/
  .models/
    Module1/
      aggregate_roots.yaml
      entities.yaml
      value_objects.yaml
      transients.yaml
      services.yaml
    Module2/
      aggregate_roots.yaml
    burgr.json
```

`burgr.json` must sit at the root of the directory you pass to `dotnet burgr`. Sub-directories are scanned recursively — no extra configuration needed.

---

## Writing templates

A template set lives in a directory (e.g. `Template.Domain/`) and contains two kinds of files:

- **`burgr.yaml`** — declares what source files exist and where their output goes.
- **Source files** (`.cs`, `.ts`, `.tpl`, …) — the actual code templates, annotated with generation markers.

### burgr.yaml

Each entry under `source_templates` maps a logical name to a source file and controls the output destination.

```yaml
source_templates:
  entity:
    source: Entity.cs              # source template file in this directory
    file_suffix: Entity.cs         # suffix appended to the generated file name
    destination_suffix: Domain     # sub-folder inside BuildingDirectory
    force_separate_dll: true       # place output in a separate assembly folder
    options:
      model_prefix: .Domain._DOMAINTYPE_.
      model_suffix: ''
    destination_language: CS
```

### Loop markers

The core mechanism is a pair of `#region` / `#endregion` directives that tell burgr which block to repeat for each matching element from the YAML spec.

```
#region foreach IDENTIFIER[filter1][filter2]...
    ... code repeated for each matching item ...
#endregion foreach IDENTIFIER
```

`IDENTIFIER` is one of:

| Identifier | Iterates over |
|---|---|
| `MODEL` | all model types (aggregate roots, entities, value objects, transients) |
| `PROPERTY` | properties of the current model |
| `DOMAIN_SERVICE` | domain services |
| `ENUMTYPE` | enum types |
| `SERVICE_METHOD_PARAMETER` | parameters of the current service method |

Loops can be nested — a `PROPERTY` loop inside a `MODEL` loop repeats for every property of every matching model.

### Filters

Filters in square brackets narrow which items a loop applies to. Multiple filters on the same `#region` line are **AND**-ed together. Prefix a filter with `!` to negate it.

**Model filters**

| Filter | Meaning |
|---|---|
| `[AG]` | Aggregate root |
| `[EN]` | Entity |
| `[TR]` | Transient |
| `[VO]` | Value object |
| `[CT]` | Change-tracked |
| `[C]` | Component |
| `[R]` | Has resources |
| `[!R]` | Has no resources |

**Property filters**

| Filter | Meaning |
|---|---|
| `[S]` | Simple type (string, int, bool, …) |
| `[E]` | Enum |
| `[M]` | Model reference (same module) |
| `[R]` | Referenced model (external module) |
| `[NO]` | Normal (not calculated, not non-persisted) |
| `[CA]` | Calculated (`=type`) |
| `[NP]` | Non-persisted (`-type`) |
| `[N]` | Nullable |
| `[NN]` | Non-nullable |
| `[AR]` | Array / collection |
| `[NAR]` | Not an array |
| `[SNA]` | Single navigation property |
| `[LNA]` | List navigation property |
| `[PUO]` | Public only |
| `[PRO]` | Private only |

**Example — repeat for every non-calculated simple property:**

```csharp
#region foreach PROPERTY[S][NO]
public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
#endregion foreach PROPERTY
```

**Example — nested loops, model then property with negated filter:**

```csharp
#region foreach MODEL[AG][EN]
public partial class _CLASSNAME_
{
    #region foreach PROPERTY[M][R][NO][!VO]
    public _IDENTITY_KEY_TYPE_ _PROPERTYNAME_Id { get; set; }
    #endregion foreach PROPERTY
}
#endregion foreach MODEL
```

### Conditional removal markers

A `to remove if` block is emitted into the output file and then stripped at generation time when the named condition matches. Use this for boilerplate that only applies to certain model configurations.

```
#region to remove if CONDITION
    ... code removed when condition is true ...
#endregion to remove if CONDITION
```

| Condition | Removed when… |
|---|---|
| `PRIVATE_ID` | entity has a private identity |
| `ANONYMOUS` | endpoint is anonymous |
| `NOT_ANONYMOUS` | endpoint requires authentication |
| `NOT_GETBYQUERY` | no query (list) endpoint |
| `NOT_GETBYID` | no get-by-id endpoint |
| `NOT_ADD` | no add endpoint |
| `NOT_UPDATE` | no update endpoint |
| `NOT_REMOVE` | no remove endpoint |
| `NO_LABEL` | model has no label field |
| `NO_CHANGE_TRACKING` | model has no change-tracking |
| `COMPOSED_ID` | model has a composite key |
| `NOT_COMPOSED_ID` | model does not have a composite key |
| `NOT_LISTCOMPONENT` | model has no list component |
| `NOT_DETAILSCOMPONENT` | model has no details component |

**Example:**

```csharp
public override string? ToString()
{
    var value = base.ToString();
    #region to remove if NO_LABEL
    value = $"{_LABEL_}";
    #endregion to remove if NO_LABEL
    return value;
}
```

There is also a `#region to remove at generation` / `#endregion to remove at generation` marker for placeholder code that must always be stripped from the generated output (e.g. compile-time constants used only inside the template for IDE support).

### Placeholder tokens

Inside a loop body, underscore-wrapped tokens are replaced with values derived from the YAML spec:

| Token | Replaced with |
|---|---|
| `_CLASSNAME_` | PascalCase class name of the current model |
| `_DOMAINTYPE_` | Domain folder name (`Aggregates`, `Entities`, `ValueObjects`, `Transients`) |
| `_IDENTITY_KEY_TYPE_` | Key type (`Guid`, `int`, …) |
| `_PROPERTYNAME_` | camelCase property name |
| `_PROPERTYTYPE_` | Property type name |
| `_SIMPLE__PROPERTYNAME_` | Property name for a simple-typed property |
| `_SIMPLE__TYPE_` | Simple property type |
| `_ENUM__PROPERTYNAME_` | Property name for an enum property |
| `_ENUMNULLABLETYPE_` | Nullable enum type name |
| `_FIELDNAME_` | Backing field name (prefixed with `_`) |
| `_DEFAULT_VALUE_` | Default value for the property type |
| `_ISNULL_` | `?` when the property is nullable, empty otherwise |
| `_LABEL_` | Value of the `is_label` property |
| `_CALCULATED__*_` | Variants of the above for calculated properties |
| `_NONPERSISTED__*_` | Variants of the above for non-persisted properties |
| `_NAVIGATION__*_` | Variants of the above for navigation properties |
| `_FORLIST__*_` | Variants of the above for list/collection properties |
| `_REF__*_` | Variants of the above for cross-module referenced properties |
| `DEPENDENCYNAMESPACE` | Root namespace of the referenced module |
| `MODELNAME` | Snake_case model name |
| `MINSIZE` / `FIELDSIZE` | Min/max field size from `field_size` / `min_size` spec attributes |

---

## Samples

| Sample | Description |
|---|---|
| `Samples/1. TODO list` | Simple console app with one aggregate root and three services |
| `Samples/2. User management API` | REST API with authentication, invite flow, and rights management |
