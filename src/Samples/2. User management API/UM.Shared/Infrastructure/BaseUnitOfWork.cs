using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Events;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using System.Diagnostics;

namespace SolidOps.UM.Shared.Infrastructure;

public abstract class BaseUnitOfWork : IUnitOfWork
{
    // Context service
    public IExecutionContext ExecutionScope { get; private set; }
    private bool isComplete = false;
    public List<RequestStat> Queries { get; private set; }

    private List<ISerializableEvent> Events { get; set; }

    public BaseUnitOfWork(string name, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider)
    {
        this.ExecutionScope = serviceProvider.GetRequiredService<IExecutionContext>();
        Name = name;
        UnitOfWorkType = unitOfWorkType;
        Properties = new Dictionary<string, object>();
        Queries = new List<RequestStat>();
        Events = new List<ISerializableEvent>();
    }

    // Name
    public string Name { get; set; }

    public UnitOfWorkType UnitOfWorkType { get; set; }

    public Dictionary<string, object> Properties { get; set; }

    // Parent Scope
    public IUnitOfWork ParentUnitOfWork { get; set; }

    public IDataAccessFactory DataAccessFactory { get; set; }

    public bool IsTransactionnal { get; set; }

    public virtual void Complete()
    {
        isComplete = true;
    }

    public void AddEvents(List<ISerializableEvent> events)
    {
        if (events != null && events.Count > 0)
            this.Events.AddRange(events);
    }

    public void AddEvent(ISerializableEvent @event)
    {
        if (@event != null)
            this.Events.Add(@event);
    }

    public virtual void Dispose()
    {
        ExecutionScope.CurrentUnitOfWork = this.ParentUnitOfWork;
    }
}
