using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Outputs;
using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr
{
    public class OutputGenerator
    {
        private readonly Dictionary<string, FullModelDescription> modelDescriptions;
        private readonly Dictionary<string, IGenerator> generators;
        private readonly GeneratorSettings generatorSettings;
        public string[] SourceTemplateFileSuffixDebugFilters = new string[]
        {
            // "Repository.cs"
        };

        public string[] ModelDebugFilters = new string[]
        {
            // "customer", "address"
        };

        public OutputGenerator(Dictionary<string, FullModelDescription> modelDescriptions, Dictionary<string, IGenerator> generators, GeneratorSettings generatorSettings)
        {
            this.modelDescriptions = modelDescriptions;
            this.generators = generators;
            this.generatorSettings = generatorSettings;
        }

        public Dictionary<string, ModuleGenerationResult> GenerateOutput(List<SourceTemplate> sourceTemplates)
        {
            Dictionary<string, ModuleGenerationResult> moduleGenerationResults = new();
            foreach (SourceTemplate sourceTemplate in sourceTemplates)
            {
                if (sourceTemplate.TemplateDescriptors == null)
                {
                    continue;
                }

                // filter for debug
                if (SourceTemplateFileSuffixDebugFilters != null && SourceTemplateFileSuffixDebugFilters.Length > 0 && !SourceTemplateFileSuffixDebugFilters.Contains(sourceTemplate.FileSuffix))
                {
                    continue;
                }

                foreach (KeyValuePair<string, FullModelDescription> modelDescriptions in modelDescriptions)
                {
                    if (modelDescriptions.Key.StartsWith(GeneratorOptions.NamespaceName + "."))
                    {
                        ModuleGenerationResult moduleGenerationResult = GetModuleGenerationResult(moduleGenerationResults, modelDescriptions.Key);
                        List<ModelDescriptor> modelDescriptors = modelDescriptions.Value.ModelDescriptors;
                        List<TemplateDescriptor> templateDescriptors = sourceTemplate.TemplateDescriptors;


                        Dictionary<string, IGenerator>.ValueCollection generatorList = generators.Values;
                        for(var ii = 0; ii < templateDescriptors.Count; ii++)
                        {
                            var template = templateDescriptors[ii];
                            var topDescriptorType = template.DescriptorType;
                            var topContent = template.Content;
                            var topLanguage = template.DestinationLanguage;
                            IGenerator generator = generatorList.SingleOrDefault(r => r.DescriptorType == template.DescriptorType);
                            if (generator == null)
                            {
                                continue;
                            }

                            List<ModelDescriptor> models = modelDescriptors.Where(md => md.DescriptorType == template.DescriptorType).ToList();
                            if (sourceTemplate.IsGereratePerModelDescription && sourceTemplate.ChildrenLevel.HasValue)
                            {
                                for (var i = 0; i < sourceTemplate.ChildrenLevel.Value; i++)
                                {
                                    template = template.GetChildren().Single();
                                    var descriptorType = template.DescriptorType;
                                    models = models.SelectMany(m => m.GetChildren(descriptorType)).ToList();
                                    if (!models.Any())
                                    {
                                        break;
                                    }
                                    generator = generator.SubGenerators.Single(s => s.DescriptorType == descriptorType);
                                    
                                }
                            }
                            generator.BeforeGenerate(models, template);
                            foreach (ModelDescriptor model in models)
                            {
                                // filter for debug
                                if (ModelDebugFilters != null && ModelDebugFilters.Length > 0 && !ModelDebugFilters.Contains(model.Name))
                                {
                                    continue;
                                }

                                FileGenerationResult fileResult = GetFileGenerationResult(sourceTemplate, model.FullModuleName, moduleGenerationResult, model);
                                FinalGenerationResult genResult = GetFinalGenerationResult(topDescriptorType, topContent, fileResult);
                                string result = generator.Generate(template.Content, model, template, sourceTemplate.SourceTemplateOptions?.ModelPrefix, sourceTemplate.SourceTemplateOptions?.ModelSuffix) + Utilities.SingleNewLine;
                                result = generator.Clean(result, model, template);
                                genResult.FinalContent += result;
                            }

                            foreach (KeyValuePair<string, FileGenerationResult> kvp in moduleGenerationResult.FileGenerationResults)
                            {
                                var fileResult = kvp.Value;
                                if (fileResult.DestinationLanguage == topLanguage)
                                {
                                    FinalGenerationResult modelResult = GetFinalGenerationResult(topDescriptorType, topContent, kvp.Value);
                                    modelResult.FinalContent = generator.AfterGenerate(modelResult.FinalContent, template);
                                }
                            }
                        }
                    }
                }
            }

            return moduleGenerationResults;
        }

        public ModuleGenerationResult GetModuleGenerationResult(Dictionary<string, ModuleGenerationResult> moduleGenerationResults, string moduleName)
        {
            if (!moduleGenerationResults.ContainsKey(moduleName))
            {
                moduleGenerationResults.Add(moduleName, new ModuleGenerationResult
                {
                    FileGenerationResults = new Dictionary<string, FileGenerationResult>()
                });
            }
            return moduleGenerationResults[moduleName];
        }

        public FileGenerationResult GetFileGenerationResult(SourceTemplate sourceTemplate, string moduleName, ModuleGenerationResult moduleGenerationResult, ModelDescriptor descriptor)
        {
            string fileName = Utilities.GetFileName(moduleName, sourceTemplate.FileSuffix, sourceTemplate.PathStyle, generatorSettings.GeneratedFilePrefix, generatorSettings.GeneratedFileSuffix);
            if (sourceTemplate.IsGereratePerModelDescription)
            {
                fileName = Utilities.GetFileName(descriptor.Name, sourceTemplate.FileSuffix, sourceTemplate.PathStyle, generatorSettings.GeneratedFilePrefix, generatorSettings.GeneratedFileSuffix);
            }

            string destinationFolder = sourceTemplate.DestinationPath;
            if (sourceTemplate.OverrideDestinationFolder != null)
            {
                destinationFolder = Utilities.CombinePath(destinationFolder, sourceTemplate.OverrideDestinationFolder);
            }

            string currentModuleName = moduleName;
            if (currentModuleName.Contains("."))
            {
                int firstDotIndex = currentModuleName.IndexOf(".");
                currentModuleName = currentModuleName[(firstDotIndex + 1)..];
            }
            if (sourceTemplate.PathStyle == "Lower")
            {
                currentModuleName = CommonTags.ConvertToJSLibrary(moduleName) + "-lib";
            }

            string modulePath;
            string finalModuleName;
            if (GeneratorOptions.OnlyOneDll && !sourceTemplate.ForceSeparateDll)
            {
                if (sourceTemplate.OverrideDestinationFolder == null)
                {
                    modulePath = Utilities.CombinePath(destinationFolder, currentModuleName);
                }
                else
                {
                    modulePath = destinationFolder;
                }
                finalModuleName = currentModuleName;
                List<string> paths = new() { modulePath };
                paths.AddRange(sourceTemplate.DestinationSuffix.Split("|"));
                destinationFolder = Utilities.CombinePath(paths.ToArray());
            }
            else
            {
                if (sourceTemplate.DestinationSuffix.Contains("|"))
                {
                    var parts = sourceTemplate.DestinationSuffix.Split("|");
                    finalModuleName = currentModuleName + "." + parts[0];
                    var subPath = sourceTemplate.DestinationSuffix.Substring(parts[0].Length + 1);
                    List<string> paths = new() { finalModuleName };
                    paths.AddRange(subPath.Split("|"));
                    modulePath = Utilities.CombinePath(destinationFolder, finalModuleName);
                    destinationFolder = Utilities.CombinePath(destinationFolder, Utilities.CombinePath(paths.ToArray()));
                }
                else
                {
                    List<string> paths = new() { currentModuleName };
                    paths.Add(sourceTemplate.DestinationSuffix);
                    finalModuleName = string.Join(".", paths);
                    destinationFolder = Utilities.CombinePath(destinationFolder, finalModuleName);
                    modulePath = destinationFolder;
                }
            }

            string filePath = Path.Combine(destinationFolder, fileName);
            filePath = AdjustFilePath(filePath, descriptor, sourceTemplate, destinationFolder, moduleName);

            if (!moduleGenerationResult.FileGenerationResults.ContainsKey(filePath))
            {
                FileGenerationResult tempFileResult = new(Utilities.Instance.Counter++)
                {
                    ModulePath = modulePath,
                    ModuleName = finalModuleName,
                    NamespaceName = generatorSettings.NamespaceName,
                    InitialContent = sourceTemplate.Content,
                    FinalGenerationResults = new Dictionary<string, Dictionary<string, FinalGenerationResult>>(),

                    OverwriteIfExist = sourceTemplate.OverwriteIfExist,
                    DestinationLanguage = sourceTemplate.DestinationLanguage,
                    RemoveConsecutiveLineBreaks = sourceTemplate.RemoveConsecutiveLineBreaks,
                    EmptyFolder = sourceTemplate.IsGereratePerModelDescription
                };

                moduleGenerationResult.FileGenerationResults.Add(filePath, tempFileResult);
            }
            return moduleGenerationResult.FileGenerationResults[filePath];
        }

        protected virtual string AdjustFilePath(string filePath, ModelDescriptor descriptor, SourceTemplate sourceTemplate, string destinationFolder, string moduleName)
        {
            return filePath;
        }


        private static FinalGenerationResult GetFinalGenerationResult(string descriptorType, string templateContent, FileGenerationResult fileResult)
        {
            if (!fileResult.FinalGenerationResults.ContainsKey(descriptorType))
            {
                fileResult.FinalGenerationResults.Add(descriptorType, new Dictionary<string, FinalGenerationResult>());
            }

            if (!fileResult.FinalGenerationResults[descriptorType].ContainsKey(templateContent))
            {
                FinalGenerationResult tempEnumResult = new(Utilities.Instance.Counter++, descriptorType)
                {
                    InitialContent = templateContent,
                    FinalContent = string.Empty
                };
                fileResult.FinalGenerationResults[descriptorType].Add(templateContent, tempEnumResult);
            }
            return fileResult.FinalGenerationResults[descriptorType][templateContent];
        }
    }
}
