namespace SolidOps.UM.Shared.Domain.Entities;

public abstract class BaseDomainEntity<T> : BaseDomainValueObject, IDomainEntity<T>
    where T : struct
{
    public T Id { get; private set; }

    public bool LazyLoadingEnabled { get; set; }

    protected BaseDomainEntity()
    {   
    }

    public string GetPublicId()
    {
        return this.Id.ToString();
    }

    public string ComposedId
    {
        get
        {
            return Id.ToString();
        }
    }

    public void SetId(string id)
    {
        if(id.Contains("|"))
        {
            var list = new List<T>();
            foreach (var item in id.Split('|'))
                list.Add(IdentityKeyHelper<T>.ReadString(item));
            SetId(IdentityKeyHelper<T>.DefaultValue(), list);
        }
        else
        {
            SetId(IdentityKeyHelper<T>.ReadString(id));
        }
    }

    public void SetId(T id, List<T> ids = null)
    {
        this.Id = id;
    }
}
