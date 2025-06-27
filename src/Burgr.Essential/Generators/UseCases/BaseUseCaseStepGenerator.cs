using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Essential.Generators.ConversionServices;
using System.Text;

namespace SolidOps.Burgr.Essential.Generators.UseCases;

public abstract class BaseUseCaseStepGenerator : BaseBurgrGenerator
{
    protected BaseUseCaseStepGenerator()
    {
        SubGenerators.Add(new ParameterGenerator());
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        content = base.Generate(content, model, template, modelPrefix, modelSuffix);

        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        string actor = "User";
        if (useCase.Is("Anonymous"))
        {
            actor = "Anonymous";
        }

        if (GetMandatoryRight(step) != null)
        {
            actor = $"User with {GetMandatoryRight(step)}";
        }

        if (GetOwnershipOverrideRight(step) != null)
        {
            actor = $"User with {GetOwnershipOverrideRight(step)}";
        }

        content = content.Replace("_ACTOR_", actor);

        content = content.Replace("MANDATORYRIGHT", GetMandatoryRight(step));
        content = content.Replace("OWNERSHIPOVERRIDERIGHT", GetOwnershipOverrideRight(step));

        return content;
    }

    protected string GetMandatoryRight(ModelDescriptor model)
    {
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        if(step.Get("StepMandatoryRight") != null)
        {
            return step.Get("StepMandatoryRight");
        }
        return useCase.Get("MandatoryRight");
    }

    protected string GetOwnershipOverrideRight(ModelDescriptor model)
    {
        ModelDescriptor step = model;
        ModelDescriptor useCase = model.Parent;

        if (step.Get("StepOwnershipOverrideRight") != null)
        {
            return step.Get("StepOwnershipOverrideRight");
        }
        return useCase.Get("OwnershipOverrideRight");
    }

