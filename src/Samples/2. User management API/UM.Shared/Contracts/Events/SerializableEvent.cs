using SolidOps.SubZero;
using System.Reflection;

namespace SolidOps.UM.Shared.Contracts.Events;

public abstract class SerializableEvent : ISerializableEvent
{
    protected SerializableEvent(bool isInternal)
    {
        var type = this.GetType();
        EventAssemblyName = type.Assembly.FullName;
        EventFullType = type.FullName;
        IsInternal = isInternal;
        EventContext = string.Empty;
    }

    protected SerializableEvent(string eventContext, bool isInternal) : this(isInternal)
    {
        EventContext = eventContext;
    }

    public string EventContext { get; set; }
    public string Content { get; set; }
    public string EventAssemblyName { get; set; }
    public string EventFullType { get; set; }
    private Type _eventType { get; set; }

    public Type GetEventType()
    {
        if (_eventType == null)
        {
            var assembly = Assembly.Load(EventAssemblyName);
            _eventType = assembly.GetType(EventFullType);
        }
        return _eventType;
    }
    public bool IsBroadcast { get; set; }

    public bool IsInternal { get; private set; }

    public virtual Type GetDataType()
    {
        return null;
    }

    public virtual Type GetIdentityType()
    {
        return null;
    }
}

public abstract class SerializableEventOf<T> : SerializableEvent, ISerializableEventOf<T>
    where T : class
{
    private T data = null;
    public T Data 
    {
        get
        {
            if(data == null && Content != null)
                data = Serializer.Deserialize<T>(Content, true);
            return data;
        }
        set
        {
            data = value;
            Content = Serializer.Serialize(Data, true);
        }
    }

    public virtual string DataId
    {
        get
        {
            return string.Empty;
        }
    }
    public string DataAssemblyName { get; set; }
    public string DataFullType { get; set; }

    protected SerializableEventOf(string eventContext, bool isInternal): base(eventContext, isInternal)
    {
        DataAssemblyName = typeof(T).Assembly.FullName;
        DataFullType = typeof(T).FullName;
    }

    protected SerializableEventOf(string eventContext, T data, bool isInternal) : this(eventContext, isInternal)
    {
        Data = data;
    }

    protected SerializableEventOf(string eventContext, string content, bool isInternal) : this(eventContext, isInternal)
    {
        Content = content;
    }
    private Type _dataType { get; set; }
    public override Type GetDataType()
    {
        if (_dataType == null)
        {
            var assembly = Assembly.Load(DataAssemblyName);
            _dataType = assembly.GetType(DataFullType);
        }
        return _dataType;
    }
}
