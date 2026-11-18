#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolidOps.UM.Domain;
namespace SolidOps.UM.Application;
// Object [AG][R]
public partial class UserService(Domain.IUserRepository repository, IUnitOfWork unitOfWork)
{

    public async Task<Result<Page<UserDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, UserDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var page = await repository.QueryAsync(request, cancellationToken);
        return page.Map(UserDto.From);
    }

    public async Task<Result<UserDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindAsync(id, cancellationToken);
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
        repository.Add(aggregate);
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        return UserDto.From(aggregate);
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UserInput input, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindAsync(id, cancellationToken);
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
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        return UserDto.From(aggregate);
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindAsync(id, cancellationToken);
        if (aggregate is null)
        {
            return Error.NotFound();
        }
        repository.Remove(aggregate);
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        return Result.Success();
    }

    // the input's own rules, then the existence of the aggregates and entities it references
    private async Task<Dictionary<string, string[]>> ValidateAsync(UserInput input, CancellationToken cancellationToken)
    {
        var errors = input.Validate();

        return errors;
    }
}
public partial class InviteService(Domain.IInviteRepository repository, IUnitOfWork unitOfWork)
{

    public async Task<Result<Page<InviteDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, InviteDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }
        var page = await repository.QueryAsync(request, cancellationToken);
        return page.Map(InviteDto.From);
    }

    public async Task<Result<InviteDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindAsync(id, cancellationToken);
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
        repository.Add(aggregate);
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
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
        if (!await unitOfWork.ExistsAsync<Domain.User>(input.CreatorId, cancellationToken))
        {
            errors["CreatorId"] = ["User not found"];
        }

        return errors;
    }
}