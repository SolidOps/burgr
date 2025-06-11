using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.Burgr.Shared.Application;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.Burgr.Shared.Domain.UnitOfWork;
namespace SolidOps.UM.Application.Services;
// Service [I]
public partial interface IAuthenticationService : IDomainService
{

    Task<IOpsResult> Login(SolidOps.UM.Domain.Transients.LoginRequest request);

    Task<IOpsResult> SetInitialPassword(System.String email, System.String password);

    Task<IOpsResult<System.Boolean>> NeedInitialPassword(System.String email);

}
public partial class AuthenticationService : BaseAuthenticationService, IAuthenticationService
{
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    public AuthenticationService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
        // Dependency [EN][AG]
        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

    }
}
public partial interface IConfigurationsService : IDomainService
{

    Task<IOpsResult<System.String>> GetConfiguration(System.String application, System.String environment);

}
public partial class ConfigurationsService : BaseConfigurationsService, IConfigurationsService
{
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IApplicationRepository _dependencyApplicationRepository;

    public ConfigurationsService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IApplicationRepository dependencyApplicationRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
        // Dependency [EN][AG]
        _dependencyApplicationRepository = dependencyApplicationRepository ?? throw new ArgumentNullException(nameof(dependencyApplicationRepository));

    }
}
public partial interface IInvitesService : IDomainService
{

    Task<IOpsResult> UseInvite(System.Guid inviteId, System.String password);

    Task<IOpsResult<SolidOps.UM.Domain.Transients.InviteResult>> CheckInvite(System.Guid inviteId);

}
public partial class InvitesService : BaseInvitesService, IInvitesService
{
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IInviteRepository _dependencyInviteRepository;

    public InvitesService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IInviteRepository dependencyInviteRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
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
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IRoleRepository _dependencyRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRoleRepository _dependencyUserRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserOrganizationRepository _dependencyUserOrganizationRepository;

    public SelfUserCreationService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        , SolidOps.UM.Domain.Repositories.IRoleRepository dependencyRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserRoleRepository dependencyUserRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserOrganizationRepository dependencyUserOrganizationRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
        // Dependency [EN][AG]
        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

        _dependencyRoleRepository = dependencyRoleRepository ?? throw new ArgumentNullException(nameof(dependencyRoleRepository));

        _dependencyUserRoleRepository = dependencyUserRoleRepository ?? throw new ArgumentNullException(nameof(dependencyUserRoleRepository));

        _dependencyUserOrganizationRepository = dependencyUserOrganizationRepository ?? throw new ArgumentNullException(nameof(dependencyUserOrganizationRepository));

    }
}
public partial interface IServerStatusService : IDomainService
{

    Task<IOpsResult<System.Boolean>> NeedTechUserPasswordUpdate(System.String techUser);

}
public partial class ServerStatusService : BaseServerStatusService, IServerStatusService
{
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IRoleRepository _dependencyRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRoleRepository _dependencyUserRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public ServerStatusService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        , SolidOps.UM.Domain.Repositories.IRoleRepository dependencyRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserRoleRepository dependencyUserRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

        _dependencyRoleRepository = dependencyRoleRepository ?? throw new ArgumentNullException(nameof(dependencyRoleRepository));

        _dependencyUserRoleRepository = dependencyUserRoleRepository ?? throw new ArgumentNullException(nameof(dependencyUserRoleRepository));

        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

    }
}
public partial interface ITokenValidationService : IDomainService
{

    Task<IOpsResult<System.String>> Validate();

}
public partial class TokenValidationService : BaseTokenValidationService, ITokenValidationService
{
    private readonly IExecutionScope executionScope;

    public TokenValidationService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;

    }
}
public partial interface IUserCreationService : IDomainService
{

    Task<IOpsResult<Guid>> CreateUser(SolidOps.UM.Domain.Transients.UserCreationInfo userCreationInfo);

}
public partial class UserCreationService : BaseUserCreationService, IUserCreationService
{
    private readonly IExecutionScope executionScope;
    // Dependency [EN][AG]
    private readonly SolidOps.UM.Domain.Repositories.ILocalUserRepository _dependencyLocalUserRepository;

    private readonly SolidOps.UM.Domain.Repositories.IRoleRepository _dependencyRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRoleRepository _dependencyUserRoleRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserOrganizationRepository _dependencyUserOrganizationRepository;

    private readonly SolidOps.UM.Domain.Repositories.IUserRepository _dependencyUserRepository;

    public UserCreationService(IExecutionScope executionScope, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    // Dependency [EN][AG]
        , SolidOps.UM.Domain.Repositories.ILocalUserRepository dependencyLocalUserRepository

        , SolidOps.UM.Domain.Repositories.IRoleRepository dependencyRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserRoleRepository dependencyUserRoleRepository

        , SolidOps.UM.Domain.Repositories.IUserOrganizationRepository dependencyUserOrganizationRepository

        , SolidOps.UM.Domain.Repositories.IUserRepository dependencyUserRepository

        ) : base(serviceProvider, loggerFactory)
    {
        this.executionScope = executionScope;
        // Dependency [EN][AG]
        _dependencyLocalUserRepository = dependencyLocalUserRepository ?? throw new ArgumentNullException(nameof(dependencyLocalUserRepository));

        _dependencyRoleRepository = dependencyRoleRepository ?? throw new ArgumentNullException(nameof(dependencyRoleRepository));

        _dependencyUserRoleRepository = dependencyUserRoleRepository ?? throw new ArgumentNullException(nameof(dependencyUserRoleRepository));

        _dependencyUserOrganizationRepository = dependencyUserOrganizationRepository ?? throw new ArgumentNullException(nameof(dependencyUserOrganizationRepository));

        _dependencyUserRepository = dependencyUserRepository ?? throw new ArgumentNullException(nameof(dependencyUserRepository));

    }
}
// Service [I]
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
public abstract class BaseConfigurationsService
{
    protected readonly ILogger<IConfigurationsService> logger;
    protected readonly IServiceProvider serviceProvider;
    public BaseConfigurationsService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IConfigurationsService>();
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<System.String>> GetConfiguration(System.String application, System.String environment)
    {
        throw new NotImplementedException("GetConfiguration");
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