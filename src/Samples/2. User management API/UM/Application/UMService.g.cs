using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;
namespace SolidOps.UM.Application.Services;
// Service 
public partial interface IAuthenticationService : IDomainService
{

    Task<IOpsResult> Login(SolidOps.UM.Domain.Transients.LoginRequest request);

    Task<IOpsResult> SetInitialPassword(System.String email, System.String password);

    Task<IOpsResult<System.Boolean>> NeedInitialPassword(System.String email);

}
public partial class AuthenticationService : BaseAuthenticationService, IAuthenticationService
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public AuthenticationService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

    }
}
public partial interface IInvitesService : IDomainService
{

    Task<IOpsResult> UseInvite(System.Guid inviteId, System.String password);

    Task<IOpsResult<SolidOps.UM.Domain.Transients.InviteResult>> CheckInvite(System.Guid inviteId);

}
public partial class InvitesService : BaseInvitesService, IInvitesService
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IInviteRepository _dependencyInviteRepository;

    public InvitesService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IInviteRepository dependencyInviteRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyInviteRepository = dependencyInviteRepository ?? throw new ArgumentNullException(nameof(dependencyInviteRepository));

    }
}
public partial interface ISelfUserCreationService : IDomainService
{

    Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.SelfUserCreationRequest request);

}
public partial class SelfUserCreationService : BaseSelfUserCreationService, ISelfUserCreationService
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public SelfUserCreationService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

    }
}
public partial interface IServerStatusService : IDomainService
{

    Task<IOpsResult<System.Boolean>> NeedTechUserPasswordUpdate(System.String techUser);

}
public partial class ServerStatusService : BaseServerStatusService, IServerStatusService
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public ServerStatusService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

    }
}
public partial interface ITokenValidationService : IDomainService
{

    Task<IOpsResult<System.String>> Validate();

}
public partial class TokenValidationService : BaseTokenValidationService, ITokenValidationService
{
    private readonly IExecutionContext executionContext;

    public TokenValidationService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;

    }
}
public partial interface IUserCreationService : IDomainService
{

    Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.UserCreationInfo userCreationInfo);

}
public partial class UserCreationService : BaseUserCreationService, IUserCreationService
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public UserCreationService(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

    }
}
// Service 
public abstract class BaseAuthenticationService
{
    protected readonly ILogger<IAuthenticationService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseAuthenticationService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IAuthenticationService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult> Login(SolidOps.UM.Domain.Transients.LoginRequest request)
    {
        throw new NotImplementedException("Login");
    }

    public virtual Task<IOpsResult> SetInitialPassword(System.String email, System.String password)
    {
        throw new NotImplementedException("SetInitialPassword");
    }

    public virtual Task<IOpsResult<System.Boolean>> NeedInitialPassword(System.String email)
    {
        throw new NotImplementedException("NeedInitialPassword");
    }

}
public abstract class BaseInvitesService
{
    protected readonly ILogger<IInvitesService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseInvitesService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IInvitesService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult> UseInvite(System.Guid inviteId, System.String password)
    {
        throw new NotImplementedException("UseInvite");
    }

    public virtual Task<IOpsResult<SolidOps.UM.Domain.Transients.InviteResult>> CheckInvite(System.Guid inviteId)
    {
        throw new NotImplementedException("CheckInvite");
    }

}
public abstract class BaseSelfUserCreationService
{
    protected readonly ILogger<ISelfUserCreationService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseSelfUserCreationService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<ISelfUserCreationService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.SelfUserCreationRequest request)
    {
        throw new NotImplementedException("CreateUser");
    }

}
public abstract class BaseServerStatusService
{
    protected readonly ILogger<IServerStatusService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseServerStatusService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IServerStatusService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<System.Boolean>> NeedTechUserPasswordUpdate(System.String techUser)
    {
        throw new NotImplementedException("NeedTechUserPasswordUpdate");
    }

}
public abstract class BaseTokenValidationService
{
    protected readonly ILogger<ITokenValidationService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseTokenValidationService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<ITokenValidationService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<System.String>> Validate()
    {
        throw new NotImplementedException("Validate");
    }

}
public abstract class BaseUserCreationService
{
    protected readonly ILogger<IUserCreationService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseUserCreationService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IUserCreationService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.UserCreationInfo userCreationInfo)
    {
        throw new NotImplementedException("CreateUser");
    }

}