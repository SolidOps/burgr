using SolidOps.TODO.Shared.Domain;
namespace SolidOps.TODO.Domain.Repositories;
// Object [EN][AG]
public partial interface IItemRepository : IReadWriteDomainRepository<Guid, Domain.AggregateRoots.Item>
{
    // Queries

    // UniqueQueryableProperty 
    Task<Domain.AggregateRoots.Item> GetSingleByName(String name, string includes = null);

}