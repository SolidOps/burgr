#nullable enable
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace SolidOps.UM.Infrastructure;
// Object [AG]
public partial class LocalUserRepository(UMDbContext db) : EfRepository<Domain.LocalUser>(db), Domain.ILocalUserRepository
{
    protected override IQueryable<Domain.LocalUser> WithDetails(IQueryable<Domain.LocalUser> query)
    {
        return query

            ;
    }
}
public partial class UserRepository(UMDbContext db) : EfRepository<Domain.User>(db), Domain.IUserRepository
{
    protected override IQueryable<Domain.User> WithDetails(IQueryable<Domain.User> query)
    {
        return query
            // Property [M][LNA][AG][EN]
            .Include(x => x.UserRights)

            ;
    }
}
public partial class InviteRepository(UMDbContext db) : EfRepository<Domain.Invite>(db), Domain.IInviteRepository
{
    protected override IQueryable<Domain.Invite> WithDetails(IQueryable<Domain.Invite> query)
    {
        return query

            ;
    }
}