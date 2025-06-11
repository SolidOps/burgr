namespace SolidOps.UM.Shared.Infrastructure.Queries;

public static class FilterHelper
{
    public const string JOKERCHAR = "*";

    public static bool MatchFilter<T>(T objectValue, string filterValue)
    {
        if (typeof(T) == typeof(int?))
            return MatchNullableInt32Filter((int?)(object)objectValue, filterValue);
        if (typeof(T) == typeof(Guid?))
            return MatchNullableGuidFilter((Guid?)(object)objectValue, filterValue);
        if (typeof(T) == typeof(int))
            return MatchInt32Filter((int)(object)objectValue, filterValue);
        if (typeof(T) == typeof(Guid))
            return MatchGuidFilter((Guid)(object)objectValue, filterValue);
        if (typeof(T) == typeof(string))
            return MatchStringFilter((string)(object)objectValue, filterValue);
        throw new Exception("Unknown type for T");
    }

    public static bool MatchNullableInt32Filter(int? objectValue, string filterValue)
    {
        return false;
    }

    public static bool MatchNullableGuidFilter(Guid? objectValue, string filterValue)
    {
        return false;
    }

    public static bool MatchInt32Filter(int objectValue, string filterValue)
    {
        return false;
    }

    public static bool MatchGuidFilter(Guid objectValue, string filterValue)
    {
        return false;
    }

    public static bool MatchStringFilter(string objectValue, string filterValue)
    {
        if(filterValue.StartsWith(JOKERCHAR))
        {
            if(filterValue.EndsWith(JOKERCHAR))
            {
                return objectValue.ToLower().Contains(filterValue.ToLower());
            }
            else
            {
                return objectValue.ToLower().StartsWith(filterValue.ToLower());
            }
        }
        else if (filterValue.EndsWith(JOKERCHAR))
        {
            return objectValue.ToLower().EndsWith(filterValue.ToLower());
        }

        return objectValue == filterValue;
    }
}
