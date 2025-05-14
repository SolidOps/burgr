namespace SolidOps.Burgr.Core.Generators
{
    public interface ITemplateParser
    {
        string LoopIdentifier { get; }

        List<string> AdditionalLoopIdentifiers { get; }

        List<TemplateOption> Options { get; }
    }

    public class TemplateOption
    {
        public string Name { get; set; }
        public string Tag { get; set; }
    }
}
