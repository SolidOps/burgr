using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Core.Generators;
using SolidOps.Burgr.Core.Template;

namespace SolidOps.Burgr.Core.Generators
{
    public abstract class BaseGenerator : IGenerator
    {
        public abstract string DescriptorType { get; }

        public virtual string AimedModelDescriptorType { get; } = null;

        public ITemplateParser TemplateParser { get; protected set; }

        public List<IGenerator> SubGenerators { get; private set; } = new List<IGenerator>();

        private int GenerateCount { get; set; }

        public virtual void BeforeGenerate(List<ModelDescriptor> models, TemplateDescriptor template)
        {
            GenerateCount = 0;
        }

        protected virtual string CheckIfApply(ModelDescriptor model, TemplateDescriptor template)
        {
            return null;
        }

        private IModelParser _modelParser = null;
        protected virtual IModelParser InstantiateModelParser(string modelParserType)
        {
            return null;
        }

        public string ModelParserType { get; private set; }

        public void SetModelParserType(string modelParserType)
        {
            ModelParserType = modelParserType;

            if (SubGenerators == null)
                return;

            foreach (var subGenerator in SubGenerators)
            {
                subGenerator?.SetModelParserType(modelParserType);
            }

        }

        public IModelParser GetModelParser(string modelParserType)
        {
            if (_modelParser == null)
            {
                _modelParser = InstantiateModelParser(modelParserType);
            }

            return _modelParser;
        }

        public virtual void ParseModel(IModelParserEngine modelParserEngine, ModelDescriptionsRepository modelsRepository, List<IGenerator> generators)
        {
            SetModelParserType(modelParserEngine.ModelParserType);

            _modelParser = GetModelParser(modelParserEngine.ModelParserType);

            _modelParser?.ParseModel(modelParserEngine, modelsRepository, generators);
        }

        public virtual string Generate(string content, ModelDescriptor model, TemplateDescriptor template, string modelPrefix, string modelSuffix)
        {
            string reason = CheckIfApply(model, template);
            if (reason == null)
            {
                GenerateCount++;
                List<TemplateDescriptor> templateChildren = template.GetChildren();
                List<ModelDescriptor> modelChildren = model.GetChildren();
                if (templateChildren != null && modelChildren != null)
                {
                    string templateResult = string.Empty;
                    foreach (TemplateDescriptor childTemplate in templateChildren)
                    {
                        IGenerator childGenerator = SubGenerators.SingleOrDefault(r => r.DescriptorType == childTemplate.DescriptorType);
                        if (childGenerator == null)
                        {
                            continue;
                        }

                        string generatorDescriptorType = childGenerator.DescriptorType;
                        if (childGenerator.AimedModelDescriptorType != null)
                            generatorDescriptorType = childGenerator.AimedModelDescriptorType;

                        List<ModelDescriptor> childrenModels = modelChildren.Where(md => md.DescriptorType == generatorDescriptorType).ToList();

                        childGenerator.BeforeGenerate(childrenModels, childTemplate);
                        string subResult = string.Empty;
                        foreach (ModelDescriptor childModel in childrenModels)
                        {
                            string childResult = childGenerator.Generate(childTemplate.Content, childModel, childTemplate, modelPrefix, modelSuffix) + Utilities.SingleNewLine;
                            childResult = childGenerator.Clean(childResult, childModel, template);
                            subResult += childResult;
                        }
                        subResult = childGenerator.AfterGenerate(subResult, childTemplate);
                        content = content.Replace(childTemplate.Content, subResult + Utilities.SingleNewLine);
                    }
                }
                content = content.Replace("MODELNAME", model.Name);
                return content;
            }

            return string.Empty;
        }

        public virtual string Clean(string content, ModelDescriptor model, TemplateDescriptor template)
        {
            IEnumerable<string> lines = content.Split("\n").Skip(1);
            string result = string.Join("\n", lines);
            return result;
        }

        public virtual string AfterGenerate(string content, TemplateDescriptor template)
        {
            return GenerateCount == 0 ? Utilities.SingleNewLine : GetHeader(template) + content + Utilities.SingleNewLine;
        }

        public virtual string GetHeader(TemplateDescriptor descriptor)
        {
            return GetCommentMarkerStart(descriptor.DestinationLanguage) + descriptor.DescriptorType + " " + descriptor.Headers + GetCommentMarkerEnd(descriptor.DestinationLanguage) + Utilities.SingleNewLine;
        }

        public static string GetCommentMarkerStart(string language)
        {
            return language switch
            {
                "MySQL" => "-- ",
                "HTML" => "<!--",
                "PUML" => "' ",
                _ => "// ",
            };
        }

        public static string GetCommentMarkerEnd(string language)
        {
            return language switch
            {
                "HTML" => "-->",
                _ => "",
            };
        }
    }

    public enum NavigationType
    {
        None = 0,
        Single = 1,
        List = 2,
        Both = 3
    }

    public enum ArrayOption
    {
        Both = 0,
        OnlyArray = 1,
        OnlyNonArray = 2
    }
}
