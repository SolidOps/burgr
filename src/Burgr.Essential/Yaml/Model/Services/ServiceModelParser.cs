using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.Burgr.Essential.Generators.Rights;
using SolidOps.Burgr.Essential.Generators.Services;
using SolidOps.Burgr.Essential.Yaml.Model.Objects;

namespace SolidOps.Burgr.Essential.Yaml.Model.Services;

public class ServiceModelParser : BaseYamlModelParser, IModelParser
{
    private ModelParserEngine parserEngine;
    private ObjectModelParser objectModelParser;

    public ServiceModelParser()
    {
        DefaultDescriptorType = ServiceGenerator.Name;
    }

    public void ParseModel(IModelParserEngine parserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators)
    {
        this.parserEngine = parserEngine as ModelParserEngine;
        this.modelsRepository = modelsRepository;

        objectModelParser = generators.Single(g => g.DescriptorType == "Object").GetModelParser(parserEngine.ModelParserType) as ObjectModelParser;

        foreach (var kvpModule in this.parserEngine.YamlModelContentByModule)
        {
            string moduleName = this.parserEngine.ModuleName;
            if (kvpModule.Key != string.Empty)
                moduleName += "." + kvpModule.Key;
            foreach (var kvp in kvpModule.Value.services)
            {
                GetOrCreateService(kvp.Key, kvp.Value, this.parserEngine.NamespaceName, moduleName);
            }
        }
    }

    private void GetOrCreateService(string name, service service, string namespaceName, string moduleName)
    {
        if (GetServiceObjectInList(name) == null)
        {
            CreateService(name, service, namespaceName, moduleName);
        }
    }

