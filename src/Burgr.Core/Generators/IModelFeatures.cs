namespace SolidOps.Burgr.Core.Generators
{
    // Implemented by a model parser engine that can tell which model features the parsed
    // models use, checked against the SupportedFeatures of the template set's manifest.
    public interface IModelFeatures
    {
        IReadOnlyCollection<string> GetUsedFeatures();
    }
}
