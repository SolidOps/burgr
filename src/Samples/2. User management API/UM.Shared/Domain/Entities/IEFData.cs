using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SolidOps.UM.Shared.Domain.Entities;

public interface IEFObject
{
    ILazyLoader LazyLoader { get; set; }

    bool LazyLoadingEnabled { get; set; }
}

public interface IEFEntity<T, TDomain> : IEFObject, IEntityOfDomain<T>
    where T : struct
    where TDomain : IDomainEntity<T>
{
    T Id { get; set; }
}
