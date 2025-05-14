using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidOps.Burgr.Core.Generators
{
    public interface IModelParserEngine : IDisposable
    {
        string ModelsDirectory { get; set; }
        string ModuleName { get; set; }
        string NamespaceName { get; set; }

        string ModelParserType { get; }
        void BeforeParsing();

        void AfterParsing(ModelDescriptionsRepository modelsRepository);
    }
}
