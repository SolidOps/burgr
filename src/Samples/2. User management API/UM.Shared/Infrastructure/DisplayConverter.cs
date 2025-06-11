using System.Collections;
using System.Globalization;

namespace SolidOps.UM.Shared.Infrastructure;

public static class DisplayConverter
{
    public static object ConvertTo(object any)
    {
        throw new NotImplementedException();
    }
    public static DateTime? ConvertToNullableDateTime(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToDateTime(value);
    }

    public static int? ConvertToNullableInt32(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToInt32(value);
    }

    public static Int64? ConvertToNullableInt64(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToInt64(value);
    }

    public static uint? ConvertToNullableUInt32(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToUInt32(value);
    }

    public static double? ConvertToNullableDouble(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToDouble(value);
    }

    public static decimal? ConvertToNullableDecimal(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToDecimal(value);
    }

    public static Boolean? ConvertToNullableBoolean(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToBoolean(value);
    }

    public static DateTime ConvertToDateTime(string value)
    {
        return DateTime.Parse(value, CultureInfo.InvariantCulture);
    }

    public static bool ConvertToBoolean(string value)
    {
        return value == "0" ? false : true;
    }

    public static Guid ConvertToGuid(string value)
    {
        return new Guid(value);
    }

    public static Guid? ConvertToNullableGuid(string value)
    {
        if (!string.IsNullOrEmpty(value) && value != "00000000-0000-0000-0000-000000000000")
            return new Guid(value);
        return null;
    }

    public static T ConvertToIdentity<T>(string value)
    {
        if (typeof(T) == typeof(int?))
            return (T)(object)ConvertToNullableInt32(value);
        if (typeof(T) == typeof(Guid?))
            return (T)(object)ConvertToNullableGuid(value);
        if (typeof(T) == typeof(int))
            return (T)(object)ConvertToInt32(value);
        if (typeof(T) == typeof(Guid))
            return (T)(object)ConvertToGuid(value);
        if (typeof(T) == typeof(string))
            return (T)(object)ConvertToString(value);
        throw new Exception("unkown type for T");
    }

    public static int ConvertToInt32(string value)
    {
        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    public static Int64 ConvertToInt64(string value)
    {
        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    public static uint ConvertToUInt32(string value)
    {
        return uint.Parse(value, CultureInfo.InvariantCulture);
    }

    public static ulong ConvertToUInt64(string value)
    {
        return ulong.Parse(value, CultureInfo.InvariantCulture);
    }

    public static ulong? ConvertToNullableUInt64(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToUInt64(value);
    }

    public static double ConvertToDouble(string value)
    {
        return double.Parse(value, CultureInfo.InvariantCulture);
    }

    public static decimal ConvertToDecimal(string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public static string ConvertToString(string value)
    {
        return value;
    }

    public static T ConvertToEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static byte[] ConvertToArrayOfByte(string value)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetBytes(value);
    }

    public static T ConvertToNullableEnum<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);
        return (T)Enum.Parse(typeof(T), value);
    }

    public static MemoryStream ConvertToMemoryStream(string value)
    {
        if (value == null)
            return null;
        return null;
    }

    public static T1 GetEnumData<T1, T2, T3>(T3 value)
    {
        var enumType = typeof(T2);
        if(enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value != null)
            return (T1)Enum.Parse(enumType, value.ToString());
        return default(T1);
    }

    public static T1 GetEnumData<T1, T2>(object value)
    {
        var enumType = typeof(T2);
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value != null)
            return (T1)Enum.ToObject(enumType, value);
        return default(T1);
    }

    public static T1 GetEnumData<T1, T2>(string value)
    {
        var enumType = typeof(T2);
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value != null)
            return (T1)Enum.Parse(enumType, value);
        return default(T1);
    }

    public static List<T1> GetEnumDataList<T1, T2, T3>(List<T3> values)
    {
        if (values == null)
            return null;
        List<T1> result = new List<T1>();
        foreach (var value in (IEnumerable)values)
            result.Add(GetEnumData<T1, T2>(value.ToString()));
        return result;
    }
    public static List<T1> GetEnumDataList<T1, T2>(IEnumerable values)
    {
        List<T1> result = new List<T1>();
        foreach (var value in values)
            result.Add(GetEnumData<T1, T2>(value));
        return result;
    }
}
