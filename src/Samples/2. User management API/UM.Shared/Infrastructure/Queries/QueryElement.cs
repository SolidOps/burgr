namespace SolidOps.UM.Shared.Infrastructure.Queries;

public class AnyQueryElement : BaseQueryElement
{
    public List<BaseQueryElement> Elements { get; private set; }

    public AnyQueryElement()
    {
        Elements = new List<BaseQueryElement>();
    }

    public override object Clone()
    {
        AnyQueryElement clone = new AnyQueryElement();
        foreach (var c in Elements)
            clone.Elements.Add((BaseQueryElement)c.Clone());
        return clone;
    }
}

public class AllQueryElement : BaseQueryElement
{
    public List<BaseQueryElement> Elements { get; private set; }

    public AllQueryElement()
    {
        Elements = new List<BaseQueryElement>();
    }

    public override object Clone()
    {
        AllQueryElement clone = new AllQueryElement();
        foreach (var c in Elements)
            clone.Elements.Add((BaseQueryElement)c.Clone());
        return clone;
    }
}

public abstract class BaseQueryElement : ICloneable
{
    public abstract object Clone();
}

public class SingleQueryElement : BaseQueryElement
{
    public string Member { get; set; }
    public object Value { get; private set; }
    public CriteriaOperation CriteriaOperation { get; set; }

    public SingleQueryElement(string member, CriteriaOperation operation, object value)
    {
        Member = member;
        CriteriaOperation = operation;
        Value = value;
    }

    public override object Clone()
    {
        SingleQueryElement clone = new SingleQueryElement(Member, CriteriaOperation, Value);
        return clone;
    }
}
