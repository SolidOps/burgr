namespace SolidOps.UM.Application;

/// <summary>
/// The caller of a use case. A right is a claim of type <see cref="RightClaimType"/>; the
/// right <see cref="AllRights"/> grants every right. Implemented by the API layer.
/// </summary>
public interface ICurrentUser
{
    public const string RightClaimType = "right";
    public const string AllRights = "*";

    bool IsAuthenticated { get; }

    /// <summary>The name of the authenticated user; null when anonymous.</summary>
    string? Name { get; }

    bool HasRight(string right);
}

/// <summary>
/// A request body that validates itself: the API answers 400 with its errors, by property
/// path, before calling the service.
/// </summary>
public interface IValidatable
{
    Dictionary<string, string[]> Validate();
}
