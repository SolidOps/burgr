using SolidOps.Burgr.Core.Descriptors;

namespace SolidOps.Burgr.Core.Generators
{
    public interface IGenerator
    {
        string DescriptorType { get; }

        string AimedModelDescriptorType { get; }

        string ModelParserType { get; }

        void SetModelParserType(string modelParserType);

        ITemplateParser TemplateParser { get; }

        IModelParser GetModelParser(string modelParserType);
        
        void ParseModel(IModelParserEngine modeParserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators);

        List<IGenerator> SubGenerators { get; }

        void BeforeGenerate(List<ModelDescriptor> models, TemplateDescriptor template);
        string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix);
        string Clean(string content, ModelDescriptor model, TemplateDescriptor template);
        string AfterGenerate(string content, TemplateDescriptor template);
    }
}
