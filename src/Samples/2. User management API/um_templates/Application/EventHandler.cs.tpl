using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using MetaCorp.Template.Domain.Repositories;
using SolidOps.UM.Shared.Application.Events;
using SolidOps.UM.Shared.Contracts.Results;
namespace MetaCorp.Template.Application.EventHandlers;

#region foreach MODEL[EN][AG]
public abstract partial class BaseCLASSNAMEEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<ICLASSNAMERepository> logger;
    protected Domain.Repositories.ICLASSNAMERepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules;

    #region foreach DEPENDENCY[EN][AG]
    protected readonly DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository _dependencyDEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    public BaseCLASSNAMEEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , ICLASSNAMERepository repository
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository // for base
    #endregion foreach DEPENDENCY
        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<ICLASSNAMERepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;

        #region foreach DEPENDENCY[EN][AG]
        _dependencyDEPENDENCYTYPERepository = dependencyDEPENDENCYTYPERepository ?? throw new ArgumentNullException(nameof(dependencyDEPENDENCYTYPERepository));
        #endregion foreach DEPENDENCY
    }
}

#region foreach CONSUMEDEVENT
public abstract partial class BaseCLASSNAMEEventHandler : IEventHandler<CONSUMEDEVENTTYPE>
{
    public async Task<IOpsResult> Handle(BusMessage<CONSUMEDEVENTTYPE> message)
    {
        return await this.OnEvent(message.Event);
    }

    protected virtual async Task<IOpsResult> OnEvent(CONSUMEDEVENTTYPE @event)
    {
        await Task.CompletedTask;
        return IOpsResult.Ok();
    }
}
#endregion foreach CONSUMEDEVENT

public partial class CLASSNAMEEventHandler : BaseCLASSNAMEEventHandler
{
    public CLASSNAMEEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , ICLASSNAMERepository repository
        , IEnumerable<IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>> entityRules
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules
    #region foreach DEPENDENCY[EN][AG]
        , dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
            )
    {
    }
}
#endregion foreach MODEL