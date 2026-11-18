using SolidOps.UM.Domain;

namespace SolidOps.UM.Application;

public static class Paging
{
    /// <summary>The errors of a page request, by query parameter; empty when valid.</summary>
    public static Dictionary<string, string[]> Validate(PageRequest request, IReadOnlyCollection<string> sortableProperties)
    {
        var errors = new Dictionary<string, string[]>();
        if (request.Page < 1)
        {
            errors["page"] = ["page starts at 1"];
        }
        if (request.PageSize < 1 || request.PageSize > PageRequest.MaximumPageSize)
        {
            errors["pageSize"] = [$"pageSize is between 1 and {PageRequest.MaximumPageSize}"];
        }
        if (request.OrderBy is not null && !sortableProperties.Contains(request.OrderBy))
        {
            errors["orderBy"] = [$"orderBy is one of: {string.Join(", ", sortableProperties)}"];
        }
        return errors;
    }
}
