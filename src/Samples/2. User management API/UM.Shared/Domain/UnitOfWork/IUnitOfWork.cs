using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Events;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    string Name { get; set; }

    UnitOfWorkType UnitOfWorkType { get; set; }

    IUnitOfWork ParentUnitOfWork { get; set; }

    void Complete();

    List<RequestStat> Queries { get; }

    IDataAccessFactory DataAccessFactory { get; }

    void AddEvents(List<ISerializableEvent> events);
    void AddEvent(ISerializableEvent @event);

    bool IsTransactionnal { get; set; }
    IExecutionContext ExecutionScope { get; }

    Dictionary<string, object> Properties { get; }
}

public enum UnitOfWorkType
{
    Read = 0,
    Write = 1,
    QueryUpdate = 2
}
