using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Contracts.Results;

namespace SolidOps.UM.Shared.Application.Events;

public interface IEventHandler<T> where T : ISerializableEvent
{
    Task<IOpsResult> Handle(BusMessage<T> message);
}
