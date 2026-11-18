#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidOps.UM.Domain;
namespace SolidOps.UM.Application;
// Object [AG][R]
public partial class UserService(
    Domain.IUserRepository repository,
    // Dependency [AG]
    Domain.ILocalUserRepository LocalUserRepository,

    IUnitOfWork unitOfWork,
    IEventPublisher events,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider)
{
    protected Domain.IUserRepository Repository { get; } = repository;
    // Dependency [AG]
    protected Domain.ILocalUserRepository LocalUserRepository { get; } = LocalUserRepository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected IEventPublisher Events { get; } = events;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services the rules need besides the repositories of the model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;

    public async Task<Result<Page<UserDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, UserDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var page = await Repository.QueryAsync(request, cancellationToken);
        return page.Map(UserDto.From);
    }

    public async Task<Result<UserDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
        return aggregate is null ? Error.NotFound() : UserDto.From(aggregate);
    }

    public async Task<Result<UserDto>> AddAsync(UserInput input, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(input, cancellationToken);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var aggregate = new Domain.User();
        input.ApplyTo(aggregate);
        // Rule [BA]
        if (await CreateLocalUserAsync(aggregate, cancellationToken) is { } CreateLocalUserError)
        {
            return CreateLocalUserError;
        }

        Repository.Add(aggregate);
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }

        return UserDto.From(aggregate);
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UserInput input, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
        if (aggregate is null)
        {
            return Error.NotFound();
        }
        var errors = await ValidateAsync(input, cancellationToken);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        input.ApplyTo(aggregate);

        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }

        return UserDto.From(aggregate);
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
        if (aggregate is null)
        {
            return Error.NotFound();
        }

        Repository.Remove(aggregate);
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }

        await Events.PublishAsync(new UserEvent(EventContext.Remove, UserDto.From(aggregate)), cancellationToken);

        return Result.Success();
    }

    // Rule 
    /// <summary>The rule CreateLocalUser of the model, written by hand: null to go on, or the error that stops the use case.</summary>
    private partial Task<Error?> CreateLocalUserAsync(Domain.User aggregate, CancellationToken cancellationToken);

    // the input's own rules, then the existence of the aggregates and entities it references
    private async Task<Dictionary<string, string[]>> ValidateAsync(UserInput input, CancellationToken cancellationToken)
    {
        var errors = input.Validate();

        return errors;
    }
}
public partial class InviteService(
    Domain.IInviteRepository repository,

    IUnitOfWork unitOfWork,
    IEventPublisher events,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider)
{
    protected Domain.IInviteRepository Repository { get; } = repository;

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
    protected IEventPublisher Events { get; } = events;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    /// <summary>The services the rules need besides the repositories of the model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;

    public async Task<Result<Page<InviteDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, InviteDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var page = await Repository.QueryAsync(request, cancellationToken);
        return page.Map(InviteDto.From);
    }

    public async Task<Result<InviteDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
        return aggregate is null ? Error.NotFound() : InviteDto.From(aggregate);
    }

    public async Task<Result<InviteDto>> AddAsync(InviteInput input, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(input, cancellationToken);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var aggregate = new Domain.Invite();
        input.ApplyTo(aggregate);

        Repository.Add(aggregate);
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }

        return InviteDto.From(aggregate);
    }

    // the input's own rules, then the existence of the aggregates and entities it references
    private async Task<Dictionary<string, string[]>> ValidateAsync(InviteInput input, CancellationToken cancellationToken)
    {
        var errors = input.Validate();
        // Property [M][NO][NAR][AG][EN][PUO][NRO][NN]
        if (!await UnitOfWork.ExistsAsync<Domain.User>(input.CreatorId, cancellationToken))
        {
            errors["CreatorId"] = ["User not found"];
        }

        return errors;
    }
}