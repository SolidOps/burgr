using System.Text.Json;
using SolidOps.UM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SolidOps.UM.Infrastructure;

/// <summary>Mapping rules shared by every object of the UM module, applied by its DbContext.</summary>
public static class ModelConventions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// A value object is stored as JSON in the row of its owner, nested value objects and
    /// collections included, and compared by value.
    /// </summary>
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> property)
    {
        property.HasConversion(
            value => JsonSerializer.Serialize(value, JsonOptions),
            json => JsonSerializer.Deserialize<T>(json, JsonOptions)!,
            new ValueComparer<T>(
                (left, right) => JsonSerializer.Serialize(left, JsonOptions) == JsonSerializer.Serialize(right, JsonOptions),
                value => JsonSerializer.Serialize(value, JsonOptions).GetHashCode(),
                value => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, JsonOptions), JsonOptions)!));
        return property;
    }

    /// <summary>
    /// An entity referencing an aggregate root is deleted with it; any other reference
    /// prevents the deletion of what it references (a conflict for the API).
    /// </summary>
    public static void ApplyDeleteRules(this ModelBuilder modelBuilder)
    {
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            var dependentIsEntity = !typeof(AggregateRoot).IsAssignableFrom(foreignKey.DeclaringEntityType.ClrType);
            var principalIsAggregate = typeof(AggregateRoot).IsAssignableFrom(foreignKey.PrincipalEntityType.ClrType);
            foreignKey.DeleteBehavior = dependentIsEntity && principalIsAggregate ? DeleteBehavior.Cascade : DeleteBehavior.Restrict;
        }
    }
}
