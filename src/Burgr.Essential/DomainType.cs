namespace SolidOps.Burgr.Essential;

public enum DomainType
{
    Unknown,
    Aggregate,
    Entity,
    ValueObject,
    UseCase,
    Transient,
    Event
}

public static class DomainTypeHelper
{
    public static string ReplaceDomainType(string content, string modelDomainType)
    {
        return content.Replace("_DOMAINTYPE_", GetDomainType(modelDomainType));
    }

    public static string GetDomainType(string modelDomainType)
    {
        if (modelDomainType == DomainType.Aggregate.ToString())
        {
            return "AggregateRoots";
        }

        if (modelDomainType == DomainType.Entity.ToString())
        {
            return "Entities";
        }

        if (modelDomainType == DomainType.ValueObject.ToString())
        {
            return "ValueObjects";
        }

        if (modelDomainType == DomainType.Transient.ToString())
        {
            return "Transients";
        }

        return "unknown";
    }
}
