using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.Burgr.Essential.Yaml.Model;
using SolidOps.SubZero;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public class ObjectGenerator : BaseBurgrGenerator, IGenerator
{
    public static string Name = "Object";
    public override string DescriptorType => Name;

    public ObjectGenerator()
    {
        TemplateParser = new ObjectTemplateParser();
        SubGenerators.Add(new PropertyGenerator());
        SubGenerators.Add(new QueryablePropertyGenerator());
        SubGenerators.Add(new UniqueQueryablePropertyGenerator());
        SubGenerators.Add(new MultipleUniqueQueryablePropertyGenerator());
        SubGenerators.Add(new DependencyGenerator());
        SubGenerators.Add(new ConsumedEventGenerator());
        SubGenerators.Add(new DependentForGenerator());
        SubGenerators.Add(new RuleGenerator());
    }

    protected override IModelParser InstantiateModelParser(string modelParserType)
    {
        return new Yaml.Model.Objects.ObjectModelParser();
    }

    protected override string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
    {
        if (template.Is("Aggregate") || template.Is("Entity") || template.Is("ValueObject") || template.Is("Transient"))
        {
            if (!template.Is("Aggregate") && model.Get("DomainType") == "Aggregate")
            {
                return "Template not aggregate root";
            }

            if (!template.Is("Entity") && model.Get("DomainType") == "Entity")
            {
                return "Template not entity";
            }

            if (!template.Is("ValueObject") && model.Get("DomainType") == "ValueObject")
            {
                return "Template not ValueObject";
            }

            if (!template.Is("Transient") && model.Get("DomainType") == "Transient")
            {
                return "Template not Transient";
            }
        }

        if (template.Is("AtLeastOneResource") && model.GetChildren(DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR).Count() == 0)
            return "no resources";

        if (template.Is("OnlyEvent") && !model.Is("AlsoEvent") && !model.Is("OnlyEvent"))
            return "model not event";

        if (model.Is("OnlyEvent") && !template.Is("OnlyEvent"))
            return "template not event";

        if (template.Is("OnlyExposed") && !model.Is("Exposed"))
            return "model not exposed";

        if (template.Is("Cacheable") && !model.Is("Cacheable"))
            return "model not cacheable";

        if (template.Is("NotCacheable") && model.Is("Cacheable"))
            return "model cacheable";

        if (template.Is("Component") && !model.Is("Component"))
            return "model not component";

        return null;
    }

    public override string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
    {
        var conversionService = ConversionServices[template.DestinationLanguage];
        string result = base.Generate(content, model, template, modelPrefix, modelSuffix);
        if (result == string.Empty)
            return result;
        if (model.Get("EventDataType") != null)
        {
            result = result.Replace(Tags.DependencyEventDataType, ConversionHelper.ConvertToPascalCase(model.Get("EventDataType")));
        }

        if (model.Is("PrivateId"))
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFPRIVATEID)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }

        if (model.Is("ComposedId"))
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFCOMPOSEDID)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFNOTCOMPOSEDID)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }

        if (model.Get("Label") != null)
        {
            result = result.Replace("_LABEL_", ConversionHelper.ConvertToPascalCase(model.Get("Label")));
        }
        else
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFNOLABEL)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }

        // Resources
        List<ModelDescriptor> resourceDefinitions = model.GetChildren(DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR);
        result = HandleResource(result, resourceDefinitions.Where(r => r.Name.ToLower() == "query").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTGETBYQUERY);
        result = HandleResource(result, resourceDefinitions.Where(r => r.Name.ToLower() == "get").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTGETBYID);
        result = HandleResource(result, resourceDefinitions.Where(r => r.Name.ToLower() == "add").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTADD);
        result = HandleResource(result, resourceDefinitions.Where(r => r.Name.ToLower() == "update").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTUPDATE);
        result = HandleResource(result, resourceDefinitions.Where(r => r.Name.ToLower() == "remove").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTREMOVE);

        // Components
        List<ModelDescriptor> componentDefinitions = model.GetChildren(DescriptorTypes.COMPONENT_DEFINITION_DESCRIPTOR);
        result = HandleComponent(result, componentDefinitions.Where(r => r.Name.ToLower() == "list").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTLISTCOMPONENT);
        result = HandleComponent(result, componentDefinitions.Where(r => r.Name.ToLower() == "details").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTDETAILSCOMPONENT);

        // Views
        List<ModelDescriptor> viewDefinitions = model.GetChildren(DescriptorTypes.VIEW_DEFINITION_DESCRIPTOR);
        result = HandleView(result, viewDefinitions.Where(r => r.Name.ToLower() == "list").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTLISTVIEW, resourceDefinitions.Where(r => r.Name.ToLower() == "query").SingleOrDefault()?.Is("Anonymous"));
        result = HandleView(result, viewDefinitions.Where(r => r.Name.ToLower() == "details").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOTDETAILSVIEW, false);

        List<ModelDescriptor> eventDefinitions = model.GetChildren(DescriptorTypes.PRODUCED_EVENT_DESCRIPTOR);
        result = HandleEvent(result, eventDefinitions.Where(r => r.Name.ToLower() == "add").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOADDEVENT);
        result = HandleEvent(result, eventDefinitions.Where(r => r.Name.ToLower() == "update").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOUPDATEEVENT);
        result = HandleEvent(result, eventDefinitions.Where(r => r.Name.ToLower() == "remove").SingleOrDefault(), ObjectTemplateParser.TOREMOVEIFNOREMOVEEVENT);

        result = result.Replace("SlugCLASSNAME", TextHelper.GenerateSlug(ConversionHelper.ConvertToPascalCase(model.Name)));
        result = result.Replace("CLASSNAME", ConversionHelper.ConvertToPascalCase(model.Name));
        result = result.Replace("FULLTABLE", conversionService.ConvertToFullTableName(model.FullModuleName, model.Name, GeneratorOptions.ForcedPrefix, model.Get("TableName")));
        result = result.Replace("FULLMYSQLTABLE", conversionService.ConvertToFullMySQLTableName(model.ModuleName, model.Name, GeneratorOptions.ForcedPrefix, model.Get("TableName")));

        result = result.Replace("IDCOLUMNNAME", model.Get("IdColumnName"));
        result = result.Replace("IDPROPERTYNAME", ConversionHelper.ConvertToPascalCase(model.Get("IdColumnName")));
        result = result.Replace("TEMPLATEMODULE", model.Get("ModuleName"));
        result = result.Replace("PREFTABLENAME", model.Get("TableName"));
        result = result.Replace("FORCEDPREFIX", model.Get("ForcedPrefix") ?? "");
        result = result.Replace("_TR_", string.Empty);
        result = result.Replace("_VO_", string.Empty);
        result = result.Replace("_LIST_", string.Empty);

        result = result.Replace("_DOMAINTYPE_", conversionService.ConvertDomainType(model));

        if (model.Is("ComposedId"))
        {
            result = result.Replace("PRIMARYKEYCOLUMNNAME", "`" + string.Join("`,`", model.Get("IdColumnName").Split('|')) + "`");
            result = result.Replace("NEW_KEYCOLUMNVALUE", "CONCAT(NEW." + string.Join(", \"|\", NEW.", model.Get("IdColumnName").Split('|')) + ")");
            result = result.Replace("OLD_KEYCOLUMNVALUE", "CONCAT(OLD." + string.Join(", \"|\", OLD.", model.Get("IdColumnName").Split('|')) + ")");
        }
        else
        {
            result = result.Replace("PRIMARYKEYCOLUMNNAME", "`" + model.Get("IdColumnName") + "`");
            result = result.Replace("NEW_KEYCOLUMNVALUE", "NEW." + model.Get("IdColumnName"));
            result = result.Replace("OLD_KEYCOLUMNVALUE", "OLD." + model.Get("IdColumnName"));
        }

        string interfaces = string.Empty;
        foreach (string inter in model.GetList("ImplementsInterface"))
        {
            interfaces += ", " + inter;
        }

        foreach (string removeTemplate in Utilities.GetInnerTemplates(template.Content, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEATGENERATION)))
        {
            result = result.Replace(removeTemplate, Utilities.SingleNewLine);
        }

        result = result.Replace("/*, INTERFACE */", interfaces);

        var type = model.Get("IdentityKeysType") ?? GeneratorOptions.IdentityKeysType;
        result = conversionService.ReplaceIdentityType(result, type);

        result = DomainTypeHelper.ReplaceDomainType(result, model.Get("DomainType"));

        return result;
    }

    private string HandleResource(string result, ModelDescriptor definition, string identifier)
    {
        if (definition == null)
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(identifier)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            if (definition.Is("Anonymous"))
            {
                foreach (string rightCheckTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(identifier)))
                {
                    string changedTemplate = rightCheckTemplate;
                    foreach (string anonymousTemplate in Utilities.GetInnerTemplates(rightCheckTemplate, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFANONYMOUS)))
                    {
                        changedTemplate = changedTemplate.Replace(anonymousTemplate, Utilities.SingleNewLine);
                    }
                    changedTemplate = changedTemplate.Replace("_ACTOR_", "Anonymous");
                    result = result.Replace(rightCheckTemplate, changedTemplate);
                }
            }
            else
            {
                foreach (string mandatoryTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(identifier)))
                {
                    string changedTemplate = mandatoryTemplate;
                    foreach (string anonymousTemplate in Utilities.GetInnerTemplates(changedTemplate, Utilities.GetLoopIdentifiers(ObjectTemplateParser.TOREMOVEIFNOTANONYMOUS)))
                    {
                        changedTemplate = changedTemplate.Replace(anonymousTemplate, Utilities.SingleNewLine);
                    }
                    changedTemplate = changedTemplate.Replace("MANDATORYRIGHT", definition.Get("MandatoryRight"));
                    changedTemplate = changedTemplate.Replace("OWNERSHIPOVERRIDERIGHT", definition.Get("OwnershipOverrideRight"));

                    string actor = "User";
                    if (definition.Get("MandatoryRight") != null)
                    {
                        var right = definition.Get("MandatoryRight");
                        if (right == "*")
                        {
                            actor = "Admin";
                        }
                        else
                        {
                            actor = $"User with {right}";
                        }
                    }

                    if (definition.Get("OwnershipOverrideRight") != null)
                    {
                        var right = definition.Get("OwnershipOverrideRight");
                        if (right == "*")
                        {
                            actor = "Admin";
                        }
                        else
                        {
                            actor = $"User with {right}";
                        }
                    }

                    changedTemplate = changedTemplate.Replace("_ACTOR_", actor);

                    result = result.Replace(mandatoryTemplate, changedTemplate);
                }
            }
        }
        return result;
    }

    private string HandleComponent(string result, ModelDescriptor definition, string identifier)
    {
        if (definition == null)
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(identifier)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            var includes = definition.Get("Includes") ?? string.Empty;
            result = result.Replace("_INCLUDES_", includes);
        }
        return result;
    }

    private string HandleView(string result, ModelDescriptor definition, string identifier, bool? isAnonymous)
    {
        if (definition == null)
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(identifier)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {
            result = result.Replace("_HIDDEN_FUNC_RESULT_", isAnonymous ?? false ? "!this.isUserLogged" : "false");
        }
        return result;
    }

    private string HandleEvent(string result, ModelDescriptor definition, string toRemoveIfNotEvent)
    {
        if (definition == null)
        {
            foreach (string removeTemplate in Utilities.GetInnerTemplates(result, Utilities.GetLoopIdentifiers(toRemoveIfNotEvent)))
            {
                result = result.Replace(removeTemplate, Utilities.SingleNewLine);
            }
        }
        else
        {

        }

        return result;
    }
}

