# Burgr.Templates.Onion

Burgr's complete template set. From a YAML model, [burgr](https://github.com/SolidOps/burgr) generates an ASP.NET Core API in onion architecture: Domain, Application, Infrastructure, Api and Tests projects, plus a PlantUML class diagram. It covers entities, value objects, services, transients, rights, rules, factories, in-process events, synchronized navigations and paged queries.

The invariant behaviour (unit of work, rights, events, paging...) lives in a light framework generated next to your code as `*.f.cs`: read it to understand a behaviour, extend it through its extension points, never edit it. The generated code depends on public packages only.

It optionally ships an AI harness for coding agents (`AGENTS.md`, `CLAUDE.md`, a Claude Code hook): `dotnet burgr specs/ /harness:.` installs it. `AGENTS.md` documents the model, the extension points and the framework contracts.

```json
{
  "ProjectName": "Library",
  "NamespaceName": "Acme",
  "BuildingDirectory": "../src/",
  "TemplatePackage": { "Id": "Burgr.Templates.Onion", "Version": "1.10.0" }
}
```

- Documentation: <https://github.com/SolidOps/burgr#template-sources>
- Sample: <https://github.com/SolidOps/burgr-sample-onion>
- Licence: MIT-0. The code generated from these templates is yours, no attribution required.
