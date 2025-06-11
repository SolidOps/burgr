using SolidOps.UM.Shared.Domain.Entities;

namespace SolidOps.UM.Shared.Presentation.ETag;

public interface IETagRepository<TEntity, T> : IETagRepository
    where TEntity : IDomainEntity<T>
    where T: struct
{
}

public interface IETagRepository
{
    void ChangeETag(string id);
    void ChangeWholeTableETag();
    void RemoveETag(string id);
    string GetByIdETag(string userId, string id);
    string GetByQueryETag(string userId);
    void Reset();
    void Clear();
}
