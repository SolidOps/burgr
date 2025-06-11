using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Queries;
namespace SolidOps.UM.Domain.Repositories
{
    public partial interface IUMDataAccessFactory : IDataAccessFactory
    {
    }
    // Object [EN][AG]
    public partial interface ILocalUserRepository : IReadWriteDomainRepository<Guid, Domain.AggregateRoots.LocalUser>
    {
        // Queries

        // UniqueQueryableProperty 
        Task<Domain.AggregateRoots.LocalUser> GetSingleByName(String name, string includes = null, bool disableAccessRightCheck = false);

        Task<IEnumerable<Domain.AggregateRoots.LocalUser>> GetList(LocalUserQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);
        Task<Domain.AggregateRoots.LocalUser> GetSingle(LocalUserQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);
        Domain.AggregateRoots.LocalUser MapToDomain(Domain.AggregateRoots.LocalUser efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

    public partial interface IUserRepository : IReadWriteDomainRepository<Guid, Domain.AggregateRoots.User>
    {
        // Queries

        // UniqueQueryableProperty 
        Task<Domain.AggregateRoots.User> GetSingleByEmail(String email, string includes = null, bool disableAccessRightCheck = false);

        Task<IEnumerable<Domain.AggregateRoots.User>> GetList(UserQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);
        Task<Domain.AggregateRoots.User> GetSingle(UserQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);
        Domain.AggregateRoots.User MapToDomain(Domain.AggregateRoots.User efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

    public partial interface IUserRightRepository : IReadWriteDomainRepository<Guid, Domain.Entities.UserRight>
    {
        // Queries

        Task<IEnumerable<Domain.Entities.UserRight>> GetList(UserRightQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);
        Task<Domain.Entities.UserRight> GetSingle(UserRightQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);
        Domain.Entities.UserRight MapToDomain(Domain.Entities.UserRight efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

    public partial interface IRightRepository : IReadWriteDomainRepository<Guid, Domain.Entities.Right>
    {
        // Queries

        // UniqueQueryableProperty 
        Task<Domain.Entities.Right> GetSingleByName(String name, string includes = null, bool disableAccessRightCheck = false);

        Task<IEnumerable<Domain.Entities.Right>> GetList(RightQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);
        Task<Domain.Entities.Right> GetSingle(RightQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);
        Domain.Entities.Right MapToDomain(Domain.Entities.Right efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

    public partial interface IInviteRepository : IReadWriteDomainRepository<Guid, Domain.AggregateRoots.Invite>
    {
        // Queries
        // QueryableProperty 
        Task<IEnumerable<Domain.AggregateRoots.Invite>> GetListByEmail(string _email, string includes = null, bool disableAccessRightCheck = false);

        Task<IEnumerable<Domain.AggregateRoots.Invite>> GetList(InviteQueryFilter filter = null, string includes = null, bool disableAccessRightCheck = false);
        Task<Domain.AggregateRoots.Invite> GetSingle(InviteQueryFilter filter, string includes = null, bool disableAccessRightCheck = false);
        Domain.AggregateRoots.Invite MapToDomain(Domain.AggregateRoots.Invite efEntity, Dictionary<Type, List<object>> mappingCache, bool enableLazyLoading = false);
    }

}
namespace SolidOps.UM.Domain.Queries
{
    // Object [EN][AG]
    public class LocalUserQueryFilter : BaseQueryFilter
    {
        public LocalUserQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {
        }
    }

    public class UserQueryFilter : BaseQueryFilter
    {
        public UserQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {
        }
    }

    public class UserRightQueryFilter : BaseQueryFilter
    {
        public UserRightQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {
        }
    }

    public class RightQueryFilter : BaseQueryFilter
    {
        public RightQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {
        }
    }

    public class InviteQueryFilter : BaseQueryFilter
    {
        public InviteQueryFilter(string filter = null, string orderBy = null, int? maxResults = null, string continuationId = null)
            : base(filter, orderBy, maxResults, continuationId)
        {
        }
    }

}