public class ObjectTemplateParser : ITemplateParser
{
    public string LoopIdentifier => "foreach MODEL";

    public const string TOREMOVEIFPRIVATEID = "to remove if PRIVATE_ID";
    public const string TOREMOVEIFANONYMOUS = "to remove if ANONYMOUS";
    public const string TOREMOVEIFNOTANONYMOUS = "to remove if NOT_ANONYMOUS";
    public const string TOREMOVEIFNOTGETBYQUERY = "to remove if NOT_GETBYQUERY";
    public const string TOREMOVEIFNOTGETBYID = "to remove if NOT_GETBYID";
    public const string TOREMOVEIFNOTADD = "to remove if NOT_ADD";
    public const string TOREMOVEIFNOTUPDATE = "to remove if NOT_UPDATE";
    public const string TOREMOVEIFNOTREMOVE = "to remove if NOT_REMOVE";

    public const string TOREMOVEIFNOTLISTCOMPONENT = "to remove if NOT_LISTCOMPONENT";
    public const string TOREMOVEIFNOTDETAILSCOMPONENT = "to remove if NOT_DETAILSCOMPONENT";

    public const string TOREMOVEIFNOTLISTVIEW = "to remove if NOT_LISTVIEW";
    public const string TOREMOVEIFNOTDETAILSVIEW = "to remove if NOT_DETAILSVIEW";

