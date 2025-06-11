using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Objects;
using System.Reflection;
using System.Reflection.Metadata;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public class ParameterGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = DescriptorTypes.USECASE_STEP_PARAMETER_DESCRIPTOR;
    public override string DescriptorType => Name;

    public ParameterGenerator()
    {
        TemplateParser = new ParameterTemplateParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (model.Is("List") && template.Is("NonArray"))
        {
            return "template is only non array";
        }

        if (!model.Is("List") && template.Is("Array"))
        {
            return "template is only array";
        }

        if (template.Is("NullableOnly") && !model.Is("Null"))
        {
            return "parameter is not null";
        }

        if (template.Is("NonNullableOnly") && model.Is("Null"))
        {
            return "template is not nullable";
        }

        if (template.Is("Simple") && model.Get("SimpleType") == null)
        {
            return "Property is not simple";
        }

        if (template.Is("Enum") && !model.Is("Enum"))
        {
            return "Property is not enum";
        }

        if (template.Is("Model") && !model.Is("Model"))
        {
            return "Property is not model";
        }

        var related = model.GetRelated("Object");
        if (model.Is("Model") && template.Is("Model"))
        {
            List<string> domainTypes = template.GetList("DomainTypes");
            if (domainTypes.Count > 0)
            {
                if (!domainTypes.Contains(related.Get("DomainType")))
                {
                    return "DomainType incorrect";
                }
            }
        }

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        var conversionService = ConversionServices[template.DestinationLanguage];
        var paramType = conversionService.ConvertParameterType(model, modelPrefix, modelSuffix);

        result = result.Replace("_PARAMETER_", ConversionHelper.ConvertToCamelCase(model.Name));
        if (model.Get("SimpleType") != null)
        {
            result = result.Replace("_PARAMTYPE_", paramType);
            if (paramType.StartsWith("System."))
            {
                paramType = paramType.Replace("System.", string.Empty);
            }
            result = result.Replace("_SHORTPARAMTYPE_", paramType);
        }
        else if(model.Is("Model"))
        {
            var related = model.GetRelated("Object");
            result = result.Replace("_SHORTPARAMTYPE_", ConversionHelper.ConvertToPascalCase(related.Name));
        }
        return result;
    }
}

public class ParameterTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach SERVICE_METHOD_PARAMETER";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public ParameterTemplateParser()
    {
        // property types
        Options.Add(new TemplateOption() { Name = "Simple", Tag = "[S]", });
        Options.Add(new TemplateOption() { Name = "Enum", Tag = "[E]", });
        Options.Add(new TemplateOption() { Name = "Model", Tag = "[M]", });
        Options.Add(new TemplateOption() { Name = "ReferencedModel", Tag = "[R]", });

        // model types
        Options.Add(new TemplateOption() { Name = "Aggregate", Tag = "[AG]", });
        Options.Add(new TemplateOption() { Name = "Entity", Tag = "[EN]", });
        Options.Add(new TemplateOption() { Name = "Transient", Tag = "[TR]", });
        Options.Add(new TemplateOption() { Name = "ValueObject", Tag = "[VO]", });

        // usage type
        Options.Add(new TemplateOption() { Name = "Normal", Tag = "[NO]" });
        Options.Add(new TemplateOption() { Name = "NonPersisted", Tag = "[NP]" });
        Options.Add(new TemplateOption() { Name = "Calculated", Tag = "[CA]", });
        Options.Add(new TemplateOption() { Name = "BothNavigation", Tag = "[NA]", });
        Options.Add(new TemplateOption() { Name = "SingleNavigation", Tag = "[SNA]", });
        Options.Add(new TemplateOption() { Name = "ListNavigation", Tag = "[LNA]", });

        // attributes
        // - nullable
        Options.Add(new TemplateOption() { Name = "NullableOnly", Tag = "[N]", });
        Options.Add(new TemplateOption() { Name = "NonNullableOnly", Tag = "[NN]", });
        // - public
        Options.Add(new TemplateOption() { Name = "PublicOnly", Tag = "[PUO]", });
        Options.Add(new TemplateOption() { Name = "PrivateOnly", Tag = "[PRO]", });
        // - array
        Options.Add(new TemplateOption() { Name = "Array", Tag = "[AR]", });
        Options.Add(new TemplateOption() { Name = "NonArray", Tag = "[NAR]", });
    }
}
