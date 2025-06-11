namespace SolidOps.UM.Shared.Contracts.DTO;

public interface IQueryFilterDTO
{
    string Filter { get; set; }
    List<OrderByClause> OrderBy { get; set; }
    int? MaxResults { get; }
    string ContinuationId { get; }

}
