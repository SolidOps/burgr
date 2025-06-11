
using SolidOps.UM.Shared.Contracts.DTO;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IQueryFilter
{
    string LiteralQuery { get; set; }
}

public class BaseQueryFilter : IQueryFilter
{
    public BaseQueryFilter(string filter, string orderBy, int? maxResults, string continuationId)
    {
        LiteralQuery = filter;
        MaxResults = maxResults;
        ContinuationId = continuationId;

        if (orderBy != null)
        {
            OrderBy = new List<OrderByClause>();
            foreach (var part in orderBy.Split("|"))
            {
                if (part.StartsWith('-'))
                {
                    OrderBy.Add(new OrderByClause(part.Substring(1), OrderByWay.Descending));
                }
                else
                {
                    OrderBy.Add(new OrderByClause(part, OrderByWay.Ascending));
                }
            }
        }
    }

    public int? MaxResults { get; set; }
    public string ContinuationId { get; set; }

    public List<OrderByClause> OrderBy { get; set; }

    public string LiteralQuery { get; set; }

}
