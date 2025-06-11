namespace SolidOps.UM.Shared.Infrastructure;

public static class Pagination
{
    public static int GetSkip(string continuationId)
    {
        if (!string.IsNullOrEmpty(continuationId))
        {
            var parts = continuationId.Split('|');
            return int.Parse(parts[0]);
        }

        return 0;
    }

    public static string GetLastId(string continuationId)
    {
        if (!string.IsNullOrEmpty(continuationId))
        {
            var parts = continuationId.Split('|');
            return parts[1];
        }

        return "";
    }

    public static string CreateContinuationId(int skip, object id)
    {
        return string.Format("{0}|{1}", skip, id);
    }
}
