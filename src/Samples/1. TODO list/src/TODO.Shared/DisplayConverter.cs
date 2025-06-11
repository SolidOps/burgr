using SolidOps.TODO.Shared.Domain.Results;
using System.Collections;
using System.Globalization;

namespace SolidOps.TODO.Shared;

public static class DisplayConverter
{
    public static IOpsResult<object> ConvertTo(object any)
    {
        throw new NotImplementedException();
    }
    public static IOpsResult<DateTime?> ConvertToNullableDateTime(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<DateTime?>(null);
        return ConvertToDateTime(value).ToResult<DateTime?>();
    }

    public static IOpsResult<int?> ConvertToNullableInt32(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<int?>(null);
        return ConvertToInt32(value).ToResult<int?>();
    }

    public static IOpsResult<Int64?> ConvertToNullableInt64(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<Int64?>(null);
        return ConvertToInt64(value).ToResult<Int64?>();
    }

    public static IOpsResult<uint?> ConvertToNullableUInt32(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<uint?>(null);
        return ConvertToUInt32(value).ToResult<uint?>();
    }

    public static IOpsResult<double?> ConvertToNullableDouble(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<double?>(null);
        return ConvertToDouble(value).ToResult<double?>();
    }

    public static IOpsResult<decimal?> ConvertToNullableDecimal(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<decimal?>(null);
        return ConvertToDecimal(value).ToResult<decimal?>();
    }

    public static IOpsResult<Boolean?> ConvertToNullableBoolean(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok<Boolean?>(null);
        return ConvertToBoolean(value).ToResult<Boolean?>();
    }

    public static IOpsResult<DateTime> ConvertToDateTime(string value)
    {
        DateTime result;
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }

        return IOpsResult.Invalid($"input {value} is invalid").ToResult<DateTime>();
    }

    public static IOpsResult<bool> ConvertToBoolean(string value)
    {
        return value == "0" ? IOpsResult.Ok(false) : IOpsResult.Ok(true);
    }

    public static IOpsResult<Guid> ConvertToGuid(string value)
    {
        return IOpsResult.Ok(new Guid(value));
    }

    public static IOpsResult<Guid?> ConvertToNullableGuid(string value)
    {
        if (!string.IsNullOrEmpty(value) && value != "00000000-0000-0000-0000-000000000000")
            return IOpsResult.Ok(new Guid(value)).ToResult<Guid?>();
        return null;
    }

    public static IOpsResult<T> ConvertToIdentity<T>(string value)
    {
        if (typeof(T) == typeof(int?))
            return ConvertToNullableInt32(value).ToResult<T>();
        if (typeof(T) == typeof(Guid?))
            return ConvertToNullableGuid(value).ToResult<T>();
        if (typeof(T) == typeof(int))
            return ConvertToInt32(value).ToResult<T>();
        if (typeof(T) == typeof(Guid))
            return ConvertToGuid(value).ToResult<T>();
        if (typeof(T) == typeof(string))
            return ConvertToString(value).ToResult<T>();
        return IOpsResult.Invalid("unkown type for T").ToResult<T>();

    }

    public static IOpsResult<int> ConvertToInt32(string value)
    {
        int result;
        if( int.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<int>();
    }

    public static IOpsResult<Int64> ConvertToInt64(string value)
    {
        Int64 result;
        if(Int64.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<Int64>();
    }

    public static IOpsResult<uint> ConvertToUInt32(string value)
    {
        uint result;
        if (uint.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<uint>();
    }

    public static IOpsResult<ulong> ConvertToUInt64(string value)
    {
        ulong result;
        if (ulong.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<ulong>();
    }

    public static IOpsResult<ulong?> ConvertToNullableUInt64(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        return ConvertToUInt64(value).ToResult<UInt64?>();
    }

    public static IOpsResult<double> ConvertToDouble(string value)
    {
        double result;
        if (double.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<double>();
    }

    public static IOpsResult<decimal> ConvertToDecimal(string value)
    {
        decimal result;
        if (decimal.TryParse(value, CultureInfo.InvariantCulture, out result))
        {
            return IOpsResult.Ok(result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<decimal>();
    }

    public static IOpsResult<string> ConvertToString(string value)
    {
        return IOpsResult.Ok(value);
    }

    public static IOpsResult<T> ConvertToEnum<T>(string value)
    {
        //return (T)Enum.Parse(typeof(T), value);
        object? result;
        if (Enum.TryParse(typeof(T), value, true, out result))
        {
            return IOpsResult.Ok((T)result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<T>();
    }

    public static IOpsResult<byte[]> ConvertToArrayOfByte(string value)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return IOpsResult.Ok(encoding.GetBytes(value));
    }

    public static IOpsResult<T> ConvertToNullableEnum<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return IOpsResult.Ok(default(T));
        object? result;
        if (Enum.TryParse(typeof(T), value, true, out result))
        {
            return IOpsResult.Ok((T)result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<T>();
    }

    public static IOpsResult<MemoryStream> ConvertToMemoryStream(string value)
    {
        if (value == null)
            return null;
        return null;
    }

    public static IOpsResult<T1> GetEnumData<T1, T2, T3>(T3 value)
    {
        var enumType = typeof(T2);
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value == null)
            return IOpsResult.Ok(default(T1));

        object? result;
        if (Enum.TryParse(typeof(T1), value.ToString(), true, out result))
        {
            return IOpsResult.Ok((T1)result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<T1>();

    }

    public static IOpsResult<T1> GetEnumData<T1, T2>(object value)
    {
        var enumType = typeof(T2);
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value == null)
            return IOpsResult.Ok(default(T1));

        object? result;
        if (Enum.TryParse(typeof(T1), value.ToString(), true, out result))
        {
            return IOpsResult.Ok((T1)result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<T1>();
    }

    public static IOpsResult<T1> GetEnumData<T1, T2>(string value)
    {
        var enumType = typeof(T2);
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            enumType = Nullable.GetUnderlyingType(enumType);
        }
        if (value == null)
            return IOpsResult.Ok(default(T1));

        object? result;
        if (Enum.TryParse(typeof(T1), value.ToString(), true, out result))
        {
            return IOpsResult.Ok((T1)result);
        }
        return IOpsResult.Invalid($"input {value} is invalid").ToResult<T1>();
    }

    public static IOpsResult<List<T1>> GetEnumDataList<T1, T2, T3>(List<T3> values)
    {
        if (values == null)
            return null;
        List<T1> result = new List<T1>();
        foreach (var value in (IEnumerable)values)
        {
            var res = GetEnumData<T1, T2>(value);
            if (!res.HasError)
            {
                result.Add(res.Data);
            }
        }
        return IOpsResult.Ok(result);
    }
    public static IOpsResult<List<T1>> GetEnumDataList<T1, T2>(IEnumerable values)
    {
        List<T1> result = new List<T1>();
        foreach (var value in values)
        {
            var res = GetEnumData<T1, T2>(value);

            if (!res.HasError)
            {
                result.Add(res.Data);
            }
        }
        return IOpsResult.Ok(result);
    }
}
