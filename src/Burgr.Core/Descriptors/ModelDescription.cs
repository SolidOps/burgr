namespace SolidOps.Burgr.Core.Descriptors
{
    public class FullModelDescription
    {
        public FullModelDescription(string moduleName)
        {
            ModelDescriptors = new List<ModelDescriptor>();
            ModuleName = moduleName;
        }

        public List<ModelDescriptor> ModelDescriptors { get; private set; }
        public string ModuleName { get; }
    }


}
