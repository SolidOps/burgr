namespace SolidOps.UM.Shared.Contracts.Events;

public interface ISerializableEvent
{
    string EventContext { get; }
    string Content { get; }
    string EventFullType { get; }
    string EventAssemblyName { get; }
    bool IsBroadcast { get; }
    bool IsInternal { get; }
    Type GetEventType();
    Type GetDataType();
    Type GetIdentityType();
}

public interface ISerializableEventOf<T> : ISerializableEvent
    where T : class
{
    T Data { get; set; }
    string DataId { get; }
    string DataFullType { get; }
    string DataAssemblyName { get; }
}
