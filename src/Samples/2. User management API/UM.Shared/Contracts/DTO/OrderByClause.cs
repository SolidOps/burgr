namespace SolidOps.UM.Shared.Contracts.DTO;

public enum OrderByWay
{
    Ascending,
    Descending
}

public class OrderByClause
{
    public OrderByClause(string member, OrderByWay way) : this(null, member, way) { }

    public OrderByClause(string alias, string member, OrderByWay way)
    {
        Alias = alias;
        Member = member;
        Way = way;
    }

    public string Alias { get; private set; }
    public string Field
    {
        get
        {
            if (Alias == null)
            {
                return Member;
            }
            return Alias + "." + Member;
        }
    }
    public string Member { get; private set; }
    public OrderByWay Way { get; private set; }
}
