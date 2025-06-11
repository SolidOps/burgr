using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;
namespace SolidOps.UM.Application.UseCases;
// UseCase [I]
public partial interface IAuthenticationUseCase : IDomainUseCase
{

    Task<IOpsResult> Login(SolidOps.UM.Domain.Transients.LoginRequest request);

    Task<IOpsResult> SetInitialPassword(System.String email, System.String password);

    Task<IOpsResult<System.Boolean>> NeedInitialPassword(System.String email);

}
public partial class AuthenticationUseCase : BaseAuthenticationUseCase, IAuthenticationUseCase
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public AuthenticationUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
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
public partial interface IInvitesUseCase : IDomainUseCase
{

    Task<IOpsResult> UseInvite(System.Guid inviteId, System.String password);

    Task<IOpsResult<SolidOps.UM.Domain.Transients.InviteResult>> CheckInvite(System.Guid inviteId);

}
public partial class InvitesUseCase : BaseInvitesUseCase, IInvitesUseCase
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IInviteRepository _dependencyInviteRepository;

    public InvitesUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IInviteRepository dependencyInviteRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        // Dependency [EN][AG]
        _dependencyInviteRepository = dependencyInviteRepository ?? throw new ArgumentNullException(nameof(dependencyInviteRepository));

    }
}
public partial interface ISelfUserCreationUseCase : IDomainUseCase
{

    Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.SelfUserCreationRequest request);

}
public partial class SelfUserCreationUseCase : BaseSelfUserCreationUseCase, ISelfUserCreationUseCase
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public SelfUserCreationUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
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
public partial interface IServerStatusUseCase : IDomainUseCase
{

    Task<IOpsResult<System.Boolean>> NeedTechUserPasswordUpdate(System.String techUser);

}
public partial class ServerStatusUseCase : BaseServerStatusUseCase, IServerStatusUseCase
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public ServerStatusUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
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
public partial interface ITokenValidationUseCase : IDomainUseCase
{

    Task<IOpsResult<System.String>> Validate();

}
public partial class TokenValidationUseCase : BaseTokenValidationUseCase, ITokenValidationUseCase
{
    private readonly IExecutionContext executionContext;

    public TokenValidationUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;

    }
}
public partial interface IUserCreationUseCase : IDomainUseCase
{

    Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.UserCreationInfo userCreationInfo);

}
public partial class UserCreationUseCase : BaseUserCreationUseCase, IUserCreationUseCase
{
    private readonly IExecutionContext executionContext;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public UserCreationUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
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
// UseCase [I]
public abstract class BaseAuthenticationUseCase
{
    protected readonly ILogger<IAuthenticationUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseAuthenticationUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IAuthenticationUseCase>();
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
public abstract class BaseInvitesUseCase
{
    protected readonly ILogger<IInvitesUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseInvitesUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IInvitesUseCase>();
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
public abstract class BaseSelfUserCreationUseCase
{
    protected readonly ILogger<ISelfUserCreationUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseSelfUserCreationUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<ISelfUserCreationUseCase>();
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
public abstract class BaseServerStatusUseCase
{
    protected readonly ILogger<IServerStatusUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseServerStatusUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IServerStatusUseCase>();
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
public abstract class BaseTokenValidationUseCase
{
    protected readonly ILogger<ITokenValidationUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseTokenValidationUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<ITokenValidationUseCase>();
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
public abstract class BaseUserCreationUseCase
{
    protected readonly ILogger<IUserCreationUseCase> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseUserCreationUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IUserCreationUseCase>();
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