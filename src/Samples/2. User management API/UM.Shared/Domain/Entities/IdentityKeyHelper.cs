namespace SolidOps.UM.Shared.Domain.Entities;

public static class IdentityKeyHelper<T>
{
    public static bool IsDefault(T value)
    {
        return value.Equals(DefaultValue());
    }

    public static T NewValue(bool useEF = false)
    {
        if (typeof(T) == typeof(int))
            return (T)(object)-99;
        if (typeof(T) == typeof(ulong))
        {
            if (useEF)
                return (T)(object)ulong.MinValue;
            return (T)(object)ulong.MaxValue;
        }
        if (typeof(T) == typeof(Guid))
            return (T)(object)Guid.NewGuid();
        throw new Exception("unkown type for T");
    }

    public static T DefaultValue()
    {
        if (typeof(T) == typeof(int))
            return (T)(object)0;
        if (typeof(T) == typeof(ulong))
            return (T)(object)ulong.MinValue;
        if (typeof(T) == typeof(Guid))
            return (T)(object)Guid.Empty;
        throw new Exception("unkown type for T");
    }

    public static bool IsInvalid(T value)
    {
        if (typeof(T) == typeof(int) || typeof(T) == typeof(ulong))
        {
            return value.Equals(DefaultValue()) || value.Equals(NewValue());
        }
        if (typeof(T) == typeof(Guid))
        {
            return value.Equals(DefaultValue());
        }
        throw new Exception("unkown type for T");
    }

    public static T ReadString(string value)
    {
        Type NonNullType = Nullable.GetUnderlyingType(typeof(T));

        if (NonNullType != null)
        {
            if (string.IsNullOrEmpty(value))
                return default;

            if (NonNullType == typeof(int))
                return (T)(object)int.Parse(value);
            if (NonNullType == typeof(ulong))
                return (T)(object)ulong.Parse(value);
            if (NonNullType == typeof(Guid))
                return (T)(object)Guid.Parse(value);
            throw new Exception("unkown type for T");
        }
        else
        {
            if (string.IsNullOrEmpty(value))
                return DefaultValue();
            if (typeof(T) == typeof(int))
                return (T)(object)int.Parse(value);
            if (typeof(T) == typeof(ulong))
                return (T)(object)ulong.Parse(value);
            if (typeof(T) == typeof(Guid))
                return (T)(object)Guid.Parse(value);
            throw new Exception("unkown type for T");
        }
    }
}
