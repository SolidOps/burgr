namespace SolidOps.Burgr.Core.Descriptors
{
    public class ModelDescriptor : BaseDescriptor<ModelDescriptor>
    {
        public ModelDescriptor(string name, string descriptorType)
        {
            Name = name;
            DescriptorType = descriptorType;
        }

        public string Name { get; private set; }

        public string ModuleName { get; set; }

        public string NamespaceName { get; set; }

        public string FullModuleName
        {
            get
            {
                return NamespaceName + "." + ModuleName;
            }
        }

        public override string ToString()
        {
            return DescriptorType + ":" + Name;
        }
    }
}
