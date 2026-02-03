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

        bool isServiceExternal = service.api || service.api_description != null;
        bool isServiceAnonymous = false;
        if (service.api_description != null && service.api_description.is_anonymous.HasValue)
        {
            isServiceAnonymous = service.api_description.is_anonymous.Value;
        }

        FullModelDescription modelDescription = GetModelDescription(namespaceName, moduleName);

        if (!string.IsNullOrEmpty(service.api_description?.mandatory_right))
        {
            modelDescriptor.Set("MandatoryRight", service.api_description?.mandatory_right);
            AddRight(modelDescription, modelDescriptor.Get("MandatoryRight"), modelDescriptor.FullModuleName, modelDescriptor.NamespaceName, modelDescriptor.ModuleName);
        }

        if (!string.IsNullOrEmpty(service.api_description?.ownership_override_right))
        {
            modelDescriptor.Set("OwnershipOverrideRight", service.api_description?.ownership_override_right);
            AddRight(modelDescription, modelDescriptor.Get("OwnershipOverrideRight"), modelDescriptor.FullModuleName, modelDescriptor.NamespaceName, modelDescriptor.ModuleName);
        }

        modelDescriptor.Set("ImplementsInterfaces", service.implements_interfaces);

        foreach (var kvpMethods in service.methods)
        {
            service_method_description method = kvpMethods.Value ?? new service_method_description();

            ReturnType returnType = GetReturnType(method.result, moduleName);

            string descriptorType = returnType switch
            {
                ReturnType.Void => VoidServiceMethodGenerator.Name,
                ReturnType.Simple => SimpleServiceMethodGenerator.Name,
                ReturnType.Model => ModelServiceMethodGenerator.Name,
                ReturnType.ModelList => ModelListServiceMethodGenerator.Name,
                ReturnType.Identity => IdentityServiceMethodGenerator.Name,
                _ => throw new NotImplementedException("Unimplemented return type"),
            };
            ModelDescriptor modelServiceMethod = new(kvpMethods.Key, descriptorType)
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


            modelServiceMethod.Set("MethodMandatoryRight", method.api_description?.mandatory_right);
            AddRight(modelDescription, modelServiceMethod.Get("MethodMandatoryRight"), modelServiceMethod.FullModuleName, modelServiceMethod.NamespaceName, modelServiceMethod.ModuleName);
            modelServiceMethod.Set("OwnershipOverrideRight", method.api_description?.ownership_override_right);
            AddRight(modelDescription, modelServiceMethod.Get("OwnershipOverrideRight"), modelServiceMethod.FullModuleName, modelServiceMethod.NamespaceName, modelServiceMethod.ModuleName);

            if (returnType == ReturnType.Identity)
            {
                modelServiceMethod.Set("IdentityAsLocation", "true");
            }

            modelServiceMethod.Set("ReturnType", returnType.ToString());

            bool isMethodExternal = isServiceExternal || method.api || method.api_description != null;
            bool isMethodAnonymous = isServiceAnonymous;
            if (method.api_description != null && method.api_description.is_anonymous.HasValue)
            {
                isMethodAnonymous = method.api_description.is_anonymous.Value;
            }

            modelServiceMethod.Set("External", isMethodExternal.ToString());
            modelServiceMethod.Set("Anonymous", isMethodAnonymous.ToString());

            modelServiceMethod.Set("ForcePost", method.api_description?.force_post.ToString());

            modelServiceMethod.Set("Component", method.api_description?.create_component.ToString());

            //if (modelDescriptor.Is("CreateRestServices"))
            //{
            //    ModelDescriptor resourceAndMethod = GetOrCreateResource(modelDescriptor, modelServiceMethod, namespaceName, moduleName);
            //    if (resourceAndMethod != null)
            //    {
            //        modelServiceMethod.AddRelated("Object", resourceAndMethod);
            //    }
            //}

            modelDescriptor.AddChild(modelServiceMethod);

            if (method.inputs != null)
            {
                foreach (var kvpInput in method.inputs)
                {
                    if (kvpInput.Key.ToLower() != kvpInput.Key)
                    {
                        throw new Exception($"service parameter method name must be in lower case : {kvpInput.Key} of {kvpMethods.Key} of {name}");
                    }

                    ModelDescriptor modelServiceMethodParameter = new(kvpInput.Key, DescriptorTypes.SERVICE_METHOD_PARAMETER_DESCRIPTOR)
                    {
                        NamespaceName = namespaceName,
                        ModuleName = moduleName
                    };

                    string inputValue = kvpInput.Value ?? "string";

                    var typeInfo = new TypeInfo(inputValue, moduleName);

                    if (typeInfo.IsEnum)
                    {
                        modelServiceMethodParameter.Set("Enum", "true");
                        AddRelatedDescriptor(modelServiceMethodParameter, typeInfo, Generators.Enums.EnumGenerator.Name);
                    }
                    else if (typeInfo.TypeType != TypeType.Simple)
                    {
                        modelServiceMethodParameter.Set("Model", "true");
                        if (typeInfo.IsArray)
                        {
                            AddRelatedDescriptor(modelServiceMethodParameter, typeInfo, Generators.Objects.ObjectGenerator.Name);
                            modelServiceMethodParameter.Set("List", "true");
                        }
                        else
                            AddRelatedDescriptor(modelServiceMethodParameter, typeInfo, Generators.Objects.ObjectGenerator.Name);
                    }
                    else
                    {
                        modelServiceMethodParameter.Set("SimpleType", typeInfo.Name);
                        modelServiceMethodParameter.Set("List", typeInfo.IsArray.ToString());
                        modelServiceMethodParameter.Set("Null", typeInfo.IsNull.ToString());
                    }

                    modelServiceMethod.AddChild(modelServiceMethodParameter);

                    if (typeInfo.Name == "System.IO.Stream")
                    {
                        modelDescriptor.Set("UseStreaming", "true");
                    }
                }
            }

            if (method.api_description?.create_component == true)
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

    private static void AddRight(FullModelDescription modelDescription, string right, string fullModuleName, string @namespace, string moduleName)
    {
        ModelDescriptor found = modelDescription.ModelDescriptors.Where(mr => mr.DescriptorType == RightGenerator.Name && mr.FullModuleName == fullModuleName && mr.Name == right).SingleOrDefault();
        if (found == null)
        {
            modelDescription.ModelDescriptors.Add(new ModelDescriptor(right, RightGenerator.Name)
            {
                NamespaceName = @namespace,
                ModuleName = moduleName
            });
        }
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
            foreach (ModelDescriptor en in kvp.Value.ModelDescriptors.Where(m => m.DescriptorType == ServiceGenerator.Name))
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
