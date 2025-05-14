using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Outputs;
using SolidOps.Burgr.Core.Template;
using SolidOps.SubZero;
using System.Reflection;

namespace SolidOps.Burgr
{
    public class GeneratorSettings
    {
        public string ModuleName { get; set; }
        public string NamespaceName { get; set; }
        public string BuildingDirectory { get; set; }
        public string TemplateSpecDirectory { get; set; }
        public bool ObjectsMonitored { get; set; }
        public string IdentityKeysType { get; set; } = "Guid";
        public string ForcedPrefix { get; set; }
        public string OverrideDestination { get; set; }
        public bool OnlyOneDll { get; set; } = true;
        public string Templates { get; set; } = null;
        public List<IGenerator> Generators { get; set; } = new List<IGenerator>();

        public IModelParserEngine ModelParserEngine { get; set; }
        public ITemplateParserEngine TemplateParserEngine { get; set; }

        public string GeneratedFilePrefix { get; set; }

        public string GeneratedFileSuffix { get; set; }

        public string ToRemoveAtGenerationIdentifier { get; set; } = "to remove at generation";
        public string ToRemoveIfNotMonitoredIdentifier { get; set; } = "to remove if NOT_MONITORED";
        public string ToRemoveIfNoAPIIdentifier { get; set; } = "to remove if NO_API";

    }

    public class BurgrPlayer : IDisposable
    {
        private readonly List<Assembly> templateAssemblies = new();
        public readonly ModelDescriptionsRepository modelsRepository;
        private string templatesDirectory;
        private Dictionary<string, IGenerator> generators;
        private List<string> Templates = null;
        public List<SourceTemplate> sourceTemplates = null;
        private IModelParserEngine modelParserEngine;
        private ITemplateParserEngine templateParserEngine;

        private List<string> additionalLoopIdentifiers = new List<string>();

        private GeneratorSettings generatorSettings;

        public BurgrPlayer()
        {
            this.modelsRepository = new ModelDescriptionsRepository();
        }

