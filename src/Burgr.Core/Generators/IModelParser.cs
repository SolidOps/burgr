namespace SolidOps.Burgr.Core.Generators
{
    public interface IModelParser
    {
        void ParseModel(IModelParserEngine modeParserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators);
    }
}