using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.Burgr.Essential.Generators.Enums;
using SolidOps.Burgr.Essential.Generators.Objects;
using SolidOps.Burgr.Essential.Generators.Rights;
using SolidOps.Burgr.Essential.Yaml.Model.Enums;
using System.Globalization;

namespace SolidOps.Burgr.Essential.Yaml.Model.Objects;

public class ObjectModelParser : BaseYamlModelParser, IModelParser
{
    private EnumModelParser enumParser;
    private ModelParserEngine modelParserEngine;

    public ObjectModelParser()
    {
        DefaultDescriptorType = ObjectGenerator.Name;
    }

    public virtual void ParseModel(IModelParserEngine parserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators)
    {
        enumParser = generators.Single(g => g.DescriptorType == "Enum").GetModelParser(parserEngine.ModelParserType) as EnumModelParser;
        this.modelsRepository = modelsRepository;
        modelParserEngine = parserEngine as ModelParserEngine;

        foreach (var kvpModule in modelParserEngine.YamlModelContentByModule)
        {
            string moduleName = modelParserEngine.ModuleName;
            if (kvpModule.Key != string.Empty)
                moduleName += "." + kvpModule.Key;
            foreach (var kvp in kvpModule.Value.aggregate_roots)
            {
                _ = GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Aggregate, parserEngine.NamespaceName, moduleName, null);
            }

            foreach (var kvp in kvpModule.Value.entities)
            {
                _ = GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Entity, parserEngine.NamespaceName, moduleName, null);
            }

