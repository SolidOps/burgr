using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators;
using SolidOps.Burgr.Essential.Generators.Objects;
using SolidOps.Burgr.Essential.Generators.UseCases;
using SolidOps.Burgr.Essential.Yaml.Model;

namespace SolidOps.Burgr.Essential.Yaml;

public class ModelParserEngine : IModelParserEngine
{
    public bool IsInitialized { get; set; } = false;

    public string ModelParserType => "Yaml";

    public string ModelsDirectory { get; set; }
    public string ModuleName { get; set; }
    public string NamespaceName { get; set; }

    public Dictionary<string, YamlModelContentV1> YamlModelContentByModule { get; set; }

    public ModelParserEngine()
    {

    }

    public ModelParserEngine(string modelsDirectory, string moduleName, string namespaceName)
    {
        ModelsDirectory = modelsDirectory;
        ModuleName = moduleName;
        NamespaceName = namespaceName;
    }

    public void BeforeParsing()
    {
        if (!IsInitialized)
        {
            string assemblyFileName = null;

            List<string> patterns = new()
            {
                "*.yaml"
            };
            List<string> specFiles = new();
            foreach (string pattern in patterns)
            {
                var files = Directory.GetFiles(ModelsDirectory, pattern, SearchOption.AllDirectories);
                specFiles.AddRange(files);
            }

            if (specFiles.Count == 0)
            {
                throw new Exception("can't find any model in " + ModelsDirectory);
            }

            YamlModelContentByModule = new Dictionary<string, YamlModelContentV1>();

            foreach (string file in specFiles)
            {
                var fileInfo = new FileInfo(file);
                var directoryInfo = new DirectoryInfo(ModelsDirectory);

                string moduleName = fileInfo.DirectoryName.Replace(directoryInfo.FullName, string.Empty).Replace("\\", string.Empty);

                if (!YamlModelContentByModule.ContainsKey(moduleName))
                {
                    YamlModelContentByModule.Add(moduleName, new YamlModelContentV1());
                }
                YamlModelContentV1 yamlModelContent = YamlModelContentByModule[moduleName];

                var lines = File.ReadLines(file).ToList();
                var version = lines.First().Replace("burgr-model-version: ", string.Empty);

                lines.RemoveAt(0);

                if (version == "1.0.0")
                {
                    var fileContent = new YamlDotNet.Serialization.Deserializer().Deserialize<YamlModelContentV1>(String.Join("\r\n", lines));
                    foreach (var @enum in fileContent.enums)
                    {
                        yamlModelContent.enums.Add(@enum.Key, @enum.Value);
                    }
                    foreach (var entity in fileContent.entities)
                    {
                        yamlModelContent.entities.Add(entity.Key, entity.Value);
                    }
                    foreach (var aggregate_root in fileContent.aggregate_roots)
                    {
                        yamlModelContent.aggregate_roots.Add(aggregate_root.Key, aggregate_root.Value);
                    }
                    foreach (var transient in fileContent.transients)
                    {
                        yamlModelContent.transients.Add(transient.Key, transient.Value);
                    }
                    foreach (var use_case in fileContent.use_cases)
                    {
                        yamlModelContent.use_cases.Add(use_case.Key, use_case.Value);
                    }
                    foreach (var value_object in fileContent.value_objects)
                    {
                        yamlModelContent.value_objects.Add(value_object.Key, value_object.Value);
                    }
                    foreach (var @event in fileContent.events)
                    {
                        yamlModelContent.events.Add(@event.Key, @event.Value);
                    }
                }
            }

            IsInitialized = true;
        }
    }

