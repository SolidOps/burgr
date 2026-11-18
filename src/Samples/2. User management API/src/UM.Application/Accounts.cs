using System.Security.Cryptography;
using SolidOps.UM.Domain;

namespace SolidOps.UM.Application;

/// <summary>Issues the token returned by a login. Implemented by the API layer.</summary>
public interface ITokenIssuer
{
    string Issue(string name, IEnumerable<string> rights);
}

internal static class Accounts
{
    public const string LocalProvider = "local";

    // a user and the local user it logs in with, named after its email
    public static async Task<Result<Guid>> CreateAsync(
        IUserRepository users, ILocalUserRepository localUsers, IUnitOfWork unitOfWork,
        string email, string password, IEnumerable<Guid> rightIds, CancellationToken cancellationToken)
    {
        var user = new User { Email = email, Provider = LocalProvider };
        user.UserRights.AddRange(rightIds.Select(rightId => new UserRight { RightId = rightId }));
        users.Add(user);
        localUsers.Add(new LocalUser { Name = email, HashedPassword = Passwords.Hash(password) });
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        return user.Id;
    }
}

internal static class Passwords
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
        {
            return false;
        }
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(parts[0]), Iterations, HashAlgorithmName.SHA256, HashSize);
        return CryptographicOperations.FixedTimeEquals(hash, Convert.FromBase64String(parts[1]));
    }
}
