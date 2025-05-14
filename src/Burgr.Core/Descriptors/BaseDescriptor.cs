using System.ComponentModel;

namespace SolidOps.Burgr.Core.Descriptors
{
    public abstract class BaseDescriptor<T> where T : BaseDescriptor<T>
    {
        public string DescriptorType { get; set; }

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        private List<T> Children { get; } = new List<T>();

        public T Parent { get; set; }

        private Dictionary<string, T> _related { get; set; } = new Dictionary<string, T>();

        public bool Is(string option)
        {
            return Attributes.ContainsKey(option) && Attributes[option] != null && Attributes[option].ToLower() == "true";
        }

        public string Get(string option)
        {
            return Attributes.ContainsKey(option) ? Attributes[option] : null;
        }

        public T Get<T>(string option)
        {
            string value = Get(option);
            if (value != null)
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)(converter.ConvertFromInvariantString(value));
            }
            return default;
        }

        public List<T> GetChildren(params string[] descriptorTypes)
        {
            return descriptorTypes == null || descriptorTypes.Length == 0
                ? Children
                : Children.Where(c => descriptorTypes.Contains(c.DescriptorType)).ToList();
        }
        public List<string> GetList(string option, string separator = "|")
        {
            string value = Get(option);
            return value != null ? value.Split(separator).ToList() : new List<string>();
        }

        public void Set(string option, string value)
        {
            if (value == null)
                return;

            if (!Attributes.ContainsKey(option))
            {
                Attributes.Add(option, value);
            }
            else
            {
                Attributes[option] = value;
            }
        }

        public void AddChild(T child)
        {
            Children.Add(child);
            child.Parent = this as T;
        }

        public void AddRelated(string relation, T related)
        {
            _related.Add(relation, related);
        }

        public T GetRelated(string relation)
        {
            return _related.ContainsKey(relation) ? _related[relation] : null;
        }
    }
}
