#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace SolidOps.UM.Application;
// Object [AG][EN]
/// <summary>The factories of LocalUser declared by the model; a new object is not saved by its factory.</summary>
public partial interface ILocalUserFactory
{

}
// the methods of ILocalUserFactory are written by hand in another part of this class
public partial class LocalUserFactory(

    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : ILocalUserFactory
{

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
/// <summary>The factories of User declared by the model; a new object is not saved by its factory.</summary>
public partial interface IUserFactory
{

    Task<Result<Domain.User>> CreateLocalAsync(System.String email, CancellationToken cancellationToken = default);

}
// the methods of IUserFactory are written by hand in another part of this class
public partial class UserFactory(
    // Dependency [AG]
    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : IUserFactory
{
    // Dependency [AG]
    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
/// <summary>The factories of UserRight declared by the model; a new object is not saved by its factory.</summary>
public partial interface IUserRightFactory
{

}
// the methods of IUserRightFactory are written by hand in another part of this class
public partial class UserRightFactory(

    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : IUserRightFactory
{

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
/// <summary>The factories of Right declared by the model; a new object is not saved by its factory.</summary>
public partial interface IRightFactory
{

}
// the methods of IRightFactory are written by hand in another part of this class
public partial class RightFactory(

    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : IRightFactory
{

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}
/// <summary>The factories of Invite declared by the model; a new object is not saved by its factory.</summary>
public partial interface IInviteFactory
{

}
// the methods of IInviteFactory are written by hand in another part of this class
public partial class InviteFactory(

    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : IInviteFactory
{

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}