namespace SolidOps.Burgr.Core
{
    // Names of the module tokens, replaced in the whole file (Tokens of burgr-templates.json).
    // The historical names by default, so that a template set without Tokens is unchanged.
    public class TemplateTokens
    {
        public string ModuleNamespace { get; set; } = CommonTags.Namespace;
        public string Module { get; set; } = CommonTags.ShortModule;
        public string ModuleSlug { get; set; } = CommonTags.Slug + CommonTags.ShortModule;

        public void Check()
        {
            var tokens = new[] { ModuleNamespace, Module, ModuleSlug };
            if (tokens.Any(string.IsNullOrEmpty) || tokens.Distinct(StringComparer.Ordinal).Count() < tokens.Length)
            {
                throw new InvalidOperationException($"Tokens of {TemplateManifest.FileName}: ModuleNamespace, Module and ModuleSlug must be set and different");
            }
        }

        public string Replace(string content, string moduleNamespace, string module, string moduleSlug)
        {
            // longest first: a token may contain another one (_MODULE_ in _MODULE_NAMESPACE_)
            foreach (var (token, value) in new[] { (ModuleNamespace, moduleNamespace), (ModuleSlug, moduleSlug), (Module, module) }.OrderByDescending(t => t.Item1.Length))
            {
                content = content.Replace(token, value);
            }
            return content;
        }
    }
}
