#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
namespace SolidOps.UM.Application;
// Object [AG][EN]
public sealed partial class LocalUserDto
{
    public Guid Id { get; set; }
    // Property [S][NO][CA][NAR][PUO]
    public System.String Name { get; set; } = string.Empty;

    public System.String HashedPassword { get; set; } = string.Empty;

    public static LocalUserDto From(Domain.LocalUser entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Name = entity.Name,

        HashedPassword = entity.HashedPassword,

    };
}
public sealed partial class UserDto
{
    public Guid Id { get; set; }
    // Property [S][NO][CA][NAR][PUO]
    public System.String Email { get; set; } = string.Empty;

    public System.String Provider { get; set; } = string.Empty;

    public System.Boolean TechnicalUser { get; set; } = default(System.Boolean);

    // Property [M][NO][NAR][VO][PUO]
    public Domain.Preferences? Preferences { get; set; } = null!;

    // Property [M][LNA][AG][EN][PUO]
    public List<UserRightDto> UserRights { get; set; } = [];

    public static UserDto From(Domain.User entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Email = entity.Email,

        Provider = entity.Provider,

        TechnicalUser = entity.TechnicalUser,

        // Property [M][NO][NAR][VO][PUO]
        Preferences = entity.Preferences,

        // Property [M][LNA][AG][EN][PUO]
        UserRights = entity.UserRights.Select(UserRightDto.From).ToList(),

    };
}
public sealed partial class UserRightDto
{
    public Guid Id { get; set; }

    // Property [M][NO][NAR][AG][EN][PUO]
    public Guid RightId { get; set; }

    public Guid UserId { get; set; }

    public static UserRightDto From(Domain.UserRight entity) => new()
    {
        Id = entity.Id,

        // Property [M][NO][NAR][AG][EN][PUO]
        RightId = entity.RightId,

        UserId = entity.UserId,

    };
}
public sealed partial class RightDto
{
    public Guid Id { get; set; }
    // Property [S][NO][CA][NAR][PUO]
    public System.String Name { get; set; } = string.Empty;

    public static RightDto From(Domain.Right entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Name = entity.Name,

    };
}
public sealed partial class InviteDto
{
    public Guid Id { get; set; }
    // Property [S][NO][CA][NAR][PUO]
    public System.String Email { get; set; } = string.Empty;

    public System.String CreatorName { get; set; } = string.Empty;

    public System.String CreatorMessage { get; set; } = string.Empty;

    public System.Boolean IsOpen { get; set; } = default(System.Boolean);

    // Property [E][NO][CA][NAR][PUO]
    public Domain.InviteStatus Status { get; set; }

    // Property [M][NO][NAR][AG][EN][PUO]
    public Guid CreatorId { get; set; }

    public static InviteDto From(Domain.Invite entity) => new()
    {
        Id = entity.Id,
        // Property [S][NO][CA][NAR][PUO]
        Email = entity.Email,

        CreatorName = entity.CreatorName,

        CreatorMessage = entity.CreatorMessage,

        IsOpen = entity.IsOpen,

        // Property [E][NO][CA][NAR][PUO]
        Status = entity.Status,

        // Property [M][NO][NAR][AG][EN][PUO]
        CreatorId = entity.CreatorId,

    };
}
// Object [AG][R]
public sealed partial class UserDto
{
    /// <summary>The values accepted by the orderBy parameter of the query.</summary>
    public static readonly IReadOnlyCollection<string> SortableProperties =
    [
        "Id",
        // Property [S][NO][NAR][PUO]
        "Email",

        "Provider",

        "TechnicalUser",

    ];
}
public sealed partial class UserInput
{
    // Property [S][NO][NAR][PUO][NRO]
    public System.String Email { get; set; } = string.Empty;

    public System.String Provider { get; set; } = string.Empty;

    public System.Boolean TechnicalUser { get; set; } = default(System.Boolean);

    // Property [M][NO][NAR][VO][PUO][NRO]
    public Domain.Preferences? Preferences { get; set; }