    public const string TOREMOVEATGENERATION = "to remove at generation";
    public const string TOREMOVEIFNOTCOMPOSEDID = "to remove if NOT_COMPOSED_ID";
    public const string TOREMOVEIFCOMPOSEDID = "to remove if COMPOSED_ID";

    public const string TOREMOVEIFNOADDEVENT = "to remove if NO_ADD_EVENT";
    public const string TOREMOVEIFNOUPDATEEVENT = "to remove if NO_UPDATE_EVENT";
    public const string TOREMOVEIFNOREMOVEEVENT = "to remove if NO_REMOVE_EVENT";

    public const string TOREMOVEIFNOLABEL = "to remove if NO_LABEL";

    public List<string> AdditionalLoopIdentifiers => new List<string>() {
        TOREMOVEIFPRIVATEID,
        TOREMOVEIFANONYMOUS,
        TOREMOVEIFNOTANONYMOUS,
        TOREMOVEIFNOTGETBYQUERY,
        TOREMOVEIFNOTGETBYID,
        TOREMOVEIFNOTADD,
        TOREMOVEIFNOTUPDATE,
        TOREMOVEIFNOTREMOVE,
        TOREMOVEATGENERATION,
        TOREMOVEIFNOTCOMPOSEDID,
        TOREMOVEIFCOMPOSEDID,
        TOREMOVEIFNOADDEVENT,
        TOREMOVEIFNOUPDATEEVENT,
        TOREMOVEIFNOREMOVEEVENT,
        TOREMOVEIFNOTLISTCOMPONENT,
        TOREMOVEIFNOTDETAILSCOMPONENT,
        TOREMOVEIFNOTLISTVIEW,
        TOREMOVEIFNOTDETAILSVIEW,
        TOREMOVEIFNOLABEL
    };

    public List<TemplateOption> Options { get; } = new List<TemplateOption>();

    public ObjectTemplateParser()
    {
        Options.Add(new TemplateOption() { Name = "Aggregate", Tag = "[AG]" });
        Options.Add(new TemplateOption() { Name = "Entity", Tag = "[EN]" });
        Options.Add(new TemplateOption() { Name = "Transient", Tag = "[TR]" });
        Options.Add(new TemplateOption() { Name = "ValueObject", Tag = "[VO]", });
        Options.Add(new TemplateOption() { Name = "AtLeastOneResource", Tag = "[R]", });
        Options.Add(new TemplateOption() { Name = "OnlyEvent", Tag = "[EVT]", });
        Options.Add(new TemplateOption() { Name = "Cacheable", Tag = "[CACHE]", });
        Options.Add(new TemplateOption() { Name = "OnlyExposed", Tag = "[EXP]", });
        // for dto used in component
        Options.Add(new TemplateOption() { Name = "Component", Tag = "[C]" });
    }
}

