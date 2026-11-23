# Burgr.Templates.Mini

Burgr's default template set. From a YAML model, [burgr](https://github.com/SolidOps/burgr) generates one ASP.NET Core Minimal API project with EF Core: entities, DTOs, `DbContext` configuration and CRUD endpoints. The generated code depends on public packages only, and has no framework of its own.

It optionally ships an AI harness for coding agents (`AGENTS.md`, `CLAUDE.md`, a Claude Code hook that keeps agents out of the generated files): `dotnet burgr specs/ --harness=.` installs it. `AGENTS.md` lists the model features the set supports, what it generates and its extension points.

```sh
dotnet tool install --global Burgr
dotnet burgr specs/ --harness=.
```

No configuration is needed: Mini is the default set. When `specs/` is missing or empty, burgr first writes the set's starter project (a library of authors and books, taken from the sample), then generates it: `dotnet run --project src/Library` serves the API, browsable under `/swagger`. To pin it, set `TemplatePackage` in `burgr.json`.

- Website: <https://solidops.com/burgr>
- Quick start: <https://github.com/SolidOps/burgr#quick-start>
- Sample: <https://github.com/SolidOps/burgr-sample-mini>
- Licence: MIT-0. The code generated from these templates is yours, no attribution required.
