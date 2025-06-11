using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Shared.Application.Events;
using SolidOps.UM.Shared.Contracts.Results;
namespace SolidOps.UM.Application.EventHandlers;
// Object [EN][AG]
public abstract partial class BaseLocalUserEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<ILocalUserRepository> logger;
    protected Domain.Repositories.ILocalUserRepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules;

    public BaseLocalUserEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , ILocalUserRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<ILocalUserRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;

    }
}
public partial class LocalUserEventHandler : BaseLocalUserEventHandler
{
    public LocalUserEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , ILocalUserRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules

            )
    {
    }
}
public abstract partial class BaseUserEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IUserRepository> logger;
    protected Domain.Repositories.IUserRepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules;
    // Dependency [EN][AG]
    protected readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public BaseUserEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IUserRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository // for base

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IUserRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

    }
}
public partial class UserEventHandler : BaseUserEventHandler
{
    public UserEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IUserRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.User>> entityRules
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules
    // Dependency [EN][AG]
        , dependencyLocalUserRepository

            )
    {
    }
}
public abstract partial class BaseUserRightEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IUserRightRepository> logger;
    protected Domain.Repositories.IUserRightRepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules;

    public BaseUserRightEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IUserRightRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IUserRightRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;

    }
}
public partial class UserRightEventHandler : BaseUserRightEventHandler
{
    public UserRightEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IUserRightRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.Entities.UserRight>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules

            )
    {
    }
}
public abstract partial class BaseRightEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IRightRepository> logger;
    protected Domain.Repositories.IRightRepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules;

    public BaseRightEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IRightRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IRightRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;

    }
}
public partial class RightEventHandler : BaseRightEventHandler
{
    public RightEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IRightRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.Entities.Right>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules

            )
    {
    }
}
public abstract partial class BaseInviteEventHandler
{
    protected readonly IExecutionContext executionContext;
    protected readonly ILogger<IInviteRepository> logger;
    protected Domain.Repositories.IInviteRepository repository;
    protected readonly IServiceProvider serviceProvider;
    protected readonly IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules;

    public BaseInviteEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IInviteRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules

        )
    {
        this.executionContext = executionContext;
        this.logger = loggerFactory.CreateLogger<IInviteRepository>();
        this.serviceProvider = serviceProvider;
        this.entityRules = entityRules.OrderBy(s => s.Priority).ToList();
        this.repository = repository;

    }
}
public partial class InviteEventHandler : BaseInviteEventHandler
{
    public InviteEventHandler(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider
        , IInviteRepository repository
        , IEnumerable<IEntityRules<Guid, Domain.AggregateRoots.Invite>> entityRules

        ) : base(executionContext, loggerFactory, serviceProvider, repository, entityRules

            )
    {
    }
}