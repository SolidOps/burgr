using SolidOps.UM.Shared.Contracts.Events;

namespace SolidOps.UM.Shared.Application.Events;

public class BusMessage<T>
    where T : ISerializableEvent
{
    public BusMessage(T busEvent)
    {
        Event = busEvent;
    }

    public T Event { get; }
}
