using SolidOps.Burgr.Core.Descriptors;

namespace SolidOps.Burgr.Core
{
    public class ModelDescriptionsRepository
    {
        public Dictionary<string, FullModelDescription> modelDescriptions { get; private set; } = new Dictionary<string, FullModelDescription>();

        public ModelDescriptionsRepository()
        {

        }
    }
}
