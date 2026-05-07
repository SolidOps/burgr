<div align="center">
  <img src="logo.svg" alt="burgr" width="480" />
  <br/><br/>

  [![Build](https://github.com/SolidOps/burgr/actions/workflows/build_publish.yml/badge.svg)](https://github.com/SolidOps/burgr/actions/workflows/build_publish.yml)
  [![NuGet](https://img.shields.io/nuget/v/Burgr.svg)](https://www.nuget.org/packages/Burgr)
  [![License](https://img.shields.io/github/license/SolidOps/burgr)](LICENSE)

  [**Documentation**](#yaml-spec-reference) · [**Quick start**](#quick-start) · [**Samples**](#samples) · [**Issues**](https://github.com/SolidOps/burgr/issues)
</div>

---

## What is burgr?

**burgr** is a `dotnet` source code generator driven by YAML model specifications and DDD principles.

You describe your domain once — aggregate roots, entities, enums, services — and burgr generates the C#, TypeScript, SQL, or PlantUML code you'd otherwise write by hand. Change the spec, re-run, done.

```bash
# 1. describe your domain
cat specs/aggregate_roots.yaml

# 2. run burgr
dotnet burgr specs/

# 3. find your generated files next to your source code
```

burgr is distributed as a standard `dotnet tool`. It runs a 4-stage pipeline:

> **YAML specs** → model parsing → template expansion → **generated source files**

Each stage is pluggable — bring your own model parser, template engine, or code generators via `Burgr.Core`.

---

## Installation

```sh
dotnet tool install --global Burgr
```

Or build and install locally from the `/src` directory:

```sh
make install
```

**Requirements:** .NET 6 or later.

---

## Quick start

**1. Create a specs directory with a `burgr.json`:**

```json
{
  "ModuleName": "Todo",
  "NamespaceName": "MyApp",
  "ModelParserEngineType": "SolidOps.Burgr.Essential.Yaml.ModelParserEngine, SolidOps.Burgr.Essential",
  "TemplateParserEngineType": "SolidOps.Burgr.Essential.Yaml.TemplateParserEngine, SolidOps.Burgr.Essential",
  "BuildingDirectory": "../",
  "TemplateSpecDirectory": "../my_templates",
  "Templates": ["Domain", "Application"],
  "Generators": [
    "SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential",
    "SolidOps.Burgr.Essential.Generators.Services.ServiceGenerator, SolidOps.Burgr.Essential"
  ]
}
```

**2. Add a YAML model file:**

```yaml
# specs/aggregate_roots.yaml
burgr-model-version: 1.0.0

aggregate_roots:
  task:
    properties:
      title:
        is_unique: true
        field_size: 100
      due_date: datetime
      status: $task_status
    api:
      query:
      get:
      add:
      update:
      remove:

enums:
  task_status:
    values:
      Todo: 0
      InProgress: 1
      Done: 2
```

**3. Run the generator:**

```sh
dotnet burgr specs/
```

That's it — C#, TypeScript, SQL, and diagram files land next to your source code.

---

## YAML spec reference

All YAML spec files start with:

```yaml
burgr-model-version: 1.0.0
```

A specs directory can mix any of the file types below. Directories are scanned recursively — no extra configuration needed.

### Type system

| Syntax | Meaning |
|---|---|
| `text` / `string` | String |
| `int`, `double`, `decimal` | Numeric |
| `bool` | Boolean |
| `datetime` | Date/time |
| `guid` | GUID / UUID |
| `type?` | Nullable |
| `type[]` | Array / collection |
| `->entity[]` | Navigation property (loaded from DB) |
| `$enum_name` | Reference to an enum |
| `=type` | Calculated property (mapped on read, not persisted) |
| `-type` | Non-persisted property (derived at runtime) |

---

### `aggregate_roots.yaml`

Aggregate roots are the primary persistable entities. They support full CRUD APIs, events, rules, and UI components.

```yaml
aggregate_roots:

  user:
    properties:
      email:
        is_unique: true
        field_size: 50
      technical_user: bool
      user_rights: ->user_right[]   # navigation property
      rights: =string[]             # calculated, not persisted
    api:
      query:
      get:
      add:
        ownership_override_right: ManageUsers
      update:
        ownership_override_right: ManageUsers
    components:
      list:
      details:
    rules:
      before_add:
        - create_local_user
    event_produced:
      add: UserAdded
```

**Property attributes**

| Attribute | Description |
|---|---|
| `is_unique` | Unique constraint |
| `field_size` | Max column length |
| `is_label` | Marks the display name field |
| `is_queryable` | Allows filtering via the query API |
| `is_private` | Not exposed in the API |
| `multiple_unique_constraint_with` | Composite unique key with the named field |
| `enable_change_tracking` | Audits changes to this entity |

**API access control**

| Attribute | Description |
|---|---|
| `is_anonymous: true` | No authentication required |
| `mandatory_right: RightName` | Caller must hold this right |
| `ownership_override_right: RightName` | Bypasses ownership check when caller holds this right |

`event_produced` accepts a pipe-separated list (`add|update|remove`) or a map to custom event names.

---

### `entities.yaml`

Persistable, but not aggregate roots. Accessed through their parent aggregate — no top-level API of their own.

```yaml
entities:
  user_right:
    properties:
      user: user
      right: right
```

---

### `value_objects.yaml`

Immutable objects embedded inside aggregates. Not stored in their own table.

```yaml
value_objects:
  address:
    properties:
      street:
      city:
        is_label: true
      zip_code:
        field_size: 10
```

---

### `transients.yaml`

Data transfer objects used as inputs or outputs. Not persisted.

```yaml
transients:
  login_request:
    properties:
      login:
      password:
```

---

### `services.yaml`

Services group business-logic methods. Setting `api: true` generates REST endpoints.

```yaml
services:
  authentication:
    api_description:
      is_anonymous: true
    methods:
      login:
        inputs:
          request: login_request
      need_initial_password:
        result: bool
        inputs:
          email:
    dependencies:
      user:
      local_user:
```

---

### `enums.yaml`

Enums can live in a dedicated file or inline in any other spec file.

```yaml
enums:
  card_type:
    values:
      Player: 0
      Maintenance: 3
      Skill: 5
```

---

## Configuration — `burgr.json`

| Field | Description |
|---|---|
| `ModuleName` | Short identifier used in namespaces and file names |
| `NamespaceName` | Root C# namespace |
| `ModelParserEngineType` | Assembly-qualified model parser type |
| `TemplateParserEngineType` | Assembly-qualified template parser type |
| `BuildingDirectory` | Root output path, relative to the specs directory |
| `TemplateSpecDirectory` | Directory containing template `burgr.yaml` files |
| `Templates` | Ordered list of template names to apply |
| `Generators` | List of generator types to activate |
| `ModelMonitored` | `true` to watch files and regenerate automatically |
| `OverrideDestination` | Redirect a template's output: `"NgClient=../../angular/projects/lib"` |

---

## Writing templates

A template set is a directory containing:

- **`burgr.yaml`** — maps logical names to source files and output destinations.
- **Source files** (`.cs`, `.ts`, `.tpl`, …) — code templates annotated with generation markers.

### Loop markers

```
#region foreach IDENTIFIER[filter1][filter2]
    ... repeated for each matching item ...
#endregion foreach IDENTIFIER
```

| Identifier | Iterates over |
|---|---|
| `MODEL` | All model types |
| `PROPERTY` | Properties of the current model |
| `DOMAIN_SERVICE` | Domain services |
| `ENUMTYPE` | Enum types |
| `SERVICE_METHOD_PARAMETER` | Parameters of the current service method |

Loops nest freely — a `PROPERTY` loop inside a `MODEL` loop repeats for every property of every model.

### Filters

Filters narrow what a loop matches. Multiple filters are **AND**-ed; prefix with `!` to negate.

**Model filters:** `[AG]` aggregate root · `[EN]` entity · `[TR]` transient · `[VO]` value object · `[CT]` change-tracked · `[C]` has component · `[R]` has resources

**Property filters:** `[S]` simple type · `[E]` enum · `[M]` model ref · `[R]` external ref · `[NO]` normal · `[CA]` calculated · `[NP]` non-persisted · `[N]` nullable · `[NN]` non-nullable · `[AR]` array · `[NAR]` not array · `[SNA]` single nav · `[LNA]` list nav

```csharp
#region foreach MODEL[AG][EN]
public partial class _CLASSNAME_
{
    #region foreach PROPERTY[S][NO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY
}
#endregion foreach MODEL
```

### Conditional removal markers

```
#region to remove if CONDITION
    ... stripped when condition is true ...
#endregion to remove if CONDITION
```

| Condition | Removed when… |
|---|---|
| `ANONYMOUS` | Endpoint is anonymous |
| `NOT_ANONYMOUS` | Endpoint requires authentication |
| `NOT_GETBYQUERY` / `NOT_GETBYID` / `NOT_ADD` / `NOT_UPDATE` / `NOT_REMOVE` | Corresponding endpoint is absent |
| `NO_LABEL` | Model has no label field |
| `NO_CHANGE_TRACKING` | Model has no change tracking |
| `PRIVATE_ID` | Entity has a private identity |
| `COMPOSED_ID` / `NOT_COMPOSED_ID` | Composite key presence |
| `NOT_LISTCOMPONENT` / `NOT_DETAILSCOMPONENT` | Component presence |

Use `#region to remove at generation` for placeholder code that should always be stripped (e.g. IDE-only constants).

### Placeholder tokens

| Token | Replaced with |
|---|---|
| `_CLASSNAME_` | PascalCase class name |
| `_DOMAINTYPE_` | Domain folder (`Aggregates`, `Entities`, …) |
| `_IDENTITY_KEY_TYPE_` | Key type (`Guid`, `int`, …) |
| `_PROPERTYNAME_` | camelCase property name |
| `_PROPERTYTYPE_` | Property type name |
| `_SIMPLE__PROPERTYNAME_` / `_SIMPLE__TYPE_` | Simple-typed property name / type |
| `_ENUM__PROPERTYNAME_` / `_ENUMNULLABLETYPE_` | Enum property name / nullable type |
| `_FIELDNAME_` | Backing field name (prefixed `_`) |
| `_DEFAULT_VALUE_` | Default value for the property type |
| `_ISNULL_` | `?` when nullable, empty otherwise |
| `_LABEL_` | Value of the `is_label` property |
| `_CALCULATED__*_` / `_NONPERSISTED__*_` / `_NAVIGATION__*_` | Variants for calculated / non-persisted / navigation properties |
| `_FORLIST__*_` / `_REF__*_` | Variants for list and cross-module properties |
| `DEPENDENCYNAMESPACE` | Root namespace of the referenced module |
| `MODELNAME` | Snake_case model name |
| `MINSIZE` / `FIELDSIZE` | Min / max field size |

---

## Directory layout

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

`burgr.json` must sit at the root of the directory passed to `dotnet burgr`. Sub-directories are scanned recursively.

---

## Samples

| Sample | Description |
|---|---|
| [`Samples/1. TODO list`](Samples/1.%20TODO%20list) | Simple console app — one aggregate root, three services |
| [`Samples/2. User Management API`](Samples/2.%20User%20Management%20API) | REST API with authentication, invite flow, and rights management |

---

## Contributing

Pull requests are welcome. For major changes, please open an issue first.

## License

[MIT](LICENSE)
