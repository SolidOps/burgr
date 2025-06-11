using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class UseCaseGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "UseCase";
    public override string DescriptorType => Name;
    public UseCaseGenerator()
    {
        TemplateParser = new UseCaseTemplateParser();

        SubGenerators.Add(new VoidUseCaseStepGenerator());
        SubGenerators.Add(new IdentityUseCaseStepGenerator());
        SubGenerators.Add(new SimpleUseCaseStepGenerator());
        SubGenerators.Add(new ModelUseCaseStepGenerator());
        SubGenerators.Add(new ModelListUseCaseStepGenerator());
        SubGenerators.Add(new DependencyGenerator());
        SubGenerators.Add(new ConsumedEventGenerator());
    }

    protected override IModelParser InstantiateModelParser(string modelParserType)
    {
        return new Yaml.Model.Services.UseCaseModelParser();
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
        ModelDescriptor useCase = model;

        if (useCase.Is("Anonymous"))
        {
            foreach (string anonymousTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(UseCaseTemplateParser.TOREMOVEIFANONYMOUS)))
            {
                result = result.Replace(anonymousTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            foreach (string anonymousTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(UseCaseTemplateParser.TOREMOVEIFNOTANONYMOUS)))
            {
                result = result.Replace(anonymousTemplate, Utilities.SingleNewLine);
            }
        }

        result = result.Replace("SlugUSECASENAME", TextHelper.GenerateSlug(ConversionHelper.ConvertToPascalCase(useCase.Name)));
        result = result.Replace("USECASENAME", ConversionHelper.ConvertToPascalCase(useCase.Name));

        result = result.Replace("_ANONYMOUS_", "");
        if (useCase.Is("UseStreaming"))
        {
            result = result.Replace("BindingFactory.StandardBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.WSBasicHttpBinding", "BindingFactory.StreamingBasicHttpBinding");
            result = result.Replace("BindingFactory.StandardNetNamedPipeBinding", "BindingFactory.StreamingNetNamedPipeBinding");
        }

        string interfaces = string.Empty;
        bool first = true;
        foreach (string inter in useCase.GetList("ImplementsInterfaces"))
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

public class UseCaseTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach DOMAIN_USECASE";

    public const string TOREMOVEIFANONYMOUS = "to remove if ANONYMOUS";
    public const string TOREMOVEIFNOTANONYMOUS = "to remove if NOT_ANONYMOUS";
    public List<string> AdditionalLoopIdentifiers => new List<string>()
    {
        TOREMOVEIFANONYMOUS,
        TOREMOVEIFNOTANONYMOUS
    };

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public UseCaseTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Anonymous", Tag = "[A]" });
        Options.Add(new TemplateOption() { Name = "NonAnonymous", Tag = "[-A]" });
        Options.Add(new TemplateOption() { Name = "InternalAccepted", Tag = "[I]", });
    }
}
