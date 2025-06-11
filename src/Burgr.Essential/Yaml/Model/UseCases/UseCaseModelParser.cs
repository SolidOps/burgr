using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Essential.Generators;
using SolidOps.Burgr.Essential.Generators.Common;
using SolidOps.Burgr.Essential.Generators.Rights;
using SolidOps.Burgr.Essential.Generators.UseCases;
using SolidOps.Burgr.Essential.Yaml.Model.Objects;

namespace SolidOps.Burgr.Essential.Yaml.Model.Services;

public class UseCaseModelParser : BaseYamlModelParser, IModelParser
{
    private ModelParserEngine parserEngine;
    private ObjectModelParser objectModelParser;

    public UseCaseModelParser()
    {
        DefaultDescriptorType = UseCaseGenerator.Name;
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
            foreach (var kvp in kvpModule.Value.use_cases)
            {
                GetOrCreateUseCase(kvp.Key, kvp.Value, this.parserEngine.NamespaceName, moduleName);
            }
        }
    }

    private void GetOrCreateUseCase(string name, use_case useCase, string namespaceName, string moduleName)
    {
        if (GetUseCaseObjectInList(name) == null)
        {
            CreateUseCase(name, useCase, namespaceName, moduleName);
        }
    }

    private void CreateUseCase(string name, use_case useCase, string namespaceName, string moduleName)
    {
        if (name.ToLower() != name)
        {
            throw new Exception($"use case name must be in lower case : {name}");
        }

        ModelDescriptor modelDescriptor = new(name, UseCaseGenerator.Name)
        {
            NamespaceName = namespaceName,
            ModuleName = moduleName
        };

        modelDescriptor.Set("Anonymous", useCase.is_anonymous.ToString());
        modelDescriptor.Set("CreateRestServices", useCase.create_rest_services.ToString());

        FullModelDescription modelDescription = GetModelDescription(namespaceName, moduleName);

        if (!string.IsNullOrEmpty(useCase.mandatory_right))
        {
            modelDescriptor.Set("MandatoryRight", useCase.mandatory_right);
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

        if (!string.IsNullOrEmpty(useCase.ownership_override_right))
        {
            modelDescriptor.Set("OwnershipOverrideRight", useCase.ownership_override_right);
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

        modelDescriptor.Set("Internal", useCase.is_internal.ToString());
        modelDescriptor.Set("ImplementsInterfaces", useCase.implements_interfaces);

        foreach (var kvpMethod in useCase.steps)
        {
            step method = kvpMethod.Value ?? new step();

            ReturnType returnType = GetReturnType(method.result, moduleName);

            string descriptorType = returnType switch
            {
                ReturnType.Void => VoidUseCaseStepGenerator.Name,
                ReturnType.Simple => SimpleUseCaseStepGenerator.Name,
                ReturnType.Model => ModelUseCaseStepGenerator.Name,
                ReturnType.ModelList => ModelListUseCaseStepGenerator.Name,
                ReturnType.Identity => IdentityUseCaseStepGenerator.Name,
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
                        throw new Exception($"use case parameter method name must be in lower case : {kvpInput.Key} of {kvpMethod.Key} of {name}");
                    }

                    ModelDescriptor modelServiceMethodParameter = new(kvpInput.Key, DescriptorTypes.USECASE_STEP_PARAMETER_DESCRIPTOR)
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
        if (useCase.dependencies != null)
        {
            foreach (var kvpDependecy in useCase.dependencies)
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
    private ModelDescriptor GetUseCaseObjectInList(string name)
    {
        foreach (KeyValuePair<string, FullModelDescription> kvp in modelsRepository.modelDescriptions)
        {
            foreach (ModelDescriptor en in kvp.Value.ModelDescriptors.Where(m => m.DescriptorType == Generators.UseCases.UseCaseGenerator.Name))
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
