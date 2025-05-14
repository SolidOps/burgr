using SolidOps.Burgr.Core.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolidOps.Burgr
{
    public static class BurgrLauncher
    {
        private static List<Assembly> referencedAssemblies = new List<Assembly>();
        public static int Launch(params string[] args)
        {
            string moduleName = string.Empty;
            string namespaceName = string.Empty;
            string buildingDir = string.Empty;
            string modelSpecDirectory = Directory.GetCurrentDirectory();
            var binaryDirectories = new List<string>();
            string templateSpecDirectory = null;

            bool ObjectsMonitored = true;
            string identityKeysType = "Guid";
            string forcedPrefix = null;
            string overrideDestination = null;
            string modelTypeParserEngineType = "";
            string templateTypeParserEngineType = "";
            bool onlyOneDll = true;
            string generatedFilePrefix = "";
            string generatedFileSuffix = ".g";

            string toRemoveAtGenerationIdentifier = "to remove at generation";
            string toRemoveIfNotMonitoredIdentifier = "to remove if NOT_MONITORED";
            string toRemoveIfNoAPIIdentifier = "to remove if NO_API";

            string[] templates;
            string[] generatorTypeNames;

            string jsonFileContent = null;
            if (args.Length == 1 && !args[0].StartsWith("/") && args[0].StartsWith("{"))
            {
                jsonFileContent = args[0];
            }
            else if (File.Exists("Burgr.json"))
            {
                using (StreamReader reader = File.OpenText("Burgr.json"))
                {
                    jsonFileContent = reader.ReadToEnd();
                }
            }

            if (!string.IsNullOrEmpty(jsonFileContent))
            {
                JsonElement json = JsonDocument.Parse(jsonFileContent).RootElement.Clone();
                BurgrConfig config = json.Deserialize<BurgrConfig>();
                moduleName = config.ModuleName;
                namespaceName = config.NamespaceName;
                buildingDir = Path.Combine(Directory.GetCurrentDirectory(), config.BuildingDirectory);
                templates = config.Templates;
                generatorTypeNames = config.Generators;
                generatedFilePrefix = config.GeneratedFilePrefix;
                generatedFileSuffix = config.GeneratedFileSuffix;
                toRemoveAtGenerationIdentifier = config.ToRemoveAtGenerationIdentifier ?? "to remove at generation";
                toRemoveIfNotMonitoredIdentifier = config.ToRemoveIfNotMonitoredIdentifier ?? "to remove if NOT_MONITORED";
                toRemoveIfNoAPIIdentifier = config.ToRemoveIfNoAPIIdentifier ?? "to remove if NO_API";
                overrideDestination = config.OverrideDestination;

                if (config.ModelSpecDirectory != null)
                {
                    modelSpecDirectory = config.ModelSpecDirectory;
                    if (!modelSpecDirectory.Contains(":\\"))
                    {
                        modelSpecDirectory = Path.Combine(Directory.GetCurrentDirectory(), modelSpecDirectory);
                    }
                }
                if (config.BinaryDirectories != null)
                {
                    foreach (var binaryDirectory in config.BinaryDirectories)
                    {
                        var dir = binaryDirectory;
                        if (!dir.Contains(":\\"))
                        {
                            dir = Path.Combine(Directory.GetCurrentDirectory(), dir);
                        }
                        binaryDirectories.Add(dir);
                    }
                }
                if (config.TemplateSpecDirectory != null)
                {
                    templateSpecDirectory = config.TemplateSpecDirectory;
                    if (!templateSpecDirectory.Contains(":\\"))
                    {
                        templateSpecDirectory = Path.Combine(Directory.GetCurrentDirectory(), templateSpecDirectory);
                    }
                }
                if (config.ModelMonitored.HasValue)
                {
                    ObjectsMonitored = config.ModelMonitored.Value;
                }
                if (config.IdentityKeysType != null)
                {
                    identityKeysType = config.IdentityKeysType;
                }
                if (config.ForcedPrefix != null)
                {
                    forcedPrefix = config.ForcedPrefix;
                }
                if (config.OnlyOneDll.HasValue)
                {
                    onlyOneDll = config.OnlyOneDll.Value;
                }
                if (config.ModelParserEngineType != null)
                {
                    modelTypeParserEngineType = config.ModelParserEngineType;
                }
                if (config.TemplateParserEngineType != null)
                {
                    templateTypeParserEngineType = config.TemplateParserEngineType;
                }
                if (config.OverrideDestination != null)
                {
                    overrideDestination = config.OverrideDestination;
                }
            }
            else
            {
                foreach (string arg in args)
                {
                    if (arg.StartsWith("/projectDir:", StringComparison.Ordinal))
                    {
                        string projectDir = arg.Replace("/projectDir:", string.Empty);
                        moduleName = new DirectoryInfo(projectDir).Name;
                    }
                    if (arg.StartsWith("/buildingDir:", StringComparison.Ordinal))
                    {
                        buildingDir = arg.Replace("/buildingDir:", string.Empty);
                    }
                    if (arg.StartsWith("/dllFolderPath:", StringComparison.Ordinal))
                    {
                        binaryDirectories = arg.Replace("/dllFolderPath:", string.Empty).Split("|").ToList();
                    }
                    if (arg.StartsWith("/objectsMonitored:", StringComparison.Ordinal))
                    {
                        if (bool.TryParse(arg.Replace("/objectsMonitored:", string.Empty), out bool tempObjectsMonitored))
                        {
                            ObjectsMonitored = tempObjectsMonitored;
                        }
                    }
                    if (arg.StartsWith("/identityKeysType:", StringComparison.Ordinal))
                    {
                        identityKeysType = arg.Replace("/identityKeysType:", string.Empty);
                    }
                    if (arg.StartsWith("/forcedPrefix:", StringComparison.Ordinal))
                    {
                        forcedPrefix = arg.Replace("/forcedPrefix:", string.Empty);
                    }
                    if (arg.StartsWith("/overrideDestination:", StringComparison.Ordinal))
                    {
                        overrideDestination = arg.Replace("/overrideDestination:", string.Empty);
                    }
                    if (arg.StartsWith("/onlyOneDll:", StringComparison.Ordinal))
                    {
                        if (bool.TryParse(arg.Replace("/onlyOneDll:", string.Empty), out bool tempOnlyOneDll))
                        {
                            onlyOneDll = tempOnlyOneDll;
                        }
                    }
                }

                templates = new string[] {};

                generatorTypeNames = new string[] {};
            }

            foreach (string arg in args)
            {
                Console.Write(arg + " ");
            }

            if (!binaryDirectories.Any() && !string.IsNullOrEmpty(modelSpecDirectory))
            {
                binaryDirectories.Add(modelSpecDirectory);
            }

            Console.WriteLine("/dllFolderPath:" + string.Join('|', binaryDirectories));

            foreach (string dll in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                referencedAssemblies.Add(Assembly.LoadFrom(dll));

            foreach (var binaryDirectory in binaryDirectories)
            {
                foreach (string dll in Directory.GetFiles(binaryDirectory, "*.dll"))
                    referencedAssemblies.Add(Assembly.LoadFrom(dll));
            }

            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembliesForLauncher;

            Type modelParserEngineType = Type.GetType(modelTypeParserEngineType);
            if (modelParserEngineType == null)
            {
                throw new ArgumentException($"{modelTypeParserEngineType} could not be found");
            }

            Type templateParserEngineType = Type.GetType(templateTypeParserEngineType);
            if (templateParserEngineType == null)
            {
                throw new ArgumentException($"{templateTypeParserEngineType} could not be found");
            }

            List<IGenerator> generators = new List<IGenerator>();
            foreach (var generatorTypeName in generatorTypeNames)
            {
                var type = Type.GetType(generatorTypeName);
                if (type == null)
                {
                    throw new ArgumentException($"{generatorTypeName} could not be found");
                }
                generators.Add(Activator.CreateInstance(type) as IGenerator);
            }

            IModelParserEngine modelParserEngine = Activator.CreateInstance(modelParserEngineType) as IModelParserEngine;
            modelParserEngine.ModelsDirectory = modelSpecDirectory;
            modelParserEngine.ModuleName = moduleName;
            modelParserEngine.NamespaceName = namespaceName;

            ITemplateParserEngine templateParserEngine = Activator.CreateInstance(templateParserEngineType) as ITemplateParserEngine;
            templateParserEngine.TemplatesDirectory = templateSpecDirectory;
            templateParserEngine.ModuleName = moduleName;
            templateParserEngine.NamespaceName = namespaceName;

            AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembliesForLauncher;

            GeneratorSettings settings = new()
            {
                // input
                ModuleName = moduleName,
                NamespaceName = namespaceName,
                TemplateSpecDirectory = templateSpecDirectory ?? modelSpecDirectory,
                // internal
                ObjectsMonitored = ObjectsMonitored,
                IdentityKeysType = identityKeysType,
                ForcedPrefix = forcedPrefix,
                OverrideDestination = overrideDestination,
                BuildingDirectory = buildingDir,
                Templates = string.Join("|", templates),
                OnlyOneDll = onlyOneDll,
                ModelParserEngine = modelParserEngine,
                TemplateParserEngine = templateParserEngine,
                Generators = generators,
                GeneratedFilePrefix = generatedFilePrefix,
                GeneratedFileSuffix = generatedFileSuffix,
                ToRemoveAtGenerationIdentifier = toRemoveAtGenerationIdentifier,
                ToRemoveIfNotMonitoredIdentifier = toRemoveIfNotMonitoredIdentifier,
                ToRemoveIfNoAPIIdentifier = toRemoveIfNoAPIIdentifier
            };
            BurgrPlayer Burgr = new();
            Burgr.Init(settings);
            Burgr.ParseModels();
            Console.WriteLine($"Models parsed: {Burgr.modelsRepository.modelDescriptions.Values.SelectMany(v => v.ModelDescriptors).Count()}");
            Burgr.ParseTemplates();
            Console.WriteLine($"Templates parsed: {Burgr.sourceTemplates.Count()}");
            Burgr.Generate();

            return 0;
        }

        private static Assembly ResolveAssembliesForLauncher(object sender, ResolveEventArgs args)
        {
            foreach (var assembly in referencedAssemblies)
            {
                string assemblySimplifiedName = assembly.FullName.Split(",")[0];
                string argSimplifiedName = args.Name.Split(",")[0];
                if (assemblySimplifiedName == argSimplifiedName)
                    return assembly;
            }

            throw new Exception("Could not load assembly " + args.Name + " requested by " + args.RequestingAssembly);
        }
    }
}
