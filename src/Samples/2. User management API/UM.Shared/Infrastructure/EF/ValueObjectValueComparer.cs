
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolidOps.SubZero;

namespace SolidOps.UM.Shared.Infrastructure;

public class ValueObjectValueComparer<T> : ValueConverter<T, string>
{
    public ValueObjectValueComparer() : base(
        x => Serializer.Serialize(x, true, true, false),
        x => Serializer.Deserialize<T>(x, true, true, false)
        )
    {
    }
}