    // Property [M][LNA][SY][EN][PUO][NRO]
    /// <summary>The whole list: a child without Id is added, a child missing from the list is removed.</summary>
    public List<UserRightInput> UserRights { get; set; } = [];

    /// <summary>Required values, maximum lengths, enum values, value objects and synchronized children, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Email))
        {
            errors["Email"] = ["Email is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Provider))
        {
            errors["Provider"] = ["Provider is required"];
        }

        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Email?.Length > 50)
        {
            errors["Email"] = ["Email is longer than 50 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Provider?.Length > 50)
        {
            errors["Provider"] = ["Provider is longer than 50 characters"];
        }

        // Property [M][NO][NAR][VO][PUO][NRO][N]
        if (Preferences is not null)
        {
            PreferencesValidation.Validate(Preferences, "Preferences", errors);
        }

        // Property [M][LNA][SY][EN][PUO][NRO]
        for (var i = 0; i < UserRights.Count; i++)
        {
            foreach (var (key, messages) in UserRights[i].Validate())
            {
                errors[$"UserRights[{i}].{key}"] = messages;
            }
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.User entity)
    {
        // Property [S][NO][NAR][PUO][NRO]
        entity.Email = Email;

        entity.Provider = Provider;

        entity.TechnicalUser = TechnicalUser;

        // Property [M][NO][NAR][VO][PUO][NRO]
        entity.Preferences = Preferences!;

        // Property [M][LNA][SY][EN][PUO][NRO]
        entity.UserRights.RemoveAll(child => !UserRights.Exists(input => Equals(input.Id, child.Id)));
        foreach (var childInput in UserRights)
        {
            var child = childInput.Id is { } childId ? entity.UserRights.Find(c => c.Id.Equals(childId)) : null;
            if (child is null)
            {
                child = new Domain.UserRight();
                entity.UserRights.Add(child);
            }
            childInput.ApplyTo(child);
        }

    }
}
public sealed partial class InviteDto
{
    /// <summary>The values accepted by the orderBy parameter of the query.</summary>
    public static readonly IReadOnlyCollection<string> SortableProperties =
    [
        "Id",
        // Property [S][NO][NAR][PUO]
        "Email",

        "CreatorName",

        "CreatorMessage",

        // Property [E][NO][NAR][PUO]
        "Status",

        // Property [M][NO][NAR][AG][EN][PUO]
        "CreatorId",

    ];
}
public sealed partial class InviteInput
{
    // Property [S][NO][NAR][PUO][NRO]
    public System.String Email { get; set; } = string.Empty;

    public System.String CreatorName { get; set; } = string.Empty;

    public System.String CreatorMessage { get; set; } = string.Empty;

    // Property [E][NO][NAR][PUO][NRO]
    public Domain.InviteStatus Status { get; set; }

    // Property [M][NO][NAR][AG][EN][PUO][NRO]
    public Guid CreatorId { get; set; }

    /// <summary>Required values, maximum lengths, enum values, value objects and synchronized children, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Email))
        {
            errors["Email"] = ["Email is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(CreatorName))
        {
            errors["CreatorName"] = ["CreatorName is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(CreatorMessage))
        {
            errors["CreatorMessage"] = ["CreatorMessage is required"];
        }

        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Email?.Length > 255)
        {
            errors["Email"] = ["Email is longer than 255 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (CreatorName?.Length > 255)
        {
            errors["CreatorName"] = ["CreatorName is longer than 255 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (CreatorMessage?.Length > 255)
        {
            errors["CreatorMessage"] = ["CreatorMessage is longer than 255 characters"];
        }

        // Property [E][NO][NAR][PUO][NRO][NN]
        if (!Enum.IsDefined(Status))
        {
            errors["Status"] = ["Status is not a valid value"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.Invite entity)
    {
        // Property [S][NO][NAR][PUO][NRO]
        entity.Email = Email;

        entity.CreatorName = CreatorName;

        entity.CreatorMessage = CreatorMessage;

        // Property [E][NO][NAR][PUO][NRO]
        entity.Status = Status;

        // Property [M][NO][NAR][AG][EN][PUO][NRO]
        entity.CreatorId = CreatorId;

    }
}
// Object [EN]
// a child of a synchronized navigation; its references to aggregate roots are set by the navigation
public sealed partial class UserRightInput
{
    /// <summary>The child to update; null for a new child.</summary>
    public Guid? Id { get; set; }

    // Property [M][NO][NAR][EN][PUO][NRO]
    public Guid RightId { get; set; }

    /// <summary>Required values, maximum lengths, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.UserRight entity)
    {

        // Property [M][NO][NAR][EN][PUO][NRO]
        entity.RightId = RightId;

    }
}
// a child of a synchronized navigation; its references to aggregate roots are set by the navigation
public sealed partial class RightInput
{
    /// <summary>The child to update; null for a new child.</summary>
    public Guid? Id { get; set; }
    // Property [S][NO][NAR][PUO][NRO]
    public System.String Name { get; set; } = string.Empty;

    /// <summary>Required values, maximum lengths, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Name))
        {
            errors["Name"] = ["Name is required"];
        }

        // Property [S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (Name?.Length > 50)
        {
            errors["Name"] = ["Name is longer than 50 characters"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
    public void ApplyTo(Domain.Right entity)
    {
        // Property [S][NO][NAR][PUO][NRO]
        entity.Name = Name;

    }
}
// Object [TR]
public sealed partial class InviteResultDto : IValidatable
{
    // Property [S][NP][NAR][PUO]
    public System.String Email { get; set; } = string.Empty;

    public System.String Creator { get; set; } = string.Empty;

    public System.String Message { get; set; } = string.Empty;

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Email))
        {
            errors["Email"] = ["Email is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Creator))
        {
            errors["Creator"] = ["Creator is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Message))
        {
            errors["Message"] = ["Message is required"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
}
public sealed partial class InviteUseRequestDto : IValidatable
{
    // Property [S][NP][NAR][PUO]
    public System.Guid InviteId { get; set; } = default(System.Guid);

    public System.String Password { get; set; } = string.Empty;

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Password))
        {
            errors["Password"] = ["Password is required"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
}
public sealed partial class LoginRequestDto : IValidatable
{
    // Property [S][NP][NAR][PUO]
    public System.String Login { get; set; } = string.Empty;

    public System.String Password { get; set; } = string.Empty;

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Login))
        {
            errors["Login"] = ["Login is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Password))
        {
            errors["Password"] = ["Password is required"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
}
public sealed partial class SelfUserCreationRequestDto : IValidatable
{
    // Property [S][NP][NAR][PUO]
    public System.String Email { get; set; } = string.Empty;

    public System.String Password { get; set; } = string.Empty;

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Email))
        {
            errors["Email"] = ["Email is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Password))
        {
            errors["Password"] = ["Password is required"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
}
public sealed partial class UserCreationInfoDto : IValidatable
{
    // Property [S][NP][NAR][PUO]
    public System.String UserEmail { get; set; } = string.Empty;

    public System.String Password { get; set; } = string.Empty;

    // Property [M][NP][AR][AG][EN][TR][PUO]
    public List<UserRightDto> Rights { get; set; } = [];

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        // Property [S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(UserEmail))
        {
            errors["UserEmail"] = ["UserEmail is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(Password))
        {
            errors["Password"] = ["Password is required"];
        }

        OnValidating(errors);
        return errors;
    }
    partial void OnValidating(Dictionary<string, string[]> errors);
}
// Object [VO]
internal static partial class PreferencesValidation
{
    public static void Validate(Domain.Preferences value, string path, Dictionary<string, string[]> errors)
    {
        // Property [S][NO][NAR]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(value.Language))
        {
            errors[path + ".Language"] = ["Language is required"];
        }

        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(value.TimeZone))
        {
            errors[path + ".TimeZone"] = ["TimeZone is required"];
        }

        // Property [S][NO][NAR]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (value.Language?.Length > 10)
        {
            errors[path + ".Language"] = ["Language is longer than 10 characters"];
        }

        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (value.TimeZone?.Length > 50)
        {
            errors[path + ".TimeZone"] = ["TimeZone is longer than 50 characters"];
        }

    }
}