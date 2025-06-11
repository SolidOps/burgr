using SolidOps.UM.Shared.Domain.UnitOfWork;
using System.Transactions;

namespace SolidOps.UM.Shared.Infrastructure;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public UnitOfWork(string name, UnitOfWorkType unitOfWorkType, IServiceProvider serviceProvider, bool withTransaction)
        : base(name, unitOfWorkType, serviceProvider)
    {
        TransactionOptions options = new TransactionOptions();
        options.IsolationLevel = IsolationLevel.ReadCommitted;
        IsTransactionnal = withTransaction;
        TransactionScope = new TransactionScope(withTransaction ? TransactionScopeOption.Required : TransactionScopeOption.Suppress, options, TransactionScopeAsyncFlowOption.Enabled);
    }

    public TransactionScope TransactionScope { get; set; }

    public override void Complete()
    {
        base.Complete();
        TransactionScope.Complete();
    }

    public override void Dispose()
    {
        TransactionScope.Dispose(); 
        base.Dispose();
    }
}
