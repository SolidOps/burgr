namespace SolidOps.Burgr.Core.Descriptors
{
    public interface IDescriptor
    {
        string DescriptorType { get; }
        string LoopIdentifier { get; }
    }
}
