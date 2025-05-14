namespace SolidOps.Burgr.Core
{
    public class GenerationResult
    {
        public int Id { get; }
        public string Name { get; private set; }
        public GenerationResult(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Header
        {
            get;
            set;
        }

        private string _initialContent;

        public string InitialContent
        {
            get => _initialContent;
            set
            {
                _initialContent = value;
                string[] parts = _initialContent.Split(Utilities.NewLines, StringSplitOptions.None);
                if (parts.Length > 0 && parts[0].StartsWith("["))
                {
                    Header = parts[0];
                }
            }
        }

        public override string ToString()
        {
            return $"{Id}:{Name}";
        }

    }

    public class FinalGenerationResult : GenerationResult
    {
        public FinalGenerationResult(int id, string name) : base(id, name)
        {
        }
        public string FinalContent
        {
            get;
            set;
        }
        public string CompareContent
        {
            get;
            set;
        }
    }
}