    protected string ReplaceParameters(ModelDescriptor service, IConversionService conversionService, ModelDescriptor step, string initialText, string modelPrefix, string modelSuffix, out bool hasPost)
    {
        StringBuilder parameterDefinition = new();
        StringBuilder parameters = new();
        StringBuilder convertedParameters = new();
        StringBuilder parameterUrl = new();
        StringBuilder parameterRef = new();
        StringBuilder parameterDefRef = new();
        StringBuilder parameterDefApi = new();
        StringBuilder parameterJson = new();
        parameterJson.Append("json " + ConversionHelper.ConvertToPascalCase(service.Name) + ConversionHelper.ConvertToPascalCase(step.Name) + "Input as \"Input\" { \n");
        string parameterImports = "";
        string parameterdata = "default";
        var language = conversionService.Language;
        if (language == "JS" || language == "HTML")
        {
            parameterdata = "undefined";
        }
        string parameterDataType = null;
        bool first = true;
        hasPost = false;
        int fromBodyCount = 0;
        string singleBodyParameterType = string.Empty;
        foreach (ModelDescriptor parameterInfo in step.GetChildren(DescriptorTypes.USECASE_STEP_PARAMETER_DESCRIPTOR))
        {
            if (parameterInfo.Get("SimpleType") != null || parameterInfo.Is("Enum"))
            {
                if (parameterDefinition.Length > 0)
                {
                    _ = parameterDefinition.Append(", ");
                    _ = parameterDefApi.Append(", ");
                    _ = parameters.Append(", ");
                    _ = convertedParameters.Append(", ");
                    _ = parameterDefRef.Append(", ");
                    _ = parameterRef.Append(", ");
                    _ = parameterJson.Append(",\n");
                }
                if (language == "JS" || language == "HTML")
                {
                    _ = parameterDefinition.AppendFormat("{1}: {0}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefApi.AppendFormat("{0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefRef.AppendFormat("ref {1}: {0}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterJson.AppendFormat("\"{0}\": \"{1}\"", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                }
                else
                {
                    _ = parameterDefinition.AppendFormat("{0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefApi.AppendFormat("{0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefRef.AppendFormat("ref {0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterJson.AppendFormat("\"{0}\": \"{1}\"", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                }
                _ = parameters.Append(ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                _ = convertedParameters.Append(ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                _ = parameterRef.Append("ref " + ConversionHelper.ConvertToCamelCase(parameterInfo.Name));


                if (!first)
                {
                    _ = language == "JS" || language == "HTML" ? parameterUrl.Append(" + '&") : parameterUrl.Append(" + \"&");
                }
                else
                {
                    _ = language == "JS" || language == "HTML" ? parameterUrl.Append(" + '?") : parameterUrl.Append(" + \"?");
                    first = false;
                }

                _ = language == "JS" || language == "HTML"
                    ? parameterUrl.AppendFormat("{0}=' + {0}", ConversionHelper.ConvertToCamelCase(parameterInfo.Name))
                    : parameterUrl.AppendFormat("{1}=\" + UriHelper.Convert<{0}>({1})", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
            }
            else
            {
                fromBodyCount++;
                if (fromBodyCount > 1)
                {
                    throw new Exception(string.Format("object parameter limited to only one: {0} {1}", service.Name, step.Name));
                }

                if (parameterDefinition.Length > 0)
                {
                    _ = parameterDefinition.Append(", ");
                    _ = parameterDefApi.Append(", ");
                    _ = parameters.Append(", ");
                    _ = convertedParameters.Append(", ");
                    _ = parameterDefRef.Append(", ");
                    _ = parameterRef.Append(", ");
                    _ = parameterJson.Append(",\n");
                }
                if (language == "JS" || language == "HTML")
                {
                    _ = parameterDefinition.AppendFormat("{1}: {0}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefApi.AppendFormat("{0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefRef.AppendFormat("ref {1}: {0}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterJson.AppendFormat("\"{0}\": \"{1}\"", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, false, false), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));

                    singleBodyParameterType = conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false);
                }
                else
                {
                    _ = parameterDefinition.AppendFormat("{0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = step.Get("ReturnType") == ReturnType.Void.ToString() || step.Get("ReturnType") == ReturnType.Identity.ToString() || step.Is("ForcePost")
                        ? parameterDefApi.AppendFormat("[FromBody] {0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true), ConversionHelper.ConvertToCamelCase(parameterInfo.Name))
                        : parameterDefApi.AppendFormat("[FromQuery] {0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterDefRef.AppendFormat("ref {0} {1}", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                    _ = parameterJson.AppendFormat("\"{0}\": \"{1}\"", conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true), ConversionHelper.ConvertToCamelCase(parameterInfo.Name));

                    singleBodyParameterType = conversionService.ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true);
                }
                _ = parameters.Append(ConversionHelper.ConvertToCamelCase(parameterInfo.Name));
                if (parameterInfo.Name.EndsWith("[]"))
                {
                    string mapperType = conversionService.ConvertParameterType(parameterInfo, ".Presentation.Mappers.", null, false, false, true) + "DTOMapper";
                    string propertyName = ConversionHelper.ConvertToCamelCase(parameterInfo.Name);
                    _ = convertedParameters.Append($"new {mapperType}(serviceProvider).Convert({propertyName}, serviceProvider)");
                }
                else
                {
                    string mapperType = conversionService.ConvertParameterType(parameterInfo, ".Presentation.Mappers.", null, false, false, true) + "DTOMapper";
                    string propertyName = ConversionHelper.ConvertToCamelCase(parameterInfo.Name);
                    _ = convertedParameters.Append($"new {mapperType}(serviceProvider).Convert({propertyName}, serviceProvider)");
                }
                _ = parameterRef.Append("ref " + ConversionHelper.ConvertToCamelCase(parameterInfo.Name));

                hasPost = true;
                parameterdata = language == "JS" || language == "HTML"
                    ? "input." + ConversionHelper.ConvertToCamelCase(parameterInfo.Name)
                    : ConversionHelper.ConvertToCamelCase(parameterInfo.Name);
                parameterDataType = ConversionServices[language].ConvertParameterType(parameterInfo, modelPrefix, modelSuffix, true, false, true);
            }
        }

        _ = parameterJson.Append("\n}\n");
        parameterJson.Append(ConversionHelper.ConvertToPascalCase(service.Name) + ConversionHelper.ConvertToPascalCase(step.Name) + " <.down. " + ConversionHelper.ConvertToPascalCase(service.Name) + ConversionHelper.ConvertToPascalCase(step.Name) + "Input\n");
        if (fromBodyCount > 0)
        {
            parameterJson = new StringBuilder();
            parameterJson.Append(ConversionHelper.ConvertToPascalCase(service.Name) + ConversionHelper.ConvertToPascalCase(step.Name) + " <.down. " + singleBodyParameterType + "\n");
        }

        initialText = initialText.Replace("PARAMETER_DEFINITION", parameterDefinition.ToString());
        initialText = initialText.Replace("PARAMETER_INTERNAL_DEFINITION", parameterDefinition.ToString());
        initialText = initialText.Replace("CONVERTED_PARAMETERS", convertedParameters.ToString());
        initialText = initialText.Replace("PARAMETERS", parameters.ToString());
        if (step.GetChildren(DescriptorTypes.USECASE_STEP_PARAMETER_DESCRIPTOR).Any())
        {
            initialText = initialText.Replace("PARAMETER_JSON", parameterJson.ToString());
        }
        else
        {
            initialText = initialText.Replace("PARAMETER_JSON", string.Empty);
        }

        if (parameterDataType != null)
        {
            initialText = initialText.Replace("COMMA_TYPE", ", ");
            initialText = initialText.Replace("PARAMETER_DATA_TYPE", parameterDataType);
        }
        else
        {
            initialText = initialText.Replace("COMMA_TYPE", "");
            initialText = initialText.Replace("PARAMETER_DATA_TYPE", "");
        }

        initialText = initialText.Replace("COMMA_DATA", ", ");
        initialText = parameters.Length > 0 ? initialText.Replace("COMMA", ", ") : initialText.Replace("COMMA", "");
        initialText = initialText.Replace("PARAMETER_URL", parameterUrl.ToString());
        initialText = initialText.Replace("PARAMETER_REF", parameterRef.ToString());
        initialText = initialText.Replace("PARAMETER_DEF_REF", parameterDefRef.ToString());
        initialText = initialText.Replace("PARAMETER_DATA", parameterdata);
        initialText = initialText.Replace("PARAMETER_DEF_API", parameterDefApi.ToString());
        initialText = initialText.Replace("PARAMETER_IMPORTS", parameterImports);
        return initialText;
    }

    public override string GetHeader(TemplateDescriptor descriptor)
    {
        return Utilities.SingleNewLine;
    }
}
