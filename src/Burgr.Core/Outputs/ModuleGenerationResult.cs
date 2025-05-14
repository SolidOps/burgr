namespace SolidOps.Burgr.Core.Outputs
{
    public class ModuleGenerationResult
    {
        public bool NoneExposed { get; set; }

        public Dictionary<string, FileGenerationResult> FileGenerationResults
        {
            get;
            set;
        }
    }
}
