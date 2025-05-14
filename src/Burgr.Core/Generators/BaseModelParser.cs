using SolidOps.Burgr.Core.Descriptors;
using System.Runtime.Serialization;

namespace SolidOps.Burgr.Core.Generators
{
    public abstract class BaseModelParser
    {
        protected ModelDescriptionsRepository modelsRepository { get; set; }

        protected BaseModelParser()
        {

        }

        protected virtual ModelDescriptor FindModelDescriptor(string namespaceName, string moduleName, string descriptorType, string modelName)
        {
            if (namespaceName == null)
                throw new ArgumentNullException(nameof(namespaceName));
            if (moduleName == null)
                throw new ArgumentNullException(nameof(moduleName));
            if (descriptorType == null)
                throw new ArgumentNullException(nameof(descriptorType));
            if (modelName == null)
                throw new ArgumentNullException(nameof(modelName));

            string fullModuleName = namespaceName + "." + moduleName;
            if (!modelsRepository.modelDescriptions.ContainsKey(fullModuleName))
                return null;

            return modelsRepository.modelDescriptions[fullModuleName].ModelDescriptors.SingleOrDefault(m => m.Name == modelName && m.DescriptorType == descriptorType);
        }

        protected FullModelDescription GetModelDescription(string namespaceName, string moduleName)
        {
            string fullModuleName = namespaceName + "." + moduleName;
            if (!modelsRepository.modelDescriptions.ContainsKey(fullModuleName))
            {
                modelsRepository.modelDescriptions.Add(fullModuleName, new FullModelDescription(fullModuleName));
            }

            return modelsRepository.modelDescriptions[fullModuleName];
        }

        protected ModelDescriptor FindModelDescriptor(List<ModelDescriptor> modelDescriptors, string descriptorType, string modelName)
        {
            if (descriptorType == null)
                throw new ArgumentNullException(nameof(descriptorType));
            if (modelName == null)
                throw new ArgumentNullException(nameof(modelName));

            return modelDescriptors.SingleOrDefault(m => m.Name == modelName && m.DescriptorType == descriptorType);
        }

        protected virtual ModelDescriptor GetOrCreateDescriptor(string name, string descriptorType, string namespaceName, string moduleName)
        {
            ModelDescriptor descriptor = FindModelDescriptor(namespaceName, moduleName, descriptorType, name);
            if (descriptor == null)
            {
                descriptor = CreateDescriptor(name, descriptorType, namespaceName, moduleName);
            }
            return descriptor;
        }

        protected virtual ModelDescriptor CreateDescriptor(string name, string descriptorType, string namespaceName, string moduleName)
        {
            var descriptor = new ModelDescriptor(name, descriptorType)
            {
                ModuleName = moduleName,
                NamespaceName = namespaceName
            };
            FullModelDescription modelDescription = GetModelDescription(namespaceName, moduleName);
            modelDescription.ModelDescriptors.Add(descriptor);
            descriptor.Set("WIP", "true");
            return descriptor;
        }
    }
}
