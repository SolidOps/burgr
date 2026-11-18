#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace SolidOps.UM.Application;
// Service 
public partial interface IAuthenticationService
{

    Task<Result> SetInitialPasswordAsync(SolidOps.UM.Application.LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<System.String>> LoginAsync(SolidOps.UM.Application.LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<System.Boolean>> NeedInitialPasswordAsync(System.String email, CancellationToken cancellationToken = default);

}
// the methods of IAuthenticationService are written by hand in another part of this class
public partial class AuthenticationService(
    // Dependency [AG]
    Domain.IUserRepository UserRepository,

    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : IAuthenticationService
{
    // Dependency [AG]
    protected Domain.IUserRepository UserRepository { get; } = UserRepository;

    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
public partial interface IInvitesService
{

    Task<Result> UseInviteAsync(SolidOps.UM.Application.InviteUseRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<System.Int32>> CountByStatusAsync(SolidOps.UM.Domain.InviteStatus status, CancellationToken cancellationToken = default);

    Task<Result<InviteResultDto>> CheckInviteAsync(System.Guid inviteId, CancellationToken cancellationToken = default);

}
// the methods of IInvitesService are written by hand in another part of this class
public partial class InvitesService(
    // Dependency [AG]
    Domain.IInviteRepository InviteRepository,

    Domain.IUserRepository UserRepository,

    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : IInvitesService
{
    // Dependency [AG]
    protected Domain.IInviteRepository InviteRepository { get; } = InviteRepository;

    protected Domain.IUserRepository UserRepository { get; } = UserRepository;

    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
public partial interface ISelfUserCreationService
{

    Task<Result<Guid>> CreateUserAsync(SolidOps.UM.Application.SelfUserCreationRequestDto request, CancellationToken cancellationToken = default);

}
// the methods of ISelfUserCreationService are written by hand in another part of this class
public partial class SelfUserCreationService(
    // Dependency [AG]
    Domain.IUserRepository UserRepository,

    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : ISelfUserCreationService
{
    // Dependency [AG]
    protected Domain.IUserRepository UserRepository { get; } = UserRepository;

    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
public partial interface IServerStatusService
{

    Task<Result<System.Boolean>> NeedTechUserPasswordUpdateAsync(System.String techUser, CancellationToken cancellationToken = default);

}
// the methods of IServerStatusService are written by hand in another part of this class
public partial class ServerStatusService(
    // Dependency [AG]
    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : IServerStatusService
{
    // Dependency [AG]
    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
public partial interface ITokenValidationService
{

    Task<Result<System.String>> ValidateAsync(CancellationToken cancellationToken = default);

}
// the methods of ITokenValidationService are written by hand in another part of this class
public partial class TokenValidationService(

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : ITokenValidationService
{

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
public partial interface IUserCreationService
{

    Task<Result<Guid>> CreateUserAsync(SolidOps.UM.Application.UserCreationInfoDto userCreationInfo, CancellationToken cancellationToken = default);

}
// the methods of IUserCreationService are written by hand in another part of this class
public partial class UserCreationService(
    // Dependency [AG]
    Domain.ILocalUserRepository LocalUserRepository,

    Domain.IUserRepository UserRepository,

    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : IUserCreationService
{
    // Dependency [AG]
    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected Domain.IUserRepository UserRepository { get; } = UserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}