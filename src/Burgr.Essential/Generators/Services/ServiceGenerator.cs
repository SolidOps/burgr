using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class ServiceGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "Service";
    public override string DescriptorType => Name;
    public ServiceGenerator()
    {
        TemplateParser = new ServiceTemplateParser();

        SubGenerators.Add(new VoidServiceMethodGenerator());
        SubGenerators.Add(new IdentityServiceMethodGenerator());
        SubGenerators.Add(new SimpleServiceMethodGenerator());
        SubGenerators.Add(new ModelServiceMethodGenerator());
        SubGenerators.Add(new ModelListServiceMethodGenerator());
        SubGenerators.Add(new DependencyGenerator());
        SubGenerators.Add(new ConsumedEventGenerator());
    }

    protected override IModelParser InstantiateModelParser(string modelParserType)
    {
        return new Yaml.Model.Services.ServiceModelParser();
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor service = model;

        result = result.Replace("SlugSERVICENAME", TextHelper.GenerateSlug(ConversionHelper.ConvertToPascalCase(service.Name)));
        result = result.Replace("SERVICENAME", ConversionHelper.ConvertToPascalCase(service.Name));

        result = result.Replace("_ANONYMOUS_", "");
        if (service.Is("UseStreaming"))
        {
            result = result.Replace("BindingFactory.StandardBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.WSBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.StandardNetNamedPipeBinding", "BindingFactory.StreamingNetNamedPipeBinding");
        }

        string interfaces = string.Empty;
        bool first = true;
        foreach (string inter in service.GetList("ImplementsInterfaces"))
        {
            if (first)
            {
                interfaces = ": " + inter;
            }
            else
            {
                interfaces += ", " + inter;
            }

            first = false;
        }

        result = result.Replace("/*INTERFACE*/", interfaces);

        return result;
    }
}

public class ServiceTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach DOMAIN_SERVICE";

    public const string TOREMOVEIFANONYMOUS = "to remove if ANONYMOUS";
    public const string TOREMOVEIFNOTANONYMOUS = "to remove if NOT_ANONYMOUS";
    public List<string> AdditionalLoopIdentifiers => new List<string>()
    {
        TOREMOVEIFANONYMOUS,
        TOREMOVEIFNOTANONYMOUS
    };

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();
}
