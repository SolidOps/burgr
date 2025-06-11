using SolidOps.UM.Domain.AggregateRoots;

namespace SolidOps.UM.Domain.Repositories;

public partial interface IUserRepository
{
    string CreateToken(User user);
}