        public void Init(GeneratorSettings settings)
        {
            generatorSettings = settings;

            additionalLoopIdentifiers.Add(settings.ToRemoveAtGenerationIdentifier);
            additionalLoopIdentifiers.Add(settings.ToRemoveIfNotMonitoredIdentifier);
            additionalLoopIdentifiers.Add(settings.ToRemoveIfNoAPIIdentifier);

            if (generatorSettings.GeneratedFilePrefix == null)
                generatorSettings.GeneratedFilePrefix = string.Empty;

            if (generatorSettings.GeneratedFileSuffix == null)
                generatorSettings.GeneratedFileSuffix = ".g";

            GeneratorOptions.BuildingDirectory = settings.BuildingDirectory;

            GeneratorOptions.ModuleName = settings.ModuleName;
            GeneratorOptions.NamespaceName = settings.NamespaceName;

            GeneratorOptions.ModelMonitored = settings.ObjectsMonitored;
            GeneratorOptions.IdentityKeysType = settings.IdentityKeysType;
            GeneratorOptions.ForcedPrefix = settings.ForcedPrefix;

            GeneratorOptions.OverrideDestinations = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(settings.OverrideDestination))
            {
                string[] templateAndValues = settings.OverrideDestination.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string templateAndValue in templateAndValues)
                {
                    string[] parts = templateAndValue.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string template = parts[0];
                        string value = parts[1];
                        GeneratorOptions.OverrideDestinations.Add(template, value);
                    }
                }
            }

            GeneratorOptions.OnlyOneDll = settings.OnlyOneDll;
            templatesDirectory = settings.TemplateSpecDirectory;
            if (string.IsNullOrEmpty(templatesDirectory))
            {
                templatesDirectory = Directory.GetCurrentDirectory();
            }

            if (settings.Templates != null)
            {
                Templates = settings.Templates.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            modelParserEngine = settings.ModelParserEngine;
            templateParserEngine = settings.TemplateParserEngine;

            generators = new Dictionary<string, IGenerator>();
            foreach (IGenerator res in settings.Generators)
            {
                generators.Add(res.DescriptorType, res);
            }
        }

        public void ParseTemplates(string filter = null)
        {
            templateParserEngine.Init();
            sourceTemplates = templateParserEngine.Parse(templatesDirectory, Templates, generators, filter);
        }

        public void ParseModels(params string[] filters)
        {
            modelParserEngine.BeforeParsing();
            
            foreach (KeyValuePair<string, IGenerator> generator in generators)
            {
                generator.Value.ParseModel(modelParserEngine, modelsRepository, generators.Values.ToList());
            }

            modelParserEngine.AfterParsing(modelsRepository);

        }

        public void Generate()
        {
            OutputGenerator generator = new(modelsRepository.modelDescriptions, generators, generatorSettings);
            Dictionary<string, ModuleGenerationResult> moduleGenerationResults = generator.GenerateOutput(sourceTemplates);
            CleanGenerationResults(moduleGenerationResults);
            GenerateFiles(moduleGenerationResults);
        }

        private void CleanGenerationResults(Dictionary<string, ModuleGenerationResult> moduleGenerationResults)
        {
            foreach (KeyValuePair<string, ModuleGenerationResult> kvpModule in moduleGenerationResults)
            {
                foreach (KeyValuePair<string, FileGenerationResult> kvpFile in kvpModule.Value.FileGenerationResults)
                {
                    foreach (IGenerator generator in generators.Values)
                    {
                        Dictionary<string, Dictionary<string, FinalGenerationResult>> finalGenerationResults = kvpFile.Value.FinalGenerationResults;
                        if (!finalGenerationResults.ContainsKey(generator.DescriptorType) || finalGenerationResults[generator.DescriptorType].Values.Count == 0)
                        {
                            string finalContent = kvpFile.Value.InitialContent;
                            if (generator.TemplateParser != null && finalContent.Contains(Utilities.GetStartLoopIdentifier(generator.TemplateParser.LoopIdentifier)))
                            {
                                foreach (string rightTemplate in Utilities.GetInnerTemplates(finalContent, Utilities.GetLoopIdentifiers(generator.TemplateParser.LoopIdentifier)))
                                {
                                    finalContent = finalContent.Replace(rightTemplate, Utilities.SingleNewLine);
                                }
                            }

                            kvpFile.Value.InitialContent = finalContent;
                        }
                    }
                }
            }
        }

        private static string RemoveConsecutiveLineBreaks(string content)
        {
            string finalContent = string.Empty;
            var blocks = ChunksUpto(content, 10000).ToList();
            for (var i = 0; i < blocks.Count - 1; i++)
            {
                var s = blocks[i];
                var index = s.LastIndexOf("\n");
                if (index > 0)
                {
                    var sub = s.Substring(index + 1);
                    blocks[i] = s.Substring(0, index + 1);
                    blocks[i + 1] = sub + blocks[i + 1];
                }
            }
            int count = 0;
            foreach (var block in blocks)
            {
                count++;
                bool previousEmtpy = true;
                string blockContent = string.Empty;
                foreach (string line in block.Split(Utilities.SingleNewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (string subline in line.Split(Utilities.AltNewLine, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (subline.Trim() == string.Empty)
                        {
                            if (!previousEmtpy)
                            {
                                previousEmtpy = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            previousEmtpy = false;
                        }

                        if (blockContent != string.Empty)
                        {
                            blockContent += Utilities.SingleNewLine;
                        }
                        if (!previousEmtpy)
                            blockContent += subline;
                    }
                }
                finalContent += blockContent;
                if (count < blocks.Count)
                    finalContent += Utilities.SingleNewLine;
            }
            return finalContent;
        }

        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        private void GenerateFiles(Dictionary<string, ModuleGenerationResult> moduleGenerationResults)
        {
            List<string> emptiedFolder = new List<string>();
            Dictionary<string, string> deletedFileContent = new Dictionary<string, string>();

            foreach (KeyValuePair<string, ModuleGenerationResult> moduleResult in moduleGenerationResults)
            {
                foreach (KeyValuePair<string, FileGenerationResult> fileResult in moduleResult.Value.FileGenerationResults)
                {
                    FileGenerationResult result = fileResult.Value;
                    string fileContent = result.InitialContent;

                    foreach (KeyValuePair<string, Dictionary<string, FinalGenerationResult>> kvp in result.FinalGenerationResults)
                    {
                        foreach (KeyValuePair<string, FinalGenerationResult> kvp2 in kvp.Value)
                        {
                            FinalGenerationResult genResult = kvp2.Value;
                            fileContent = Utilities.ReplaceContent(fileContent, genResult, result.DestinationLanguage);
                        }
                    }

                    string slug = TextHelper.GenerateSlug(moduleResult.Key.Split(".").Last());
                    fileContent = fileContent.Replace(CommonTags.Slug + CommonTags.ShortModule, slug);
                    fileContent = fileContent.Replace(CommonTags.Namespace, moduleResult.Key);
                    fileContent = fileContent.Replace(CommonTags.Namespace, moduleResult.Key);
                    fileContent = fileContent.Replace(CommonTags.ShortModule, moduleResult.Key.Split(".").Last());
                    fileContent = fileContent.Replace("/*", string.Empty);
                    fileContent = fileContent.Replace("*/", string.Empty);
                    fileContent = fileContent.Replace("using SolidOps.Burgr.Core;", string.Empty);
                    
                    // Clean import
                    if (fileResult.Value.DestinationLanguage == "JS")
                    {
                        List<string> lines = new();
                        foreach (string line in fileContent.Split(new char[] { '\n' }))
                        {
                            if (line.StartsWith("import {"))
                            {
                                if (!lines.Contains(line))
                                {
                                    lines.Add(line);
                                }
                            }
                            else
                            {
                                lines.Add(line);
                            }
                        }

                        fileContent = string.Join("\n", lines);
                    }

                    foreach (string removeTemplate in Utilities.GetInnerTemplates(fileContent, Utilities.GetLoopIdentifiers(generatorSettings.ToRemoveAtGenerationIdentifier)))
                    {
                        fileContent = fileContent.Replace(removeTemplate, Utilities.SingleNewLine);
                    }

                    if (!GeneratorOptions.ModelMonitored)
                    {
                        foreach (string removeTemplate in Utilities.GetInnerTemplates(fileContent, Utilities.GetLoopIdentifiers(generatorSettings.ToRemoveIfNotMonitoredIdentifier)))
                        {
                            fileContent = fileContent.Replace(removeTemplate, Utilities.SingleNewLine);
                        }
                    }

                    if (moduleResult.Value.NoneExposed)
                    {
                        foreach (string removeTemplate in Utilities.GetInnerTemplates(fileContent, Utilities.GetLoopIdentifiers(generatorSettings.ToRemoveIfNoAPIIdentifier)))
                        {
                            fileContent = fileContent.Replace(removeTemplate, Utilities.SingleNewLine);
                        }
                    }

                    fileContent = RemoveLoopIdentifiers(fileContent);

                    fileContent = fileContent.Replace(CommonTags.IdentityKeyType, GeneratorOptions.IdentityKeysType);

                    if (!GeneratorOptions.ModelMonitored)
                    {
                        foreach (string removeTemplate in Utilities.GetInnerTemplates(fileContent, Utilities.GetLoopIdentifiers(generatorSettings.ToRemoveIfNotMonitoredIdentifier)))
                        {
                            fileContent = fileContent.Replace(removeTemplate, Utilities.SingleNewLine);
                        }
                    }

                    fileContent = RemoveLoopIdentifiers(fileContent);

                    if (fileResult.Value.RemoveConsecutiveLineBreaks)
                    {
                        fileContent = RemoveConsecutiveLineBreaks(fileContent);
                    }

                    string projectFilePath = null;
                    switch (fileResult.Value.DestinationLanguage)
                    {
                        case "CS":
                            projectFilePath = Utilities.CombinePath(fileResult.Value.ModulePath, fileResult.Value.ModuleName) + ".csproj";
                            break;
                        default:
                            break;
                    }

                    if (projectFilePath != null && !File.Exists(projectFilePath))
                    {
                        FileInfo fileInfo = new(projectFilePath);
                        if (!fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }

                        if (fileResult.Value.ModuleName.EndsWith("Contracts"))
                        {
                            File.AppendAllText(projectFilePath, $@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AssemblyName>{fileResult.Value.NamespaceName}.{fileResult.Value.ModuleName}</AssemblyName>
    <RootNamespace>{fileResult.Value.NamespaceName}.{fileResult.Value.ModuleName}</RootNamespace>
  </PropertyGroup>

  <Import Project=""../version.proj"" />

  <ItemGroup>
    <PackageReference Include=""SolidOps.NORAD.Core"" Version=""1.0.4"" />
  </ItemGroup>

</Project>
");
                        }
                        else
                        {
                            File.AppendAllText(projectFilePath, $@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AssemblyName>{fileResult.Value.NamespaceName}.{fileResult.Value.ModuleName}</AssemblyName>
    <RootNamespace>{fileResult.Value.NamespaceName}.{fileResult.Value.ModuleName}</RootNamespace>
  </PropertyGroup>

  <Import Project=""../version.proj"" />

  <ItemGroup>
    <PackageReference Include=""SolidOps.NORAD.Core"" Version=""1.0.4"" />
    <ProjectReference Include=""..\{fileResult.Value.ModuleName}.Contracts\{fileResult.Value.ModuleName}.Contracts.csproj"" />
  </ItemGroup>

</Project>
");
                        }
                    }

                    // Empty folder
                    string fileDirectory = new FileInfo(fileResult.Key).DirectoryName;
                    if (result.EmptyFolder && !emptiedFolder.Contains(fileDirectory) && Directory.Exists(fileDirectory))
                    {
                        foreach (var file in Directory.GetFiles(fileDirectory, "*" + generatorSettings.GeneratedFileSuffix + ".*", SearchOption.AllDirectories))
                        {
                            deletedFileContent.Add(file, File.ReadAllText(file));
                            File.Delete(file);
                        }
                        emptiedFolder.Add(fileDirectory);
                    }

                    if (!fileResult.Value.OverwriteIfExist && File.Exists(fileResult.Key))
                    {
                        continue;
                    }

                    var filePath = new FileInfo(fileResult.Key).FullName;

                    if (!File.Exists(filePath) || File.ReadAllText(filePath) != fileContent)
                    {
                        if (fileContent.Trim().Length == 0)
                            continue;

                        string message = $"Create generated file {filePath}";
                        if (File.Exists(filePath) && !deletedFileContent.ContainsKey(filePath))
                        {
                            message = $"Update generated file {filePath}";
                            File.Delete(filePath);
                        }
                        FileInfo info = new(filePath);
                        _ = Directory.CreateDirectory(info.DirectoryName);

                        if (ShouldFileBeWritten(fileContent, result.DestinationLanguage))
                        {
                            File.AppendAllText(filePath, fileContent);
                            if (!deletedFileContent.ContainsKey(filePath) || deletedFileContent[filePath] != fileContent)
                            {
                                Console.WriteLine(message);
                            }

                            if (deletedFileContent.ContainsKey(filePath))
                            {
                                deletedFileContent.Remove(filePath);
                            }
                        }
                    }
                }
            }

            foreach (var filePath in deletedFileContent)
            {
                Console.WriteLine($"Delete generated file {filePath.Key}");
            }
        }

        private bool ShouldFileBeWritten(string content, string language)
        {
            if (content.Trim().Length == 0)
                return false;

            foreach (var line in content.Split("\r\n"))
            {
                if (!line.Trim().StartsWith(BaseGenerator.GetCommentMarkerStart(language)))
                    return true;
            }

            return false;
        }

        private string RemoveLoopIdentifiers(string fileContent)
        {
            foreach (string identifier in additionalLoopIdentifiers)
            {
                fileContent = fileContent.Replace(Utilities.GetStartLoopIdentifier(identifier), @"");
                fileContent = fileContent.Replace(Utilities.GetEndLoopIdentifier(identifier), @"");
            }

            foreach (IGenerator generator in generators.Values)
            {
                fileContent = CleanForGenerator(generator, fileContent);
            }

            return fileContent;
        }

        public string CleanForGenerator(IGenerator generator, string fileContent)
        {
            if (generator.TemplateParser != null)
            {
                fileContent = fileContent.Replace(Utilities.GetStartLoopIdentifier(generator.TemplateParser.LoopIdentifier), @"");
                fileContent = fileContent.Replace(Utilities.GetEndLoopIdentifier(generator.TemplateParser.LoopIdentifier), @"");
                foreach (var additionalIdentifier in generator.TemplateParser.AdditionalLoopIdentifiers)
                {
                    fileContent = fileContent.Replace(Utilities.GetStartLoopIdentifier(additionalIdentifier), @"");
                    fileContent = fileContent.Replace(Utilities.GetEndLoopIdentifier(additionalIdentifier), @"");
                }
                if (generator.SubGenerators != null && generator.SubGenerators.Count > 0)
                {
                    foreach (var sg in generator.SubGenerators)
                    {
                        if (sg.TemplateParser != null)
                        {
                            fileContent = CleanForGenerator(sg, fileContent);
                        }
                    }
                }
            }

            return fileContent;
        }

        public void Dispose()
        {
            this.modelParserEngine.Dispose();
        }
    }
}
