using SolidOps.Burgr.Core.Template;
using SolidOps.SubZero;
using System.Reflection;

namespace SolidOps.Burgr.Core
{
    public class Utilities
    {
        public const string SingleNewLine = "\r\n";
        public const string AltNewLine = "\n";
        public static readonly string[] NewLines = new string[] { SingleNewLine, AltNewLine };

        private Utilities()
        {

        }

        public static Utilities Instance { get; private set; } = new Utilities();

        public int Counter { get; set; }

        public static string CombinePath(params string[] args)
        {
            string path = string.Empty;
            if (args != null)
            {
                foreach (string arg in args)
                {
                    path = string.IsNullOrEmpty(path) ? arg : Path.Combine(path, arg);
                }
            }

            return path;
        }

        public static string ReadResource(string dllFolderPath, string assemblyName, string resourceName)
        {
            Assembly ass = Assembly.LoadFrom(Path.Combine(dllFolderPath, assemblyName + ".dll"));
            return ass.ReadResource(resourceName);
        }

        //public static string GetModuleRoot(Type type)
        //{
        //    string[] parts = type.Namespace.Split('.');
        //    List<string> list = parts.Reverse()
        //                .SkipWhile(p => p != "Model")
        //                .Skip(1)
        //                .Reverse()
        //                .ToList();

        //    string res = list[0];
        //    for (int i = 1; i < list.Count; i++)
        //    {
        //        res += "." + list[i];
        //    }
        //    return res;
        //}

        public static string GetNamespace(Type type)
        {
            return GetNamespace(type.Namespace);
        }

        public static string GetNamespace(string typeNamespace)
        {
            string[] parts = typeNamespace.Split(new char[] { '.' });
            string dependencyNamespace = "";
            bool first = true;
            foreach (string part in parts)
            {
                if (part == "Model")
                {
                    break;
                }
                if (!first)
                {
                    dependencyNamespace += ".";
                }
                dependencyNamespace += part;
                first = false;
            }
            return dependencyNamespace;
        }

        public static Tuple<string, string> GetLoopIdentifiers(string identifier)
        {
            return new Tuple<string, string>(GetStartLoopIdentifier(identifier), GetEndLoopIdentifier(identifier));
        }

        public static string GetStartLoopIdentifier(string identifier)
        {
            return GeneratorOptions.Tags.StartLoopKeyword + identifier;
        }

        public static string GetEndLoopIdentifier(string identifier)
        {
            return GeneratorOptions.Tags.EndLoopKeyword + identifier;
        }

        public static List<string> GetInnerTemplates(string overallTemplate, Tuple<string, string> startAndEndSequences)
        {
            return GetInnerTemplates(overallTemplate, startAndEndSequences.Item1, startAndEndSequences.Item2);
        }
        public static List<string> GetInnerTemplates(string overallTemplate, string startSequence, string endSequence)
        {
            if (overallTemplate == null)
            {
                throw new ArgumentNullException("overallTemplate");
            }

            if (startSequence == null)
            {
                throw new ArgumentNullException("startSequence");
            }

            List<string> lst = new();

            int start = 0;
            int index = 0;
            while (index != -1 && start != -1)
            {
                index = overallTemplate.IndexOf(startSequence, start, StringComparison.Ordinal);
                if (index < 0)
                {
                    break;
                }

                int startIndex = index + startSequence.Length;
                int endIndex = overallTemplate.IndexOf(endSequence, startIndex, StringComparison.Ordinal);

                if (index != -1 && endIndex != -1)
                {
                    string inner = overallTemplate[startIndex..endIndex];
                    if (inner.EndsWith("//"))
                    {
                        inner = inner[..^2];
                    }
                    lst.Add(inner);
                }

                start = endIndex;
            }

            return lst;
        }

        public static string ReplaceContent(string generatedContent, FinalGenerationResult generationResult, string destinationLanguage)
        {
            if (generationResult.FinalContent.Trim() != string.Empty)
            {
                string finalContent = generationResult.FinalContent;
                generatedContent = generatedContent.Replace(generationResult.InitialContent, finalContent);
            }
            else
            {
                generatedContent = generatedContent.Replace(generationResult.InitialContent, SingleNewLine);
            }

            return generatedContent;
        }

        public static string GetFileName(string moduleName, string fileSuffix, string pathStyle, string generatedFilePrefix, string generatedFileSuffix)
        {
            string fileName = moduleName;
            if (fileName.Contains("."))
            {
                fileName = fileName.Split(".").Last();
            }

            string[] parts = fileSuffix.Split(".");
            string extension = parts.Last();
            string suffix = string.Join(".", parts.Take(parts.Length - 1));
            fileName = pathStyle == "Lower" ? generatedFilePrefix + fileName.ToLower() + suffix + generatedFileSuffix + "." + extension : generatedFilePrefix + fileName + suffix + generatedFileSuffix + "." + extension;
            return fileName;
        }
    }


    //public static class TypeExtension
    //{
    //    public static string GetAssemblyQualifiedTypeName(this Type type)
    //    {
    //        return type.AssemblyQualifiedName + ", " + type.Namespace + "." + type.Name;
    //    }
    //}
}
