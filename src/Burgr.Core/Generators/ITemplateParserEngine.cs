using SolidOps.Burgr.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidOps.Burgr.Core.Generators
{
    public interface ITemplateParserEngine : IDisposable
    {
        string TemplatesDirectory { get; set; }
        string ModuleName { get; set; }
        string NamespaceName { get; set; }

        string TemplateParserType { get; }
        void Init();
        List<SourceTemplate> Parse(string templatesDirectory, List<string> templates, Dictionary<string, IGenerator> generators, string filter = null);
    }
}
