using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;

namespace SolidOps.Burgr.Essential.Yaml.Model;

public class BaseYamlModelParser : BaseModelParser
{
    protected string DefaultDescriptorType { get; set; }

    protected Dictionary<string, BaseYamlModelParser> OtherParsers = new Dictionary<string, BaseYamlModelParser>();

    protected void AddRelatedDescriptor(ModelDescriptor descriptor, TypeInfo typeInfo, string descriptorType)
    {
        if (typeInfo.TypeType == TypeType.Simple)
            return;
        AddRelatedDescriptor(descriptor, typeInfo, descriptorType, descriptor.NamespaceName, descriptor.ModuleName);
    }

    protected void AddRelatedDescriptor(ModelDescriptor descriptor, TypeInfo typeInfo, string descriptorType, string namespaceName, string moduleName)
    {
        ModelDescriptor internalObject = FindModelDescriptor(namespaceName, moduleName, descriptorType, typeInfo.Name);
        if (internalObject == null)
        {
            if (descriptorType == DefaultDescriptorType)
            {
                internalObject = CreateDescriptor(typeInfo, descriptorType, namespaceName, moduleName);
            }
            else
            {
                internalObject = OtherParsers[descriptorType].CreateDescriptor(typeInfo, descriptorType, namespaceName, moduleName);
            }
        }

        descriptor.AddRelated("Object", internalObject);
    }

    public virtual ModelDescriptor GetOrCreateDescriptor(TypeInfo typeInfo, string namespaceName, string moduleName)
    {
        return GetOrCreateDescriptor(typeInfo, DefaultDescriptorType, namespaceName, moduleName);
    }

    protected virtual ModelDescriptor GetOrCreateDescriptor(TypeInfo typeInfo, string descriptorType, string namespaceName, string moduleName)
    {
        ModelDescriptor descriptor = FindModelDescriptor(namespaceName, moduleName, descriptorType, typeInfo.Name);
        if (descriptor == null)
        {
            if (descriptorType == DefaultDescriptorType)
            {
                descriptor = CreateDescriptor(typeInfo, descriptorType, namespaceName, moduleName);
            }
            else
            {
                descriptor = OtherParsers[descriptorType].CreateDescriptor(typeInfo, descriptorType, namespaceName, moduleName);
            }
        }
        return descriptor;
    }

    protected virtual ModelDescriptor CreateDescriptor(TypeInfo typeInfo, string descriptorType, string namespaceName, string moduleName)
    {
        return CreateDescriptor(typeInfo.Name, descriptorType, namespaceName, moduleName);
    }
}
