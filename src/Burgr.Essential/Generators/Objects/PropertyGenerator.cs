using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.ConversionServices;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class PropertyGenerator : BaseNORADGenerator, IGenerator
{
    public static string Name = "Property";
    public override string DescriptorType => Name;

    public List<string> KnownSuffixes { get; set; } = new() { "Data", "Aggregate", "Entity", "DTO", "Model", "" };

    public PropertyGenerator()
    {
        TemplateParser = new PropertyTemplateParser();
    }

    public override void BeforeGenerate(List<ModelDescriptor> models, TemplateDescriptor template)
    {
        base.BeforeGenerate(models, template);

        List<string> domainTypes = new();
        if (template.Is("Aggregate"))
        {
            domainTypes.Add("Aggregate");
        }

        if (template.Is("Entity"))
        {
            domainTypes.Add("Entity");
        }

        if (template.Is("Transient"))
        {
            domainTypes.Add("Transient");
        }

        if (template.Is("ValueObject"))
        {
            domainTypes.Add("ValueObject");
        }

        if (domainTypes.Count > 0)
        {
            template.Set("DomainTypes", string.Join("|", domainTypes));
        }

        if ((template.Is("SingleNavigation") && template.Is("ListNavigation")) || template.Is("BothNavigation"))
        {
            template.Set("Navigation", NavigationType.Both.ToString());
        }
        else if (template.Is("SingleNavigation"))
        {
            template.Set("Navigation", NavigationType.Single.ToString());
        }
        else if (template.Is("ListNavigation"))
        {
            template.Set("Navigation", NavigationType.List.ToString());
        }
        else
        {
            template.Set("Navigation", NavigationType.None.ToString());
        }
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (model.Get("SpecialType") == SpecialType.Calculated.ToString() && !template.Is("Calculated"))
        {
            return "template not calculated";
        }

        bool templateIsNavigation = template.Get("Navigation") != null && template.Get("Navigation") != NavigationType.None.ToString();

        bool templateIsNormal = template.Is("Normal") || (!template.Is("Calculated") && !template.Is("NonPersisted") && !templateIsNavigation);

        if (model.Get("SpecialType") == SpecialType.Navigation.ToString())
        {
            if (!templateIsNavigation)
            {
                return "property is navigation";
            }

            if (model.Is("List") && template.Get("Navigation") != NavigationType.List.ToString() && template.Get("Navigation") != NavigationType.Both.ToString())
            {
                return "template is not list";
            }

            if (!model.Is("List") && template.Get("Navigation") != NavigationType.Single.ToString() && template.Get("Navigation") != NavigationType.Both.ToString())
            {
                return "property is not list";
            }
        }
        else
        {
            if (template.Get("Navigation") == NavigationType.List.ToString() || template.Get("Navigation") == NavigationType.Single.ToString())
            {
                return "template is only for navigation";
            }
        }

        if (model.Get("SpecialType") == SpecialType.NonPersisted.ToString() && !template.Is("NonPersisted"))
        {
            return "template is not not persisted";
        }

        if ((string.IsNullOrEmpty(model.Get("SpecialType")) || model.Get("SpecialType") == SpecialType.DBAlias.ToString()) && !templateIsNormal)
        {
            return "template is not normal";
        }

        if (model.Is("Private") && template.Is("PublicOnly"))
        {
            return "template is public only";
        }

        if (!model.Is("Private") && template.Is("PrivateOnly"))
        {
            return "template is private only";
        }

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
            return "property is not null";
        }

        if (template.Is("NonNullableOnly") && model.Is("Null"))
        {
            return "template is not nullable";
        }

        if (template.Is("Simple") && model.Get("PropertyType") != "Simple")
        {
            return "Property is not simple";
        }

        if (template.Is("Enum") && model.Get("PropertyType") != "Enum")
        {
            return "Property is not enum";
        }

        if (template.Is("Model") && template.Is("ReferencedModel"))
        {
            if (model.Get("PropertyType") != "Model" && model.Get("PropertyType") != "ReferencedModel")
            {
                return "Property is not model or referenced model";
            }
        }
        else if (template.Is("Model"))
        {
            if (model.Get("PropertyType") != "Model")
            {
                return "Property is not model";
            }
        }
        else if (template.Is("ReferencedModel"))
        {
            if (model.Get("PropertyType") != "ReferencedModel")
            {
                return "Property is not referenced model";
            }
        }

        var related = model.GetRelated("Object");
        if (model.Get("PropertyType") == "Model" && template.Is("Model"))
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

        if (model.Get("PropertyType") == "ReferencedModel" && template.Is("ReferencedModel"))
        {
            if (template.GetList("DomainTypes").Count > 0)
            {
                if (!template.GetList("DomainTypes").Contains(related.Get("DomainType")))
                {
                    return "DomainType incorrect";
                }
            }
        }

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        string language = template.DestinationLanguage;

        bool templateNullable = template.Is("NullableOnly") || !template.Is("NonNullableOnly");

        if (model.Get("PropertyType") == "Simple" && template.Is("Simple"))
        {
            result = result.Replace("_SIMPLE_", "");
            result = result.Replace("_TYPE_", model.GetPropertyType(conversionService, null, null, false));

            string type = model.Get("SimpleType");
            Type simpleType = conversionService.GetSimpleType(type);
            type = simpleType.Name;
            if (model.Is("Null") && type != "String")
            {
                type = "Nullable" + type;
            }

            if (type == "Byte[]")
            {
                type = "ArrayOfByte";
            }
            result = result.Replace("_CONVERTER_", "");
            result = result.Replace("_PROPERTYTYPE_", type);
            result = result.Replace("_PROPERTYINTERFACE_", type);
        }
        if (model.Get("PropertyType") == "Enum" && template.Is("Enum"))
        {
            result = result.Replace("_ENUM_", "");
            foreach (string suffix in KnownSuffixes)
            {
                result = result.Replace("_ENUMNULLABLETYPE_" + suffix, model.GetPropertyType(conversionService, null, suffix, true));
            }

            result = result.Replace("_ENUMTYPE_", ConversionHelper.ConvertToPascalCase(model.Get("EnumType")));
            result = result.Replace("_ISLIST_", model.Is("List") ? "List" : string.Empty);
            result = result.Replace("_ISQUERY_", model.Is("List") ? "Query" : "Get");
            result = result.Replace("DEPENDENCYNAMESPACE", model.FullModuleName);
        }
        if (model.Get("PropertyType") == "Model" && template.Is("Model"))
        {
            if (model.Is("Null") || model.Is("List") && templateNullable)
            {
                result = result.Replace("_NULL_", "");
            }

            foreach (string suffix in KnownSuffixes)
            {
                result = result.Replace("_PROPERTYFULLTYPE_" + suffix, model.GetPropertyType(conversionService, string.Empty, suffix, false));
                result = result.Replace("_PROPERTYFULLINTERFACE_" + suffix, model.GetPropertyType(conversionService, "I", suffix, true));
            }
            result = result.Replace("_PROPERTYTYPE_", ConversionHelper.ConvertToPascalCase(model.GetRelated("Object").Name));
            result = result.Replace("_PROPERTYINTERFACE_", "I" + ConversionHelper.ConvertToPascalCase(model.GetRelated("Object").Name));

            result = result.Replace("_COLUMNNAME_", model.Get("ColumnName"));
            result = result.Replace("_ISLIST_", model.Is("List") ? "List" : string.Empty);
            result = result.Replace("_ISQUERY_", model.Is("List") ? "Query" : "Get");
            result = result.Replace("DEPENDENCYNAMESPACE", model.FullModuleName);
        }
        if (model.Get("PropertyType") == "ReferencedModel" && template.Is("ReferencedModel"))
        {
            if (!model.Is("Null") && !model.Is("List") && !templateNullable)
            {
                foreach (string suffix in KnownSuffixes)
                {
                    result = result.Replace("_PROPERTYFULLTYPE_" + suffix, model.GetPropertyType(conversionService, string.Empty, suffix, false));
                    result = result.Replace("_PROPERTYFULLINTERFACE_" + suffix, model.GetPropertyType(conversionService, "I", suffix, true));
                }
                result = result.Replace("_PROPERTYTYPE_", model.GetPropertyType(conversionService, string.Empty, string.Empty, true));
                result = result.Replace("_PROPERTYINTERFACE_", "I" + model.GetPropertyType(conversionService, string.Empty, string.Empty, true));

                result = result.Replace(Tags.Namespace, conversionService.ConvertModuleName(model.GetRelated("Object").FullModuleName));
                result = result.Replace("_COLUMNNAME_", model.Get("ColumnName"));
                result = result.Replace("_ISLIST_", model.Is("List") ? "List" : string.Empty);
                result = result.Replace("_ISQUERY_", model.Is("List") ? "Query" : "Get");
                result = result.Replace("DEPENDENCYNAMESPACE", model.GetRelated("Object").FullModuleName);
            }
            if ((model.Is("Null") || model.Is("List")) && templateNullable)
            {
                foreach (string suffix in KnownSuffixes)
                {
                    result = result.Replace("_PROPERTYFULLTYPE_" + suffix, model.GetPropertyType(conversionService, string.Empty, suffix, false));
                    result = result.Replace("_PROPERTYFULLINTERFACE_" + suffix, model.GetPropertyType(conversionService, "I", suffix, true));
                }
                result = result.Replace("_PROPERTYTYPE_", model.GetPropertyType(conversionService, string.Empty, string.Empty, true));
                result = result.Replace("_PROPERTYINTERFACE_", "I" + model.GetPropertyType(conversionService, string.Empty, string.Empty, true));
                result = result.Replace(Tags.Namespace, conversionService.ConvertModuleName(model.GetRelated("Object").FullModuleName));
                result = result.Replace("_COLUMNNAME_", model.Get("ColumnName"));
                result = result.Replace("_ISLIST_", model.Is("List") ? "List" : string.Empty);
                result = result.Replace("_ISQUERY_", model.Is("List") ? "Query" : "Get");
                result = result.Replace("_NULL_", "");
                result = result.Replace("DEPENDENCYNAMESPACE", model.GetRelated("Object").FullModuleName);
            }

            result = result.Replace("_REF_", "");
            result = result.Replace("_FORLIST_", "");
        }

        if (result.Contains("_BACKLINK_"))
        {
            result = model.Get("SpecialType") == SpecialType.Navigation.ToString() && model.Get("BackLinkNavigation") != null
                ? result.Replace("_BACKLINK_", ConversionHelper.ConvertToPascalCase(model.Get("BackLinkNavigation")))
                : result.Replace("_BACKLINK_", "CLASSNAME");
        }

        if (result.Contains("_FROMLISTID_"))
        {
            result = model.Get("SpecialType") == SpecialType.Navigation.ToString() && model.Get("JoinNavigation") != null
                ? result.Replace("_FROMLISTID_", model.Get("JoinNavigation"))
                : result.Replace("_FROMLISTID_", "Id");
        }

        if (result.Contains("_FROMID_"))
        {
            result = model.Get("SpecialType") == SpecialType.Navigation.ToString() && model.Get("JoinNavigation") != null
                ? result.Replace("_FROMID_", model.Get("JoinNavigation"))
                : result.Replace("_FROMID_", model.Get("PropertyName"));
        }

        result = result.Replace("_CALCULATED_", "");
        result = result.Replace("_NONPERSISTED_", "");
        result = result.Replace("_NAVIGATION_", "");
        result = result.Replace("_REF_", "");
        result = result.Replace("_FORLIST_", "");
        result = result.Replace("_COLUMNNAME_", model.Get("ColumnName"));

        result = model.Get("PropertyType") == "Model" || model.Get("PropertyType") == "ReferencedModel"
            ? result.Replace("_SIMPLE__PROPERTYNAME_", conversionService.ConvertPropertyName(model.Get("IdPropertyName")))
            : result.Replace("_SIMPLE__PROPERTYNAME_", conversionService.ConvertPropertyName(model.Get("PropertyName")));

        result = result.Replace("_PROPERTYNAME_", conversionService.ConvertPropertyName(model.Get("PropertyName")));
        result = result.Replace("_FIELDNAME_", model.Get("FieldName"));
        result = result.Replace("_ISNULL_", model.Is("Null") ? "?" : string.Empty);

        bool patchNull = false;
        if (model.Get("PropertyType") == "Simple")
        {
            Type type = conversionService.GetSimpleType(model.Get("SimpleType"));
            if (!type.IsClass && !model.Is("Null"))
            {
                patchNull = true;
            }
        }

        result = result.Replace("_ISPATCHNULL_", patchNull ? "?" : string.Empty);
        result = result.Replace("_ISPATCHNULLVALUE_", patchNull ? ".Value" : string.Empty);

        result = result.Replace("_VALUEOBJECT_", "");

        result = HandlePropertyValidationRules(model, result);

        // password fields
        string inputType = "text";
        if (model.Name == "password")
        {
            inputType = "password";
        }
        result = result.Replace("INPUTTYPE", inputType);

        // entity type
        if (model.Get("PropertyType") == "Model")
        {
            var related = model.GetRelated("Object");
            result = DomainTypeHelper.ReplaceDomainType(result, related.Get("DomainType"));
        }

        if (model.Get("PropertyType") == "ReferencedModel")
        {
            var related = model.GetRelated("Object");
            result = DomainTypeHelper.ReplaceDomainType(result, related.Get("DomainType"));
        }

        result = result.Replace("[option]", conversionService.ConvertOption(model));

        return result;
    }

    private static string HandlePropertyValidationRules(ModelDescriptor model, string result)
    {
        if (result.Contains("// VALIDATION RULE"))
        {
            if (result.Contains("// VALIDATION RULE - PROPERTY_IS_UNIQUE"))
            {
                if (!model.Is("Unique"))
                {
                    result = string.Empty;
                }
            }
            if (result.Contains("// VALIDATION RULE - MULTIPLE_UNIQUE_CONSTRAINT"))
            {
                result = !model.Is("MultipleUniqueConstraint") ? string.Empty : HandleMultipleUniqueConstraint(model, result);
            }
            if (result.Contains("// VALIDATION RULE - PROPERTY_HAS_MAXSIZE"))
            {
                result = model.Get("SimpleType") == "string"
                    ? model.Get<int?>("FieldSize").HasValue && model.Get<int?>("FieldSize").Value > 0
                        ? result.Replace("FIELDSIZE", model.Get<int?>("FieldSize").ToString())
                        : !model.Is("MaxSize") ? result.Replace("FIELDSIZE", Constants.StandardMaxSize.ToString()) : string.Empty
                    : string.Empty;
            }
            if (result.Contains("// VALIDATION RULE - NULLABLE_PROPERTY"))
            {
                if (ShouldCheckNull(model))
                {
                    // do nothing
                }
                else
                {
                    result = string.Empty;
                }
            }
        }

        return result;
    }

    private static bool ShouldCheckNull(ModelDescriptor model)
    {
        if (model.Get("PropertyType") == "Simple")
        {
            return !model.Is("Null") && model.Get("SimpleType") == "string";
        }
        return !model.Is("Null");
    }

    private static string HandleMultipleUniqueConstraint(ModelDescriptor model, string result)
    {
        string constraintName = model.Get("PropertyName");
        string operation = "CriteriaOperation.Equal";
        if (model.Get("SimpleType") == "string")
        {
            operation = "CriteriaOperation.Like";
        }

        string constraintTuples = "(\"" + model.Get("PropertyName") + "\", " + operation + ", " + model.Get("FieldName") + ")";
        string constraintValues = "entity." + model.Get("QueryPropertyName");
        string constraintParams = "object " + model.Get("FieldName");
        foreach (string additionalPropertyName in model.GetList("UniqueConstraintAdditionalMembers"))
        {
            ModelDescriptor additionalPropertyDescriptor = model.Parent.GetChildren(PropertyGenerator.Name).Where(x => x.Name == additionalPropertyName).SingleOrDefault();
            if (additionalPropertyDescriptor != null)
            {
                //BaseProperty additionalProperty = BaseProperty.Instantiante(additionalPropertyDescriptor);
                operation = "CriteriaOperation.Equal";
                constraintName += "And" + additionalPropertyDescriptor.Get("PropertyName");
                if (additionalPropertyDescriptor.Get("SimpleType") == "string")
                {
                    operation = "CriteriaOperation.Like";
                }

                constraintTuples += ", (\"" + additionalPropertyDescriptor.Get("PropertyName") + "\", " + operation + ", " + additionalPropertyDescriptor.Get("FieldName") + ")";
                constraintValues += ", entity." + additionalPropertyDescriptor.Get("QueryPropertyName");
                constraintParams += ", object " + additionalPropertyDescriptor.Get("FieldName");
            }
        }

        result = result.Replace("_MULTIPLE_CONSTRAINT_", constraintName);
        result = result.Replace("_CONSTRAINT_TUPLES_", constraintTuples);
        result = result.Replace("_CONSTRAINT_VALUES_", constraintValues);
        result = result.Replace("_CONSTRAINT_PARAMS_", constraintParams);
        return result;
    }
}

public class PropertyTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach PROPERTY";
    public List<string> AdditionalLoopIdentifiers => new List<string>();

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public PropertyTemplateParser()
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