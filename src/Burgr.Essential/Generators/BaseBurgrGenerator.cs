using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.ConversionServices;

namespace SolidOps.Burgr.Essential.Generators;

public abstract class BaseBurgrGenerator : BaseGenerator
{
    public Dictionary<string, IConversionService> ConversionServices { get; } = new Dictionary<string, IConversionService>();

    protected BaseBurgrGenerator()
    {
        ConversionServices.Add("JS", new JSConversionService());
        ConversionServices.Add("HTML", new JSConversionService());
        ConversionServices.Add("CS", new CSConversionService());
        ConversionServices.Add("MySQL", new MySQLConversionService());
        ConversionServices.Add("PUML", new PUMLConversionService());
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        content = base.Generate(content, model, template, modelPrefix, modelSuffix);
        
        content = content.Replace("MODELNAME", model.Name);
        content = content.Replace("LABELNAME", ConversionServices[template.DestinationLanguage].ConvertToLabel(model.Name));

        return content;
    }

}
