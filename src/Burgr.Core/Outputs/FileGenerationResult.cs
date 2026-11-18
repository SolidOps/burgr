using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Core.Outputs
{
    public class FileGenerationResult : GenerationResult
    {
        public FileGenerationResult(int id) : base(id, "File")
        {

        }

        public Dictionary<string, Dictionary<string, FinalGenerationResult>> FinalGenerationResults { get; set; }

        public bool OverwriteIfExist { get; set; }
        public bool IsFramework { get; set; }
        public string ProjectFileTemplate { get; set; }
        // the module part of ModuleName, without the layer of a separate project: Library for Library.Domain
        public string ModuleBaseName { get; set; }

        public string DestinationLanguage { get; set; }

        public string ModulePath { get; set; }
        public string ModuleName { get; set; }
        public string NamespaceName { get; set; }

        public bool RemoveConsecutiveLineBreaks { get; set; }

        public bool EmptyFolder { get; set; }
    }
}
