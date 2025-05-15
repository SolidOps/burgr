using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Common;

namespace SolidOps.Burgr.Essential.Yaml.Model.Common;

public class ConsumedEventModelParser : BaseYamlModelParser, IModelParser
{
    private ModelParserEngine assemblyParserEngine;

    public ConsumedEventModelParser()
    {
        DefaultDescriptorType = ConsumedEventGenerator.Name;
    }

    public void ParseModel(IModelParserEngine modeParserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators)
    {
        
    }
}