    private void CreateService(string name, service service, string namespaceName, string moduleName)
    {
        if (name.ToLower() != name)
        {
            throw new Exception($"service name must be in lower case : {name}");
        }

        ModelDescriptor modelDescriptor = new(name, ServiceGenerator.Name)
        {
            NamespaceName = namespaceName,
            ModuleName = moduleName
        };

        modelDescriptor.Set("Anonymous", service.is_anonymous.ToString());
        modelDescriptor.Set("CreateRestServices", service.create_rest_services.ToString());

        FullModelDescription modelDescription = GetModelDescription(namespaceName, moduleName);

        if (!string.IsNullOrEmpty(service.mandatory_right))
        {
            modelDescriptor.Set("MandatoryRight", service.mandatory_right);
            ModelDescriptor found = modelDescription.ModelDescriptors.Where(mr => mr.DescriptorType == RightGenerator.Name && mr.FullModuleName == modelDescriptor.FullModuleName && mr.Name == modelDescriptor.Get("MandatoryRight")).SingleOrDefault();
            if (found == null)
            {
                modelDescription.ModelDescriptors.Add(new ModelDescriptor(modelDescriptor.Get("MandatoryRight"), RightGenerator.Name)
                {
                    NamespaceName = modelDescriptor.NamespaceName,
                    ModuleName = modelDescriptor.ModuleName
                });
            }
        }

        if (!string.IsNullOrEmpty(service.ownership_override_right))
        {
            modelDescriptor.Set("OwnershipOverrideRight", service.ownership_override_right);
            ModelDescriptor found = modelDescription.ModelDescriptors.Where(mr => mr.DescriptorType == RightGenerator.Name && mr.FullModuleName == modelDescriptor.FullModuleName && mr.Name == modelDescriptor.Get("OwnershipOverrideRight")).SingleOrDefault();
            if (found == null)
            {
                modelDescription.ModelDescriptors.Add(new ModelDescriptor(modelDescriptor.Get("OwnershipOverrideRight"), RightGenerator.Name)
                {
                    NamespaceName = modelDescriptor.NamespaceName,
                    ModuleName = modelDescriptor.ModuleName
                });
            }
        }

        modelDescriptor.Set("Internal", service.is_internal.ToString());
        modelDescriptor.Set("ImplementsInterfaces", service.implements_interfaces);

        foreach (var kvpMethod in service.methods)
        {
            method method = kvpMethod.Value ?? new method();

            ReturnType returnType = GetReturnType(method.result, moduleName);

            string descriptorType = returnType switch
            {
                ReturnType.Void => VoidMethodGenerator.Name,
                ReturnType.Simple => SimpleMethodGenerator.Name,
                ReturnType.Model => ModelMethodGenerator.Name,
                ReturnType.ModelList => ModelListMethodGenerator.Name,
                ReturnType.Identity => IdentityMethodGenerator.Name,
                _ => throw new NotImplementedException("Unimplemented return type"),
            };
            ModelDescriptor modelServiceMethod = new(kvpMethod.Key, descriptorType)
            {
                NamespaceName = namespaceName,
                ModuleName = moduleName
            };

            if (returnType != ReturnType.Void)
            {
                var typeInfo = new TypeInfo(method.result, moduleName);

                if (typeInfo.IsEnum)
                {
                    modelServiceMethod.Set("Enum", "true");
                    AddRelatedDescriptor(modelServiceMethod, typeInfo, Generators.Enums.EnumGenerator.Name);
                }
                else if (typeInfo.TypeType != TypeType.Simple)
                {
                    if (typeInfo.IsArray)
                    {
                        AddRelatedDescriptor(modelServiceMethod, typeInfo, Generators.Objects.ObjectGenerator.Name);
                        modelServiceMethod.Set("List", "true");
                    }
                    else
                        AddRelatedDescriptor(modelServiceMethod, typeInfo, Generators.Objects.ObjectGenerator.Name);
                }
                else
                {
                    modelServiceMethod.Set("SimpleType", typeInfo.Name);
                    modelServiceMethod.Set("List", typeInfo.IsArray.ToString());
                    modelServiceMethod.Set("Null", typeInfo.IsNull.ToString());
                }
            }

            if (method.result == "System.IO.Stream")
            {
                modelDescriptor.Set("UseStreaming", "true");
            }

            modelServiceMethod.Set("NoTransaction", method.no_transaction.ToString());

            modelServiceMethod.Set("MethodMandatoryRight", method.mandatory_right);
            modelServiceMethod.Set("OwnershipOverrideRight", method.ownership_override_right);

            if (returnType == ReturnType.Identity)
            {
                modelServiceMethod.Set("IdentityAsLocation", "true");
            }

            modelServiceMethod.Set("ReturnType", returnType.ToString());

            modelServiceMethod.Set("ForcePost", method.force_post.ToString());

            modelServiceMethod.Set("Component", method.create_component.ToString());

            if (modelDescriptor.Is("CreateRestServices"))
            {
                ModelDescriptor resourceAndMethod = GetOrCreateResource(modelDescriptor, modelServiceMethod, namespaceName, moduleName);
                if (resourceAndMethod != null)
                {
                    modelServiceMethod.AddRelated("Object", resourceAndMethod);
                }
            }
            modelDescriptor.AddChild(modelServiceMethod);

            if (method.inputs != null)
            {
                foreach (var kvpInput in method.inputs)
                {
                    if (kvpInput.Key.ToLower() != kvpInput.Key)
                    {
                        throw new Exception($"service parameter method name must be in lower case : {kvpInput.Key} of {kvpMethod.Key} of {name}");
                    }

                    ModelDescriptor modelServiceParameterMethod = new(kvpInput.Key, DescriptorTypes.SERVICE_METHOD_PARAMETER_DESCRIPTOR)
                    {
                        NamespaceName = namespaceName,
                        ModuleName = moduleName
                    };

                    string inputValue = kvpInput.Value ?? "string";

                    var typeInfo = new TypeInfo(inputValue, moduleName);

                    if (typeInfo.IsEnum)
                    {
                        modelServiceParameterMethod.Set("Enum", "true");
                        AddRelatedDescriptor(modelServiceParameterMethod, typeInfo, Generators.Enums.EnumGenerator.Name);
                    }
                    else if (typeInfo.TypeType != TypeType.Simple)
                    {
                        if (typeInfo.IsArray)
                        {
                            AddRelatedDescriptor(modelServiceParameterMethod, typeInfo, Generators.Objects.ObjectGenerator.Name);
                            modelServiceParameterMethod.Set("List", "true");
                        }
                        else
                            AddRelatedDescriptor(modelServiceParameterMethod, typeInfo, Generators.Objects.ObjectGenerator.Name);
                    }
                    else
                    {
                        modelServiceParameterMethod.Set("SimpleType", typeInfo.Name);
                        modelServiceParameterMethod.Set("List", typeInfo.IsArray.ToString());
                        modelServiceParameterMethod.Set("Null", typeInfo.IsNull.ToString());
                    }

                    modelServiceMethod.AddChild(modelServiceParameterMethod);

                    if (typeInfo.Name == "System.IO.Stream")
                    {
                        modelDescriptor.Set("UseStreaming", "true");
                    }
                }
            }

            if (method.create_component)
            {
                var result = modelServiceMethod.GetRelated("Object");
                if (result != null)
                {
                    result.Set("Component", "true");
                }
                foreach (var parameter in modelServiceMethod.GetChildren())
                {
                    var related = parameter.GetRelated("Object");
                    if (related != null)
                    {
                        related.Set("Component", "true");
                    }
                }
            }
        }

        // manage dependencies
        if (service.dependencies != null)
        {
            foreach (var kvpDependecy in service.dependencies)
            {
                ModelDescriptor child = new(kvpDependecy.Key, DependencyGenerator.Name)
                {
                    NamespaceName = namespaceName,
                    ModuleName = moduleName
                };
                AddRelatedDescriptor(child, new TypeInfo(kvpDependecy.Key, moduleName), "Object");
                modelDescriptor.AddChild(child);
            }
        }

        modelDescription.ModelDescriptors.Add(modelDescriptor);
    }

    public static ReturnType GetReturnType(string returnType, string moduleName)
    {
        if (returnType == null)
            return ReturnType.Void;

        if (returnType == "identity")
            return ReturnType.Identity;

        var typeInfo = new TypeInfo(returnType, moduleName);
        if (typeInfo.TypeType == TypeType.Model)
        {
            if (typeInfo.IsArray)
                return ReturnType.ModelList;
            return ReturnType.Model;
        }

        return ReturnType.Simple;

    }

    private ModelDescriptor GetOrCreateResource(ModelDescriptor modelUC, ModelDescriptor modelServiceMethod, string namespaceName, string moduleName)
    {
        _ = GetModelDescription(namespaceName, moduleName);

        Type resourceType = null;

        if (resourceType == null)
        {
            resourceType = typeof(Basic);
        }

        string resourceName = resourceType.Name;

        if (resourceName.EndsWith("[]"))
        {
            _ = resourceName.Replace("[]", string.Empty);
        }

        return null;
    }
    private ModelDescriptor GetServiceObjectInList(string name)
    {
        foreach (KeyValuePair<string, FullModelDescription> kvp in modelsRepository.modelDescriptions)
        {
            foreach (ModelDescriptor en in kvp.Value.ModelDescriptors.Where(m => m.DescriptorType == Generators.Services.ServiceGenerator.Name))
            {
                if (en.Name == name)
                {
                    return en;
                }
            }
        }

        return null;
    }

    public static bool IsNonPersistent(string searchTypeFullName, Dictionary<string, FullModelDescription> modelDescriptions)
    {
        return false;
    }
}
