using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Core
{
    public interface IInitTemplate
    {
        List<SourceTemplate> Init(string dllFolderPath, string destinationFolder, string overrideDestinationFolder);
    }
}
