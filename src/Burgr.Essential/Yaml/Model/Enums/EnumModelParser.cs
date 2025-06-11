using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Enums;

namespace SolidOps.Burgr.Essential.Yaml.Model.Enums;

public class EnumModelParser : BaseYamlModelParser, IModelParser
{
    private ModelParserEngine yamlParserEngine;

    public EnumModelParser()
    {
        DefaultDescriptorType = EnumGenerator.Name;
    }

    public void ParseModel(IModelParserEngine parserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators)
    {
        yamlParserEngine = parserEngine as ModelParserEngine;
        this.modelsRepository = modelsRepository;

        foreach (var kvpModule in yamlParserEngine.YamlModelContentByModule)
        {
            string moduleName = yamlParserEngine.ModuleName;
            if(kvpModule.Key != string.Empty)
                moduleName += "." + kvpModule.Key;
            foreach (var kvp in kvpModule.Value.enums)
            {
                _ = GetOrCreateDescriptor(kvp.Key, kvp.Value, yamlParserEngine.NamespaceName, moduleName);
            }
        }
    }

    public ModelDescriptor GetOrCreateDescriptor(string name, @enum value, string namespaceName, string moduleName)
    {
        ModelDescriptor descriptor = FindModelDescriptor(namespaceName, moduleName, EnumGenerator.Name, name);
        if (descriptor == null)
        {
            descriptor = CreateDescriptor(name, value, namespaceName, moduleName);
        }
        return descriptor;
    }

    public ModelDescriptor CreateDescriptor(string name, @enum value, string namespaceName, string moduleName)
    {
        if (name.ToLower() != name)
        {
            throw new Exception($"enum name must be in lower case : {name}");
        }

        ModelDescriptor descriptor = base.CreateDescriptor(name, EnumGenerator.Name, namespaceName, moduleName);

        Dictionary<int, string> dicValues = new();
        foreach (var kvp in value.values)
        {
            dicValues.Add(kvp.Value, kvp.Key);
        }
        descriptor.Set("EnumType", name);

        foreach (KeyValuePair<int, string> item in dicValues)
        {
            ModelDescriptor enumValueDescriptor = new(item.Value, "EnumValue");
            enumValueDescriptor.Set("Key", item.Key.ToString());
            descriptor.AddChild(enumValueDescriptor);
        }

        return descriptor;
    }
}
