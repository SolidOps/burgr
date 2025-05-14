using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Core.Descriptors
{
    public class TemplateDescriptor : BaseDescriptor<TemplateDescriptor>
    {
        public string Headers { get; set; }

        public string Content { get; set; }

        public string DestinationLanguage { get; set; }

        public TemplateDescriptor()
        {

        }

        public override string ToString()
        {
            return DescriptorType;
        }
    }
}
