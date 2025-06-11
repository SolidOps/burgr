namespace SolidOps.TODO.Shared.Domain;

public interface IEntityOfDomain<T>
    where T : struct
{
    T Id { get; }
}
