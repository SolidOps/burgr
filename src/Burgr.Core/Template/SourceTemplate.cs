using SolidOps.Burgr.Core.Descriptors;
namespace SolidOps.Burgr.Core.Template
{
    public class SourceTemplate
    {
        public SourceTemplate(string content)
        {
            Content = content;
            OverwriteIfExist = true;
        }

        public string Content { get; private set; }

        public List<TemplateDescriptor> TemplateDescriptors
        {
            get;
            set;
        }

        public string FileSuffix
        {
            get;
            set;
        }

        public string DestinationPath { get; set; }

        public string OverrideDestinationFolder { get; set; }

        public string DestinationSuffix { get; set; }

        public bool ForceSeparateDll { get; set; }

        public bool IsGereratePerModelDescription { get; set; }
        public int? ChildrenLevel { get; set; }

        public bool IsUniqueByModule { get; set; }

        public string AdditionalDestinationFolderPath { get; set; }

        public string DestinationLanguage { get; set; }

        public SourceTemplateOptions SourceTemplateOptions { get; set; }

        public string PathStyle { get; set; }

        public bool OverwriteIfExist { get; set; }

        // model-independent: one file per module, named after FileSuffix alone, with the framework suffix
        public bool IsFramework { get; set; }

        // project file of the template folder (project_file), for the projects its templates write to
        public string ProjectFileTemplate { get; set; }

        public bool RemoveConsecutiveLineBreaks { get; set; } = true;

        public override string ToString()
        {
            return FileSuffix;
        }
    }

    public class SourceTemplateOptions
    {
        public string ModelSuffix { get; set; } = string.Empty;

        public string ModelPrefix { get; set; } = string.Empty;
    }
}
