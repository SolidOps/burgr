using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Infrastructure;

public interface IEventPublisherService
{
    Task<IOpsResult> PublishEvents(IEnumerable<ISerializableEvent> events, IUnitOfWork unitOfWork);
}
