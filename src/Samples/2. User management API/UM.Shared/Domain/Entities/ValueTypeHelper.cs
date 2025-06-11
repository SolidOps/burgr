namespace SolidOps.UM.Shared.Domain.Entities;

public static class ValueTypeHelper
{
    public static bool IsNull(string t)
    {
        return t == null;
    }

    public static bool IsNull(object t)
    {
        return t == null;
    }

    public static bool AreSetAnfDifferent<T>(T value, T reference)
    {
        if (typeof(T).IsClass)
        {
            return value != null && !AreEqual(value, reference);
        }
        else
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != typeof(T)) // Nullable
            {
                return value != null && !AreEqual(value, reference);
            }
            return !AreEqual(value, reference);
        }
    }

    public static bool AreDifferent<T>(T value, T reference)
    {
        return !AreEqual(value, reference);

    }

    public static bool AreEqual<T>(T value, T reference)
    {
        if (typeof(T).IsClass)
        {
            return AreEqual(value as object, reference as object);
        }
        else
        {
            return value.Equals(reference);
        }
    }

    public static bool AreEqual(object value, object reference)
    {
        return value == reference;

    }
}
