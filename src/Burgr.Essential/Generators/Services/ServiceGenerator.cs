using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Services;

public class ServiceGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = "Service";
    public override string DescriptorType => Name;
    public ServiceGenerator()
    {
        TemplateParser = new ServiceTemplateParser();

        SubGenerators.Add(new VoidMethodGenerator());
        SubGenerators.Add(new IdentityMethodGenerator());
        SubGenerators.Add(new SimpleMethodGenerator());
        SubGenerators.Add(new ModelMethodGenerator());
        SubGenerators.Add(new ModelListMethodGenerator());
        SubGenerators.Add(new DependencyGenerator());
        SubGenerators.Add(new ConsumedEventGenerator());
    }

    protected override IModelParser InstantiateModelParser(string modelParserType)
    {
        return new Yaml.Model.Services.ServiceModelParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (template.Is("Anonymous") && !model.Is("Anonymous"))
        {
            return "model is not anonymous";
        }

        if (template.Is("NonAnonymous") && model.Is("Anonymous"))
        {
            return "model is anonymous";
        }

        if (model.Is("Internal") && !template.Is("InternalAccepted"))
        {
            return "template is not for internal";
        }

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        ModelDescriptor service = model;

        if (service.Is("Anonymous"))
        {
            foreach (string anonymousTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ServiceTemplateParser.TOREMOVEIFANONYMOUS)))
            {
                result = result.Replace(anonymousTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            foreach (string anonymousTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ServiceTemplateParser.TOREMOVEIFNOTANONYMOUS)))
            {
                result = result.Replace(anonymousTemplate, Utilities.SingleNewLine);
            }
        }

        result = result.Replace("SlugSERVICENAME", TextHelper.GenerateSlug(ConversionHelper.ConvertToPascalCase(service.Name)));
        result = result.Replace("SERVICENAME", ConversionHelper.ConvertToPascalCase(service.Name));

        result = result.Replace("_ANONYMOUS_", "");
        if (service.Is("UseStreaming"))
        {
            result = result.Replace("BindingFactory.StandardBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.WSBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.StandardNetNamedPipeBinding", "BindingFactory.StreamingNetNamedPipeBinding");
        }

        result = result.Replace("MANDATORYRIGHT", service.Get("MandatoryRight"));
        result = result.Replace("OWNERSHIPOVERRIDERIGHT", service.Get("OwnershipOverrideRight"));

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

    public ServiceTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Anonymous", Tag = "[A]" });
        Options.Add(new TemplateOption() { Name = "NonAnonymous", Tag = "[-A]" });
        Options.Add(new TemplateOption() { Name = "InternalAccepted", Tag = "[I]", });
    }
}
