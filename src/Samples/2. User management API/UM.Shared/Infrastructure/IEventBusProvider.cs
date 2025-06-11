using SolidOps.UM.Shared.Contracts.Events;

namespace SolidOps.UM.Shared.Infrastructure;

public interface IEventBusProvider
{
    void Notify<T>(T busEvent) where T : ISerializableEvent;

    void Notify(IEnumerable<ISerializableEvent> events);

    void NotifyNonGeneric(ISerializableEvent @event);
}
