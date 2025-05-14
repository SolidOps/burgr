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

        public string DestinationLanguage { get; set; }

        public string ModulePath { get; set; }
        public string ModuleName { get; set; }
        public string NamespaceName { get; set; }

        public bool RemoveConsecutiveLineBreaks { get; set; }

        public bool EmptyFolder { get; set; }
    }
}