    public void AfterParsing(ModelDescriptionsRepository modelsRepository)
    {
        // build exposed types
        List<string> exposedTypes = new();
        foreach (KeyValuePair<string, FullModelDescription> kvp in modelsRepository.modelDescriptions)
        {
            FullModelDescription description = kvp.Value;

            // for each model that has at least one resource
            foreach (ModelDescriptor model in description.ModelDescriptors.Where(m => m.DescriptorType == ObjectGenerator.Name))
            {
                if (model.GetChildren(DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR).Count() > 0 
                    || model.Get("DomainType") == DomainType.Transient.ToString())
                {
                    AddType(exposedTypes, model, modelsRepository);
                }
            }

            // foreach model as output or param of a use case
            foreach (ModelDescriptor useCase in description.ModelDescriptors.Where(m => m.DescriptorType == UseCaseGenerator.Name))
            {
                string[] descriptors = new string[] {
                    VoidUseCaseStepGenerator.Name,
                    SimpleUseCaseStepGenerator.Name,
                    ModelUseCaseStepGenerator.Name,
                    ModelListUseCaseStepGenerator.Name,
                    IdentityUseCaseStepGenerator.Name
                };
                foreach (ModelDescriptor step in useCase.GetChildren(descriptors))
                {
                    AddRelatedType(exposedTypes, step, modelsRepository);

                    foreach (ModelDescriptor parameter in step.GetChildren(DescriptorTypes.USECASE_STEP_PARAMETER_DESCRIPTOR))
                    {
                        AddRelatedType(exposedTypes, parameter, modelsRepository);
                    }
                }
            }
        }

        foreach (KeyValuePair<string, FullModelDescription> kvp in modelsRepository.modelDescriptions)
        {
            foreach (ModelDescriptor model in kvp.Value.ModelDescriptors.Where(m => m.DescriptorType == ObjectGenerator.Name))
            {
                model.Set("Exposed", exposedTypes.Contains(model.FullModuleName + "." + model.Name).ToString());
            }
        }
    }

    private void AddRelatedType(List<string> exposedTypes, ModelDescriptor descriptor, ModelDescriptionsRepository modelsRepository)
    {
        var related = descriptor.GetRelated("Object");
        if (related != null)
            AddType(exposedTypes, related, modelsRepository);
    }

    private void AddType(List<string> exposedTypes, ModelDescriptor descriptor, ModelDescriptionsRepository modelsRepository)
    {
        string type = descriptor.FullModuleName + "." + descriptor.Name;

        AddType(exposedTypes, type, modelsRepository);
    }

    private void AddType(List<string> exposedTypes, string type, ModelDescriptionsRepository modelsRepository)
    {
        if (type.EndsWith("[]"))
        {
            AddType(exposedTypes, type.Replace("[]", ""), modelsRepository);
        }
        else if (!exposedTypes.Contains(type))
        {
            ModelDescriptor model = GetModelObjectInList(type, modelsRepository.modelDescriptions);
            if (model != null)
            {
                exposedTypes.Add(type);
                foreach (ModelDescriptor property in model.GetChildren(PropertyGenerator.Name))
                {
                    if (property.Get("PropertyType") == "Model")
                    {
                        if (!property.Is("Private"))
                        {
                            AddType(exposedTypes, property.GetRelated("Object"), modelsRepository);
                        }
                    }
                }
            }
        }
    }

    public static ModelDescriptor GetModelObjectInList(string fullType, Dictionary<string, FullModelDescription> modelDescriptions)
    {
        foreach (KeyValuePair<string, FullModelDescription> kvp in modelDescriptions)
        {
            foreach (ModelDescriptor mo in kvp.Value.ModelDescriptors.Where(m => m.DescriptorType == ObjectGenerator.Name))
            {
                if (mo.FullModuleName + "." + mo.Name == fullType)
                {
                    return mo;
                }
            }
        }

        return null;
    }

    public static string GetModuleName(TypeInfo typeInfo, string namespaceName, string moduleName)
    {
        var subModule = GetSubModule(typeInfo, namespaceName, moduleName);

        if (subModule != null)
            return moduleName + "." + subModule;

        return moduleName;
    }

    private static string GetSubModule(TypeInfo typeInfo, string namespaceName, string moduleName)
    {
        throw new NotImplementedException();
    }

    public void Dispose() { }
}
