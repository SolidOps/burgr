using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.Events;
using System.Reflection;

namespace SolidOps.UM.Shared.Infrastructure;

public abstract class SerializableEntityEventOf<TEntity, T> : SerializableEventOf<TEntity>, IEntitySerializableEventOf<TEntity, T>
    where TEntity : class, IDomainEntity<T>
    where T : struct
{
    public override string DataId
    {
        get
        {
            return this.Data.Id.ToString();
        }
    }

    public string IdentityAssemblyName { get; set; }
    public string IdentityFullType { get; set; }

    protected SerializableEntityEventOf(string eventContext, TEntity data, bool isInternal) : base(eventContext, data, isInternal)
    {
        Data = data;
        IdentityAssemblyName = typeof(T).Assembly.FullName;
        IdentityFullType = typeof(T).FullName;
    }

    protected SerializableEntityEventOf(string eventContext, string content, bool isInternal) : base(eventContext, content, isInternal)
    {
        Content = content;
    }
    private Type _identityType { get; set; }
    public override Type GetIdentityType()
    {
        if (_identityType == null)
        {
            var assembly = Assembly.Load(IdentityAssemblyName);
            _identityType = assembly.GetType(IdentityFullType);
        }
        return _identityType;
    }
}
