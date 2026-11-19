<div align="center">
  <img src="logo.png" alt="burgr" width="480" />
  <br/><br/>

  [![Build](https://github.com/SolidOps/burgr/actions/workflows/build_publish.yml/badge.svg)](https://github.com/SolidOps/burgr/actions/workflows/build_publish.yml)
  [![NuGet](https://img.shields.io/nuget/v/Burgr.svg)](https://www.nuget.org/packages/Burgr)
  [![License](https://img.shields.io/github/license/SolidOps/burgr)](LICENSE)

  [**Documentation**](#yaml-spec-reference) · [**Quick start**](#quick-start) · [**Samples**](#samples) · [**Issues**](https://github.com/SolidOps/burgr/issues)
</div>

---

## What is burgr?

**burgr** is a deterministic, model-driven code generator for `dotnet`. You describe your domain once in a small YAML model; burgr hydrates a set of templates into the C#, TypeScript, SQL or PlantUML you would otherwise write by hand. Same model + same templates = same output, on every machine (proved on every push by a golden-files test). Change the model, run again, done.

```bash
# 1. describe your domain
cat specs/aggregate_roots.yaml

# 2. run burgr
dotnet burgr specs/

# 3. find the generated files next to your source code
```

> **YAML specs** → model parsing → template expansion → **generated source files**

Each stage is pluggable: bring your own model parser, template engine or code generators via `Burgr.Core`. The generated language is irrelevant, templates are text: they already produce C#, TypeScript, SQL and PlantUML.

### What it is for

- **Consistency and speed.** The structural code (entities, DTOs, persistence, API, client) is written once, in templates, and stays identical across every entity and project. No copy-paste drift, nothing to review in the generated files.
- **A model as the source of truth.** The YAML model is small, readable and validatable, and can produce several outputs from one description: code, database schema, diagrams.
- **Your own conventions.** Template sets are plain packages: use the public ones ([Mini and Onion](#template-sources)) or write your own for your architecture, even in another language.
- **AI coding agents.** A *Model-Driven Harness*: the AI writes the model, a deterministic generator writes the code, the tests decide. This is one use of burgr, not a requirement: nothing in the tool needs an AI, and the harness is opt-in.

### With an AI agent: the harness

*Harness engineering* frames an AI coding agent with everything around it: context, tools, permissions and deterministic feedback loops. A rule that is *enforced* holds; a rule that is only *asked* drifts. A deterministic generator fits at the center of such a harness:

- the AI writes the YAML model, plus the code of the extension points the template set provides;
- burgr writes the rest, with no tokens spent;
- the harness shipped with the template set tells the AI where it may write, what it must neither edit nor read (the generated files, enforced by a hook) and how to test. `dotnet burgr specs/ /harness:.` installs it, never overwriting a file.

#### What it saves, measured

Same tasks, same prompts, Claude Sonnet 5.5 through headless Claude Code, 5 runs per case, judged by a hidden acceptance suite. Protocol and scripts: [SolidOps/burgr-benchmarks](https://github.com/SolidOps/burgr-benchmarks).

| Task | Cost | Output tokens |
|---|---|---|
| Create a simple / a complex project | −31 % / −37 % | −51 % / −63 % |
| Evolve a complex project | −47 % | −54 % |
| Small fix, question about the code | +12 % / +18 % | 0 % / −9 % |

Read these with their caveats: input tokens go *up* when creating (more turns, almost all cached), the first build of the complex case is never green with burgr, small tasks cost a little more (the harness has an entry cost), and the share of generable code decides the gain: hand-written business logic costs the same either way. Costs are Claude Code's estimates.

---

## Installation

```sh
dotnet tool install --global Burgr
```
---

## Quick start

burgr's default template set, **Mini**, generates an ASP.NET Core Minimal API with EF Core
in one project, from public packages only.

**1. Create the project the code is generated into:**

```sh
dotnet new web -o src/Todo
dotnet add src/Todo package Microsoft.EntityFrameworkCore.Sqlite
```

**2. Create a specs directory with a `burgr.json`:**

```json
{
  "ProjectName": "Todo",
  "NamespaceName": "MyApp",
  "BuildingDirectory": "../src/"
}
```

**3. Add a YAML model file:**

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

**4. Run the generator, and install the AI harness of the template set:**

```sh
dotnet burgr specs/ /harness:.
```

burgr writes `src/Todo/{Domain,Contracts,Infrastructure,Api}/*.g.cs`, and copies
`AGENTS.md`, `CLAUDE.md` and a Claude Code hook that keeps AI agents out of the generated
files (existing files are never overwritten).

**5. Wire it in `src/Todo/Program.cs`:**

```csharp
using Microsoft.EntityFrameworkCore;
using MyApp.Todo.Api;
using MyApp.Todo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlite("Data Source=todo.db"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TodoDbContext>().Database.EnsureCreated();
}
app.MapTodoEndpoints();
app.Run();
```

`dotnet run --project src/Todo`, and `/tasks` answers. Change the model, run burgr again,
the generated files follow. `AGENTS.md` lists what the Mini set supports, what it
generates and its extension points; a model using anything else is refused with the name
of the feature. The [Mini API sample](src/Samples/3.%20Mini%20API) adds tests.

The code generated from the Mini set is yours: its templates are under
[MIT-0](src/Templates/Mini/LICENSE), which requires no attribution.

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

**Object attributes**

| Attribute | Description |
|---|---|
| `id_column_name` | Column name of the generated `Id` key |
| `key: [a, b]` | Composite key (see below) |
| `is_exposed: true` | Generates the object's DTOs (and those of what it references) without an API, e.g. for another module whose API references it |

**Composite key**

`key` lists two or more properties, in declaration order, that together identify the
row. The object then has no generated `Id`: its identity is `ComposedId`, the key values
joined by `|` (`"12|34"`), which is also what the API uses as id.

```yaml
aggregate_roots:
  category_product:
    key: [category, product]
    properties:
      category:
        type: category
        special_type: DBAlias
        db_column_name: id_category
      product:
        type: product
      position: int
```

Key properties must be persisted, non-nullable, and of a simple type, an enum or a
reference to an aggregate root. An object with a `key` must be an aggregate root, cannot
declare `id_column_name`, `is_unique` or `multiple_unique_constraint_with`, and cannot be
referenced by another object (that would need a multi-column foreign key). A parent can
still navigate to it (`->category_product[]`): the children are found by their own
reference to the parent.

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
| `ProjectName` | Short identifier used in namespaces and file names |
| `NamespaceName` | Root C# namespace |
| `ModelParserEngineType` | Assembly-qualified model parser type |
| `TemplateParserEngineType` | Assembly-qualified template parser type |
| `BuildingDirectory` | Root output path, relative to the specs directory |
| `TemplateSpecDirectory` | Directory containing template `burgr.yaml` files (takes precedence over `TemplatePackage`) |
| `TemplatePackage` | Templates published as a NuGet package: `{ "Id": "...", "Version": "...", "Source": "..." }` — see [Template sources](#template-sources) |
| `IdentityKeysType` | Identity key type of every module generated by this file: `Guid` (default) or `long` |
| `ExternalModules` | Modules generated by *another* `burgr.json`: identity key type, and the transients of that module this one references (anything else is an aggregate root): `{ "Namespace.Module": { "IdentityKeysType": "long", "Transients": ["price"] } }`. A referenced type is written `Namespace.Module.object`, as a property, a service input or a service result |
| `ForcedPrefix` | Table name prefix of every object, instead of the module's |
| `ZeroIsNoReference` | `true` when the default identifier (`0`) means "no reference", as in databases without foreign keys (PrestaShop). A template set can then write such a reference as is instead of `null`, and skip its existence check before an add or update (default `false`) |
| `Templates` | Ordered list of template names to apply |
| `Generators` | List of generator types to activate |
| `ModelMonitored` | `true` to watch files and regenerate automatically |
| `OverrideDestination` | Redirect a template's output: `"NgClient=../../angular/projects/lib"` |
| `GeneratedFilePrefix`, `GeneratedFileSuffix` | Name of a generated file: `<prefix><name><suffix>.<ext>`, `""` and `.g` by default (`ProductEntity.g.cs`) |
| `FrameworkFileSuffix` | Suffix of the [framework files](#framework-files) of the template set, `.f` by default (`AggregateRoot.f.cs`); must differ from `GeneratedFileSuffix` |

### Identity keys

`IdentityKeysType` accepts `Guid` (default) or `long`; any other value stops generation.
With `long`, the database generates the key. API DTOs and the generated TypeScript client
always carry ids as strings.

burgr never loads another module's model, so a reference to a module generated by a
different `burgr.json` (`other_thing: Namespace.Module.thing`) takes its key type from
`ExternalModules`, `Guid` if the module is not listed.

### Template sources

Templates are read from the first of:

1. the `/templateSpecDirectory:<path>` command-line option (relative to the directory
   burgr is run from): `dotnet burgr specs/ /templateSpecDirectory:../my_templates`;
2. `TemplateSpecDirectory` in `burgr.json` (relative to the specs directory);
3. `TemplatePackage`: the package's `templates/` folder, taken from the NuGet global
   packages folder (`NUGET_PACKAGES` or `~/.nuget/packages`) when already restored,
   otherwise downloaded once into `%LOCALAPPDATA%/burgr/packages` from `Source` (a NuGet
   flat-container URL, `https://api.nuget.org/v3-flatcontainer/` by default);
4. none of them: the default set, the `Burgr.Templates.Mini` package at the version of
   burgr itself.

Two template sets are published with burgr, at its version, with their AI harness
(`dotnet burgr specs/ /harness:.` installs it, never overwriting a file):

| Package | Generates |
|---|---|
| `Burgr.Templates.Mini` (default) | One project: Minimal API, DTOs, EF Core `DbContext`, CRUD endpoints. No framework of its own. |
| `Burgr.Templates.Onion` | Domain, Application, Infrastructure, Api and Tests projects in onion architecture, plus a class diagram: entities, value objects, services, transients, rights, rules, factories, in-process events, synchronized navigations, paged queries, with a light framework generated as `*.f.cs`. Its `AGENTS.md` documents the model, the extension points and the framework contracts. |

```json
{
  "ProjectName": "Library",
  "NamespaceName": "Acme",
  "BuildingDirectory": "../src/",
  "TemplatePackage": { "Id": "Burgr.Templates.Onion", "Version": "1.10.0" }
}
```

A template set describes itself in a `burgr-templates.json` file at its root, every field
optional:

```json
{
  "MinimumBurgrVersion": "1.10.0",
  "Description": "ASP.NET Core Minimal API + EF Core in one project",
  "Templates": ["Domain", "Contracts", "Infrastructure", "Api"],
  "Generators": ["SolidOps.Burgr.Essential.Generators.Objects.ObjectGenerator, SolidOps.Burgr.Essential"],
  "SupportedFeatures": ["aggregate_roots", "enums", "references", "navigations", "calculated_properties"],
  "Dependencies": [{ "Id": "Microsoft.EntityFrameworkCore", "Version": "10.0.*" }],
  "Harness": "harness",
  "ProjectFile": "Project.csproj",
  "Tokens": { "ModuleNamespace": "_MODULE_NAMESPACE_", "Module": "_MODULE_", "ModuleSlug": "_MODULE_SLUG_" }
}
```

| Field | Effect |
|---|---|
| `MinimumBurgrVersion` | An older burgr refuses to generate instead of producing code that does not compile; local development builds (`0.0.0-dev`) skip the check. |
| `Templates`, `Generators` | Used when `burgr.json` does not list its own. Without either, burgr uses the four `Burgr.Essential` generators (enums, objects, services, rights). `ModelParserEngineType` and `TemplateParserEngineType` default to `Burgr.Essential`'s. |
| `SupportedFeatures` | burgr refuses a model using a feature missing from the list, and names it: `aggregate_roots`, `entities`, `value_objects`, `transients`, `events`, `services`, `enums`, `references`, `external_references`, `navigations`, `synchronized_navigations`, `calculated_properties`, `calculated_collections`, `non_persisted_properties`, `primitive_collections`, `composite_keys`, `composite_unique_constraints`, `rights`, `rules`, `factories`, `ui_components`. Without the field, nothing is checked. |
| `Dependencies` | Packages the generated code needs (informative). |
| `Harness` | Folder of the set copied by `/harness:<directory>` (instructions and hooks for AI agents). |
| `ProjectFile` | Project file written for a C# module that has none yet, the module namespace token standing for `<NamespaceName>.<module>`. Without it, burgr writes a plain `net10.0` class library. An existing project file is never touched. A template folder can declare its own, see [Project files](#project-files). |
| `Tokens` | Names of the [module tokens](#module-tokens) used by the set's templates. |
| `EnumType` | How burgr names an enum it writes itself, in a method parameter for instance: `<module namespace><Prefix><Name><Suffix>`. `{ "Prefix": ".Contracts.Enums.", "Suffix": "Enum" }` by default; Onion uses `{ "Prefix": ".Domain.", "Suffix": "" }`. |

Template sets published on NuGet carry the `burgr-templates` tag.

---

## Writing templates

A template set is a directory containing:

- **`burgr.yaml`** — maps logical names to source files and output destinations.
- **Source files** (`.cs`, `.ts`, `.tpl`, …) — code templates annotated with generation markers.

Generation is deterministic: the same model and templates give byte-identical files on
any machine and OS. Templates can use LF or CRLF line endings; generated files are always
written with CRLF. The CI regenerates the [samples](#samples) and a reference case
covering the template options (`src/Burgr.Essential.Tests/Reference`) from scratch, under
Linux and a Turkish culture, and compares them byte for byte with the expected files.

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

**Property filters:** `[S]` simple type · `[E]` enum · `[M]` model ref · `[R]` external ref · `[NO]` normal · `[CA]` calculated · `[NP]` non-persisted · `[N]` nullable · `[NN]` non-nullable · `[AR]` array · `[NAR]` not array · `[SNA]` single nav · `[LNA]` list nav · `[SY]` / `[NSY]` synchronized nav or not · `[K]` / `[NK]` part of the composite key or not · `[BL]` / `[NBL]` list navigation with a `back_link_navigation` (and no `join_navigation`) or not · `[JN]` / `[NJN]` list navigation with a `join_navigation` or not

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
| `COMPOSED_ID` / `NOT_COMPOSED_ID` | Model has / has no composite `key` |
| `NO_ID_COLUMN_NAME` | Model does not declare `id_column_name` |
| `ZERO_IS_NO_REFERENCE` / `NOT_ZERO_IS_NO_REFERENCE` | `burgr.json` sets / does not set `"ZeroIsNoReference": true` |
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
| `_KEYINDEX_` | Position (from 0) of a `[K]` property in the composite key |
| `IDCOLUMNNAME` | `id_column_name` of the model |
| `_CALCULATED__*_` / `_NONPERSISTED__*_` / `_NAVIGATION__*_` | Variants for calculated / non-persisted / navigation properties |
| `_FORLIST__*_` / `_REF__*_` | Variants for list and cross-module properties |
| `DEPENDENCYNAMESPACE` | Root namespace of the referenced module |
| `MODELNAME` | Snake_case model name |
| `MINSIZE` / `FIELDSIZE` | Min / max field size |

### Module tokens

Unlike the tokens above, which are replaced inside loop regions, the module tokens are
replaced in the whole file, model names included. Their names are set by the `Tokens` of
`burgr-templates.json`:

| Role | Default | Recommended | Replaced with |
|---|---|---|---|
| `ModuleNamespace` | `MetaCorp.Template` | `_MODULE_NAMESPACE_` | Namespace of the module (`Acme.Library`) |
| `Module` | `Template` | `_MODULE_` | Short name of the module (`Library`) |
| `ModuleSlug` | `SlugTemplate` | `_MODULE_SLUG_` | Slug of the module (`library`) |

The defaults are kept for compatibility with existing template sets, but `Template` is an
ordinary word: a model `email_template` gives a class `EmailTemplate` that comes out as
`EmailLibrary`. A new template set declares the recommended names, as Mini does.

### Project files

A C# project that does not exist yet is created with the first of:

1. the `project_file` of the template folder writing to it, declared next to
   `source_templates` in its `burgr.yaml` (a path relative to the folder);
2. the `ProjectFile` of the template set's `burgr-templates.json`;
3. a plain `net10.0` class library.

With `force_separate_dll: true`, the templates of a folder write to a project of their own,
`<module>.<destination_suffix>`: a layered set gives each layer its project file (class
library, Web SDK, test project…). In a project file, the module namespace token stands for
the assembly name (`Acme.Library.Domain`) and the module token for the module without the
layer, to reference another layer:

```xml
<ProjectReference Include="../_MODULE_.Domain/_MODULE_.Domain.csproj" />
```

burgr refuses two template folders declaring different project files for the same project.
An existing project file is never touched.

### Framework files

The behavior that is the same for every model (base classes, unit of work, error
mapping…) belongs to the template set, not to the generated code. A template marked
`file_kind: framework` in `burgr.yaml` (default `generated`) holds such a file:

```yaml
source_templates:
  aggregate_root:
    source: AggregateRoot.cs.tpl
    file_suffix: AggregateRoot.cs
    destination_suffix: Domain
    destination_language: CS
    file_kind: framework
```

- It is written once per module, named after `file_suffix` alone with
  `FrameworkFileSuffix` (`AggregateRoot.f.cs`), and rewritten at each generation: a new
  version of the template set fixes it. A framework file the set no longer produces is
  deleted from its folder.
- It must not depend on the model: burgr refuses a framework template containing a
  `#region foreach`. Only the [module tokens](#module-tokens) are replaced.
- It is kept as written, blank lines and block comments included (other templates lose
  every `/*` and `*/`, which lets them write `_ENUMTYPE_/*_ISNULL_*/` and stay valid code):
  it is meant to be read. The hook of a template set's harness lets AI agents read it, and
  refuses to edit it.

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
| [`Samples/2. User management API`](src/Samples/2.%20User%20management%20API) | The Onion template set: Domain, Application, Infrastructure, Api and Tests projects, a class diagram, entities, a value object stored as JSON, services, factories and rules with hand-written implementations, in-process events, synchronized children, rights, JWT authentication, paging and sorting, integration tests |
| [`Samples/3. Mini API`](src/Samples/3.%20Mini%20API) | The default Mini template set: Minimal API + EF Core, a hand-written calculated property and validation rule, integration tests |

---

## License

[MIT](LICENSE)