            foreach (var kvp in kvpModule.Value.transients)
            {
                _ = GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Transient, parserEngine.NamespaceName, moduleName, null);
            }

            foreach (var kvp in kvpModule.Value.value_objects)
            {
                _ = GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.ValueObject, parserEngine.NamespaceName, moduleName, null);
            }

            foreach (var kvp in kvpModule.Value.events)
            {
                _ = GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Event, parserEngine.NamespaceName, moduleName, new Dictionary<string, string>
            {
                { "OnlyEvent", "true" },
                { "EventDataType", kvp.Value.data_type }
            });

            }
        }
    }

    #region Object
    public ModelDescriptor GetOrCreateObjectDescriptor(string name, base_object value, DomainType domainType, string namespaceName, string moduleName, Dictionary<string, string> addedAttributes)
    {
        ModelDescriptor descriptor = FindModelDescriptor(namespaceName, moduleName, ObjectGenerator.Name, name);
        if (descriptor == null)
        {
            if (name.ToLower() != name)
            {
                throw new Exception($"model object name must be in lower case : {name}");
            }

            descriptor = this.CreateDescriptor(name, ObjectGenerator.Name, namespaceName, moduleName);
        }

        if (descriptor.Is("WIP"))
        {
            if (addedAttributes != null)
            {
                foreach (var kvp in addedAttributes)
                {
                    descriptor.Set(kvp.Key, kvp.Value);
                }
            }

            descriptor.Set("IdColumnName", "id");
            descriptor.Set("TableName", null);

            if (value.id_column_name != null)
            {
                descriptor.Set("IdColumnName", value.id_column_name);
            }
            if (value.table_name != null)
            {
                descriptor.Set("TableName", value.table_name);
            }
            if (!string.IsNullOrEmpty(value.forced_prefix))
            {
                descriptor.Set("ForcedPrefix", value.forced_prefix);
            }
            if (value.extends != null)
            {
                descriptor.Set("ExtensionOfType", value.extends);
            }
            descriptor.Set("DomainType", domainType.ToString());

            if (value.event_produced != null)
            {
                descriptor.Set("AlsoEvent", "true");
                descriptor.Set("EventDataType", descriptor.Name);

                foreach (var part in value.event_produced.Split("|"))
                {

                    descriptor.AddChild(new ModelDescriptor(part, DescriptorTypes.PRODUCED_EVENT_DESCRIPTOR)
                    {
                        NamespaceName = namespaceName,
                        ModuleName = moduleName
                    });
                }
            }

            if (value.is_id_private || domainType == DomainType.Transient)
            {
                descriptor.Set("PrivateId", "true");
            }

            if (value.identity_keys_type != null)
            {
                descriptor.Set("IdentityKeysType", value.identity_keys_type);
            }

            if (value.cacheable)
            {
                descriptor.Set("Cacheable", "true");
            }

            if (value.@interface != null)
            {
                descriptor.Set("Interfaces", value.@interface);
            }

            FullModelDescription modelDescription = GetModelDescription(namespaceName, moduleName);

            if (value.properties != null)
            {
                foreach (var kvp in value.properties)
                {
                    if (kvp.Key.ToLower() != kvp.Key)
                    {
                        throw new Exception($"property name must be in lower case : {kvp.Key} of {name}");
                    }

                    ModelDescriptor propertyDescriptor;

                    if (kvp.Value is Dictionary<object, object> dic)
                    {
                        var text = new YamlDotNet.Serialization.Serializer().Serialize(dic);
                        var property = new YamlDotNet.Serialization.Deserializer().Deserialize<property>(text);
                        propertyDescriptor = GetOrCreateProperty(descriptor, kvp.Key, property, name, value);
                    }
                    else
                    {
                        propertyDescriptor = GetOrCreateProperty(descriptor, kvp.Key, new property() { type = kvp.Value as string ?? "string" }, name, value);
                    }

                    if (propertyDescriptor.Is("Label"))
                    {
                        var label = descriptor.Get("Label");
                        if (label != null)
                        {
                            throw new Exception($"model has already a lable : {label} of {descriptor.Name}");
                        }
                        descriptor.Set("Label", propertyDescriptor.Name);
                    }
                }
            }

            if (value.api != null)
            {
                foreach (var kvp in value.api)
                {
                    if (descriptor.Get("DomainType") != DomainType.Aggregate.ToString())
                    {
                        throw new Exception($"API can only be set on Aggregate Roots, currently set on {name}");
                    }

                    var resourceAttribute = kvp.Value;

                    ModelDescriptor resourceDescriptor = FindModelDescriptor(descriptor.GetChildren(), DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR, kvp.Key.ToString());
                    if (resourceDescriptor == null)
                    {
                        resourceDescriptor = new ModelDescriptor(kvp.Key.ToString(), DescriptorTypes.RESOURCE_DEFINITION_DESCRIPTOR)
                        {
                            NamespaceName = namespaceName,
                            ModuleName = moduleName
                        };
                        if (resourceAttribute != null)
                        {
                            resourceDescriptor.Set("Anonymous", resourceAttribute.is_anonymous.ToString());
                            resourceDescriptor.Set("MandatoryRight", resourceAttribute.mandatory_right);
                            resourceDescriptor.Set("OwnershipOverrideRight", resourceAttribute.ownership_override_right);
                            if (!string.IsNullOrEmpty(resourceAttribute.mandatory_right))
                            {
                                _ = GetOrCreateRightDescriptor(namespaceName, moduleName, modelDescription, resourceAttribute.mandatory_right);
                            }
                            if (!string.IsNullOrEmpty(resourceAttribute.ownership_override_right))
                            {
                                _ = GetOrCreateRightDescriptor(namespaceName, moduleName, modelDescription, resourceAttribute.ownership_override_right);
                            }
                        }
                        descriptor.AddChild(resourceDescriptor);
                    }
                }
            }

            if (value.event_consumers != null)
            {
                foreach (var event_full_type in value.event_consumers)
                {
                    descriptor.AddChild(new ModelDescriptor(event_full_type, ConsumedEventGenerator.Name)
                    {
                        NamespaceName = namespaceName,
                        ModuleName = moduleName
                    });
                }
            }

            // manage dependencies
            if (value.dependencies != null)
            {
                foreach (var kvpDependecy in value.dependencies)
                {
                    ModelDescriptor child = new(kvpDependecy.Key, DependencyGenerator.Name)
                    {
                        NamespaceName = namespaceName,
                        ModuleName = moduleName
                    };
                    AddRelatedDescriptor(child, new TypeInfo(kvpDependecy.Key, moduleName), "Object");
                    descriptor.AddChild(child);
                }
            }

            if (descriptor.Get("DomainType") != DomainType.Aggregate.ToString())
            {
                List<ModelDescriptor> properties = descriptor.GetChildren(PropertyGenerator.Name);
                foreach (ModelDescriptor propertyDescriptor in properties)
                {
                    switch (propertyDescriptor.Get("PropertyType"))
                    {
                        case "Model":
                        case "ReferencedModel":
                            ModelDescriptor dependencyObject = propertyDescriptor.GetRelated("Object");
                            if (dependencyObject != null && dependencyObject.Get("DomainType") == DomainType.Aggregate.ToString())
                            {
                                ModelDescriptor dependentForDescriptor = new(propertyDescriptor.Name, DependentForGenerator.Name)
                                {
                                    NamespaceName = dependencyObject.NamespaceName,
                                    ModuleName = dependencyObject.ModuleName
                                };
                                dependentForDescriptor.AddRelated("Object", descriptor);
                                dependencyObject.AddChild(dependentForDescriptor);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (value.components != null)
            {
                if (descriptor.Get("DomainType") != DomainType.Aggregate.ToString())
                {
                    throw new Exception($"Components can only be set on Aggregate Roots, currently set on {name}");
                }

                foreach (var kvp in value.components)
                {
                    var componentType = kvp.Value;

                    ModelDescriptor componentDescriptor = FindModelDescriptor(descriptor.GetChildren(), DescriptorTypes.COMPONENT_DEFINITION_DESCRIPTOR, kvp.Key.ToString());
                    if (componentDescriptor == null)
                    {
                        componentDescriptor = new ModelDescriptor(kvp.Key.ToString(), DescriptorTypes.COMPONENT_DEFINITION_DESCRIPTOR)
                        {
                            NamespaceName = namespaceName,
                            ModuleName = moduleName
                        };
                        if (componentType != null)
                        {
                            //componentDescriptor.Set("Anonymous", componentType.is_anonymous.ToString());
                            //componentDescriptor.Set("MandatoryRight", componentType.mandatory_right);
                            //componentDescriptor.Set("OwnershipOverrideRight", componentType.ownership_override_right);
                        }
                        descriptor.AddChild(componentDescriptor);
                    }
                }

                descriptor.Set("Component", "true");
                List<ModelDescriptor> properties = descriptor.GetChildren(PropertyGenerator.Name);
                foreach (ModelDescriptor propertyDescriptor in properties)
                {
                    var related = propertyDescriptor.GetRelated("Object");
                    if (related != null)
                        related.Set("Component", "true");
                }
            }
            if (value.views != null)
            {
                if (descriptor.Get("DomainType") != DomainType.Aggregate.ToString())
                {
                    throw new Exception($"Views can only be set on Aggregate Roots, currently set on {name}");
                }

                foreach (var kvp in value.views)
                {
                    var viewType = kvp.Value;

                    ModelDescriptor viewDescriptor = FindModelDescriptor(descriptor.GetChildren(), DescriptorTypes.VIEW_DEFINITION_DESCRIPTOR, kvp.Key.ToString());
                    if (viewDescriptor == null)
                    {
                        viewDescriptor = new ModelDescriptor(kvp.Key.ToString(), DescriptorTypes.VIEW_DEFINITION_DESCRIPTOR)
                        {
                            NamespaceName = namespaceName,
                            ModuleName = moduleName
                        };
                        if (viewType != null)
                        {
                            //componentDescriptor.Set("Anonymous", componentType.is_anonymous.ToString());
                            //componentDescriptor.Set("MandatoryRight", componentType.mandatory_right);
                            //componentDescriptor.Set("OwnershipOverrideRight", componentType.ownership_override_right);
                        }
                        descriptor.AddChild(viewDescriptor);
                    }
                }
            }

            if (value.rules != null)
            {
                foreach (var kvp in value.rules)
                {
                    var ruleType = kvp.Key;

                    foreach (var item in kvp.Value)
                    {
                        ModelDescriptor ruleDescriptor = FindModelDescriptor(descriptor.GetChildren(), RuleGenerator.Name, item);
                        if (ruleDescriptor == null)
                        {
                            ruleDescriptor = new ModelDescriptor(item, RuleGenerator.Name)
                            {
                                NamespaceName = namespaceName,
                                ModuleName = moduleName
                            };
                            ruleDescriptor.Set(ruleType, "true");
                            descriptor.AddChild(ruleDescriptor);
                        }
                        else
                        {
                            ruleDescriptor.Set(ruleType, "true");
                        }
                    }
                }
            }

            descriptor.Set("WIP", "false");
        }

        return descriptor;
    }

    private ModelDescriptor FindModelPropertyDescriptor(string namespaceName, string moduleName, string propertyType)
    {
        ModelDescriptor internalObject = FindModelDescriptor(namespaceName, moduleName, ObjectGenerator.Name, propertyType);

        if (internalObject != null)
        {
            return internalObject;
        }

        foreach (var kvpModule in modelParserEngine.YamlModelContentByModule)
        {
            string subModuleName = this.modelParserEngine.ModuleName;
            if (kvpModule.Key != string.Empty)
                subModuleName += "." + kvpModule.Key;

            if (subModuleName != moduleName)
                continue;

            foreach (var kvp in kvpModule.Value.aggregate_roots)
            {
                if (kvp.Key.Equals(propertyType))
                {
                    return GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Aggregate, namespaceName, subModuleName, null);
                }
            }

            foreach (var kvp in kvpModule.Value.entities)
            {
                if (kvp.Key.Equals(propertyType))
                {
                    return GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Entity, namespaceName, subModuleName, null);
                }
            }

            foreach (var kvp in kvpModule.Value.transients)
            {
                if (kvp.Key.Equals(propertyType))
                {
                    return GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Transient, namespaceName, subModuleName, null);
                }
            }

            foreach (var kvp in kvpModule.Value.value_objects)
            {
                if (kvp.Key.Equals(propertyType))
                {
                    return GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.ValueObject, namespaceName, subModuleName, null);
                }
            }

            foreach (var kvp in kvpModule.Value.events)
            {
                if (kvp.Key.Equals(propertyType))
                {
                    return GetOrCreateObjectDescriptor(kvp.Key, kvp.Value, DomainType.Event, namespaceName, subModuleName, new Dictionary<string, string>
                {
                    { "OnlyEvent", "true" },
                    { "EventDataType", kvp.Value.data_type }
                });
                }

            }
        }

        return null;
    }

    private T GetValue<T>(Dictionary<object, object> dictionary, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            return (T)dictionary[key];
        }
        else
        {
            return default;
        }
    }
    #endregion

    #region Right

    private static ModelDescriptor GetOrCreateRightDescriptor(string namespaceName, string moduleName, FullModelDescription modelDescription, string right)
    {
        ModelDescriptor descriptor = modelDescription.ModelDescriptors.Where(mr => mr.DescriptorType == RightGenerator.Name && mr.NamespaceName == namespaceName && mr.Name == right).SingleOrDefault();
        if (descriptor == null)
        {
            descriptor = new ModelDescriptor(right, RightGenerator.Name)
            {
                NamespaceName = namespaceName,
                ModuleName = moduleName
            };
            modelDescription.ModelDescriptors.Add(descriptor);
        }
        return descriptor;
    }
    #endregion

    #region Property
    private ModelDescriptor GetOrCreateProperty(ModelDescriptor objectDescriptor, string propName, property property, string objectName, base_object baseObject)
    {
        ModelDescriptor descriptor = FindModelDescriptor(objectDescriptor.GetChildren(), "Property", propName);
        if (descriptor == null)
        {
            descriptor = CreatePropertyDescriptor(propName, property, objectDescriptor, objectName, baseObject);
            objectDescriptor.AddChild(descriptor);
        }
        return descriptor;
    }

    public ModelDescriptor CreatePropertyDescriptor(string propName, property property, ModelDescriptor objectDescriptor, string objectName, base_object baseObject)
    {
        ModelDescriptor bProp = null;
        string propertyType = property.type ?? "string";

        string namespaceName = objectDescriptor.NamespaceName;
        string moduleName = objectDescriptor.ModuleName;

        var typeInfo = new TypeInfo(propertyType, moduleName);

        if (typeInfo.TypeType == TypeType.Model) // normal model
        {
            ModelDescriptor internalObject = FindModelPropertyDescriptor(namespaceName, moduleName, typeInfo.Name);

            if (internalObject != null)
            {
                bProp = new ModelDescriptor(propName, PropertyGenerator.Name)
                {
                    NamespaceName = namespaceName,
                    ModuleName = moduleName
                };
                bProp.AddRelated("Object", internalObject);
                bProp.Set("PropertyType", "Model");
                bProp.Set("ModelType", typeInfo.Name);
                SetBasePropertyParameter(bProp, propName, property, objectName, baseObject, objectDescriptor);
                if (typeInfo.IsNavigation)
                {
                    bProp.Set("SpecialType", "Navigation");
                }
                else if (typeInfo.IsCalculated)
                {
                    bProp.Set("SpecialType", "Calculated");
                }
                else if (typeInfo.IsNonPersisted)
                {
                    bProp.Set("SpecialType", "NonPersisted");
                }
            }
            else
            {
                throw new Exception("Unkown model " + typeInfo.Name);
            }
        }
        else if (typeInfo.TypeType == TypeType.ReferencedModel) // referenced model
        {
            bProp = new ModelDescriptor(propName, PropertyGenerator.Name)
            {
                NamespaceName = namespaceName,
                ModuleName = moduleName
            };
            var parts = typeInfo.ModuleName.Split(".");
            var related = new ModelDescriptor(typeInfo.Name, "Object")
            {
                ModuleName = string.Join(".", parts.Skip(1)),
                NamespaceName = parts[0]
            };
            related.Set("DomainType", DomainType.Aggregate.ToString()); // only aggregates should be referenced in other library?
            bProp.AddRelated("Object", related);

            bProp.Set("PropertyType", "ReferencedModel");
            SetBasePropertyParameter(bProp, propName, property, objectName, baseObject, objectDescriptor);
            if (typeInfo.IsNavigation)
            {
                bProp.Set("SpecialType", "Navigation");
            }
            else if (typeInfo.IsCalculated)
            {
                bProp.Set("SpecialType", "Calculated");
            }
            else if (typeInfo.IsNonPersisted)
            {
                bProp.Set("SpecialType", "NonPersisted");
            }

        }
        else if (typeInfo.IsEnum)
        {
            ModelDescriptor prop = FindModelDescriptor(objectDescriptor.NamespaceName, objectDescriptor.ModuleName, EnumGenerator.Name, typeInfo.Name);
            bProp = new ModelDescriptor(propName, PropertyGenerator.Name)
            {
                NamespaceName = namespaceName,
                ModuleName = moduleName
            };
            bProp.AddRelated("Object", prop);
            bProp.Set("PropertyType", "Enum");
            bProp.Set("EnumType", typeInfo.Name);
            SetBasePropertyParameter(bProp, propName, property, objectName, baseObject, objectDescriptor);
            if (typeInfo.IsCalculated)
            {
                bProp.Set("SpecialType", "Calculated");
            }
            else if (typeInfo.IsNonPersisted)
            {
                bProp.Set("SpecialType", "NonPersisted");
            }
        }
        else
        {
            bProp = new ModelDescriptor(propName, PropertyGenerator.Name)
            {
                NamespaceName = namespaceName,
                ModuleName = moduleName
            };
            bProp.Set("PropertyType", "Simple");

            if (typeInfo.Name == "text")
            {
                bProp.Set("SimpleType", "string");
                bProp.Set("MaxSize", "true");
            }
            else
            {
                bProp.Set("SimpleType", typeInfo.Name);
            }
            bProp.Set("FieldSize", property.field_size?.ToString());
            bProp.Set("MaxSize", property.max_size?.ToString());
            SetBasePropertyParameter(bProp, propName, property, objectName, baseObject, objectDescriptor);
            if (typeInfo.IsCalculated)
            {
                bProp.Set("SpecialType", "Calculated");
            }
            else if (typeInfo.IsNonPersisted)
            {
                bProp.Set("SpecialType", "NonPersisted");
            }
        }

        bProp.Set("Null", typeInfo.IsNull.ToString());
        bProp.Set("List", typeInfo.IsArray.ToString());
        if (bProp.Get("SpecialType") == null && (objectDescriptor.Get("DomainType") == "Transient" || bProp.Get("DomainType") == DomainType.Transient.ToString()))
        {
            bProp.Set("SpecialType", "NonPersisted");
        }

        var domainType = bProp.GetRelated("Object")?.Get("DomainType");
        if (domainType == DomainType.ValueObject.ToString() && bProp.Get("SpecialType") == SpecialType.Navigation.ToString())
        {
            throw new Exception($"value object property cannot be navigated: {propName}[{property.type}] in {objectDescriptor.Name}");
        }

        if (bProp.Is("List"))
        {
            if (domainType != DomainType.ValueObject.ToString() && bProp.Get("SpecialType") == null)
                throw new Exception($"only non persisted or calculated or navigation or value object property may be as list: {propName}[{property.type}] in {objectDescriptor.Name}");
        }

        return bProp;
    }

    private void SetBasePropertyParameter(ModelDescriptor baseProp, string name, property property, string parentName, base_object parent, ModelDescriptor modelObject)
    {
        var initialName = baseProp.Name;
        var specialType = property.special_type;

        baseProp.Set("PropertyName", ConversionHelper.ConvertToPascalCase(initialName));
        baseProp.Set("FieldName", "_" + ConversionHelper.ConvertToCamelCase(initialName));
        if (baseProp.Get("PropertyType") == "Model" || baseProp.Get("PropertyType") == "ReferencedModel")
        {
            var relatedObject = baseProp.GetRelated("Object");
            if (relatedObject.Get("DomainType") != "ValueObject")
            {
                baseProp.Set("QueryPropertyName", ConversionHelper.ConvertToPascalCase(initialName) + "Id");
                baseProp.Set("IdPropertyName", ConversionHelper.ConvertToPascalCase(initialName) + "Id");
                baseProp.Set("ColumnName", specialType == SpecialType.DBAlias.ToString() ? property.db_column_name : initialName + "_id");
            }
            else
            {
                baseProp.Set("QueryPropertyName", ConversionHelper.ConvertToPascalCase(initialName));
                baseProp.Set("IdPropertyName", ConversionHelper.ConvertToPascalCase(initialName));
                baseProp.Set("ColumnName", specialType == SpecialType.DBAlias.ToString() ? property.db_column_name : initialName);
            }
        }
        else
        {
            baseProp.Set("QueryPropertyName", ConversionHelper.ConvertToPascalCase(initialName));
            baseProp.Set("ColumnName", specialType == SpecialType.DBAlias.ToString() ? property.db_column_name : initialName);
        }

        baseProp.Set("Unique", property.is_unique.ToString());
        baseProp.Set("UniqueCaseSensitive", property.is_unique_case_sensitive.ToString());
        baseProp.Set("Label", property.is_label.ToString());
        baseProp.Set("Queryable", property.is_queryable.ToString());
        SetMultipleUniqueConstraint(name, property, baseProp, parentName, parent, modelObject);
        baseProp.Set("DefaultValue", property.default_value);
        baseProp.Set("SpecialType", property.special_type);
        baseProp.Set("BackLinkNavigation", property.back_link_navigation);
        baseProp.Set("JoinNavigation", property.join_navigation);
        baseProp.Set("DBColumnName", property.db_column_name);
        baseProp.Set("Private", property.is_private.ToString());
        baseProp.Set("IdColumnName", property.id_column_name);
    }

    private void SetMultipleUniqueConstraint(string name, property prop, ModelDescriptor property, string parentName, base_object parent, ModelDescriptor modelObject)
    {
        List<ModelDescriptor> uniqueConstraintCoMembers = new();
        string[] attributes = prop.multiple_unique_constraint_with?.Split("|");
        if (attributes?.Length > 0)
        {
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} is multiple constraint {1}", parentName, prop.multiple_unique_constraint_with));

            foreach (string coMemberName in attributes)
            {
                ModelDescriptor otherProp = FindPropertyInModelObjectAndCreateIfNotExist(parentName, parent, coMemberName, modelObject);
                if (prop != null)
                {
                    uniqueConstraintCoMembers.Add(otherProp);
                }
            }

            property.Set("MultipleUniqueConstraint", "true");
            List<string> additionalMembers = new();
            foreach (ModelDescriptor item in uniqueConstraintCoMembers)
            {
                additionalMembers.Add(item.Name);
            }
            property.Set("UniqueConstraintAdditionalMembers", string.Join("|", additionalMembers));
        }
    }

    private ModelDescriptor FindPropertyInModelObjectAndCreateIfNotExist(string parentName, base_object parent, string propertyName, ModelDescriptor objectDescriptor)
    {
        ModelDescriptor descriptor = FindModelDescriptor(objectDescriptor.GetChildren(), "Property", propertyName);
        if (descriptor == null)
        {
            foreach (var kvp in parent.properties)
            {
                if (kvp.Key == propertyName)
                {
                    if (kvp.Value is Dictionary<object, object> dic)
                    {
                        var text = new YamlDotNet.Serialization.Serializer().Serialize(dic);
                        var property = new YamlDotNet.Serialization.Deserializer().Deserialize<property>(text);
                        descriptor = CreatePropertyDescriptor(kvp.Key, property, objectDescriptor, parentName, parent);
                    }
                    else
                    {
                        descriptor = CreatePropertyDescriptor(kvp.Key, new property() { type = kvp.Value as string }, objectDescriptor, parentName, parent);
                    }
                    objectDescriptor.AddChild(descriptor);
                }
            }
        }
        return descriptor;
    }
    #endregion
}
