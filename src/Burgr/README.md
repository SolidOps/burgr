# Burgr

A deterministic, model-driven code generator for `dotnet`: a small YAML model and a set of templates in, source code out (C#, TypeScript, SQL, PlantUML...). Same model + same templates = same output, with nothing to review in the generated code.

```sh
dotnet tool install --global Burgr
dotnet burgr specs/
```

When `specs/` is missing or empty, burgr first writes a starter project (a library of authors and books: the model in `specs/`, `Program.cs` in `src/Library/`), then generates it: `dotnet run --project src/Library` serves the API, browsable under `/swagger`. Without any configuration, burgr uses the `Burgr.Templates.Mini` set (ASP.NET Core Minimal API + EF Core). `Burgr.Templates.Onion` generates a complete onion architecture. Optionally, `--harness=.` installs the AI harness of the template set (`AGENTS.md`, `CLAUDE.md`, a hook keeping coding agents out of generated files): the AI writes the model, burgr writes the code, the tests decide.

- Website: <https://solidops.com/burgr>
- Quick start, YAML reference, configuration: <https://github.com/SolidOps/burgr#readme>
- Template sets: [`Burgr.Templates.Mini`](https://www.nuget.org/packages/Burgr.Templates.Mini), [`Burgr.Templates.Onion`](https://www.nuget.org/packages/Burgr.Templates.Onion)
- Engine and extension points: [`Burgr.Core`](https://www.nuget.org/packages/Burgr.Core), [`Burgr.Essential`](https://www.nuget.org/packages/Burgr.Essential)
