using Microsoft.EntityFrameworkCore.Diagnostics;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using System.Data.Common;

namespace SolidOps.UM.Shared.Infrastructure;

public class RequestInterceptor : DbCommandInterceptor
{
    private readonly IExecutionContext executionContext;

    public RequestInterceptor(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        CommandStarts(command);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var res = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        CommandEnds(command);
        return res;

    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        CommandStarts(command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
    {
        var res = base.NonQueryExecuted(command, eventData, result);
        CommandEnds(command);
        return res;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        CommandStarts(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        var res = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        CommandEnds(command);
        return res;
    }
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        CommandStarts(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        var res = base.ReaderExecuted(command, eventData, result);
        CommandEnds(command);
        return res;
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
    {
        CommandStarts(command);
        return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
    {
        var res = await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        CommandEnds(command);
        return res;
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
        CommandStarts(command);
        return base.ScalarExecuting(command, eventData, result);
    }

    public override object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
    {
        var res = base.ScalarExecuted(command, eventData, result);
        CommandEnds(command);
        return res;
    }

    public void CommandStarts(DbCommand command)
    {
        
    }

    public void CommandEnds(DbCommand command)
    {
        
    }
}