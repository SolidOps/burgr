using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidOps.Burgr.Core
{
    public class BurgrConfig
    {
        public string ModuleName { get; set; }
        public string NamespaceName { get; set; }
        public string ModelParserEngineType { get; set; }
        public string TemplateParserEngineType { get; set; }

        public string ModelSpecDirectory { get; set; }
        public string[] BinaryDirectories { get; set; }
        public string TemplateSpecDirectory { get; set; }
        public string BuildingDirectory { get; set; }
        public string IdentityKeysType { get; set; }
        public bool? ModelMonitored { get; set; }
        public bool? OnlyOneDll { get; set; }
        public string ForcedPrefix { get; set; }
        public string OverrideDestination { get; set; }
        public string[] Templates { get; set; }
        public string[] Generators { get; set; }

        public string GeneratedFilePrefix { get; set; }

        public string GeneratedFileSuffix { get; set; }

        public string ToRemoveAtGenerationIdentifier { get; set; }
        public string ToRemoveIfNotMonitoredIdentifier { get; set; }
        public string ToRemoveIfNoAPIIdentifier { get; set; }
    }
}
