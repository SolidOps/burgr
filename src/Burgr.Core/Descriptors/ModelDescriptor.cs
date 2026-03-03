namespace SolidOps.Burgr.Core.Descriptors
{
    public class ModelDescriptor : BaseDescriptor<ModelDescriptor>
    {
        public ModelDescriptor(string name, string descriptorType)
        {
            Name = name;
            DescriptorType = descriptorType;
        }

        public string Name { get; private set; }

        public string ModuleName { get; set; }

        public string NamespaceName { get; set; }

        public string FullModuleName
        {
            get
            {
                return NamespaceName + "." + ModuleName;
            }
        }

        public string InternalModuleName
        {
            get
            {
                return ModuleName?.Split(".").Last();
            }
        }

        public override string ToString()
        {
            return DescriptorType + ":" + Name;
        }

        public string PluralName
        {
            get
            {
                string plural = Get("Plural") ?? "s";
                string pluralIndexString = Get("PluralIndex");
                int pluralIndex;
                if (string.IsNullOrEmpty(pluralIndexString) || !int.TryParse(pluralIndexString, out pluralIndex))
                {
                    pluralIndex = 0;
                }
                return Name.Substring(0, Name.Length + pluralIndex) + plural;
            }
        }
    }
}
