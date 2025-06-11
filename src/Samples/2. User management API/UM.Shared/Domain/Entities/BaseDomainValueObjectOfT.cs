namespace SolidOps.UM.Shared.Domain.Entities;

[Serializable]
public abstract class BaseDomainValueObject<TValueObject> : BaseDomainValueObject
    where TValueObject : BaseDomainValueObject<TValueObject>
{
    public abstract void CopyValues(TValueObject copy);
}
