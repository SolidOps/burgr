#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _MODULE_NAMESPACE_.Domain;

namespace _MODULE_NAMESPACE_.Application;

#region foreach MODEL[AG][R]
public partial class CLASSNAMEService(Domain.ICLASSNAMERepository repository, IUnitOfWork unitOfWork)
{
    #region to remove if NOT_GETBYQUERY
    public async Task<Result<Page<CLASSNAMEDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, CLASSNAMEDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }

        var page = await repository.QueryAsync(request, cancellationToken);
        return page.Map(CLASSNAMEDto.From);
    }
    #endregion to remove if NOT_GETBYQUERY

    #region to remove if NOT_GETBYID
    public async Task<Result<CLASSNAMEDto>> GetAsync(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default)
    {
        var aggregate = await repository.FindAsync(id, cancellationToken);
        return aggregate is null ? Error.NotFound() : CLASSNAMEDto.From(aggregate);
    }
    #endregion to remove if NOT_GETBYID

    #region to remove if NOT_ADD
    public async Task<Result<CLASSNAMEDto>> AddAsync(CLASSNAMEInput input, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateAsync(input, cancellationToken);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }

        var aggregate = new Domain.CLASSNAME();
        input.ApplyTo(aggregate);
        repository.Add(aggregate);
        if (await unitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        return CLASSNAMEDto.From(aggregate);
    }
    #endregion to remove if NOT_ADD

    #region to remove if NOT_UPDATE
    public async Task<Result<CLASSNAMEDto>> UpdateAsync(_IDENTITY_KEY_TYPE_ id, CLASSNAMEInput input, CancellationToken cancellationToken = default)
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
        return CLASSNAMEDto.From(aggregate);
    }
    #endregion to remove if NOT_UPDATE

    #region to remove if NOT_REMOVE
    public async Task<Result> RemoveAsync(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default)
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
    #endregion to remove if NOT_REMOVE

    // the input's own rules, then the existence of the aggregates and entities it references
    private async Task<Dictionary<string, string[]>> ValidateAsync(CLASSNAMEInput input, CancellationToken cancellationToken)
    {
        var errors = input.Validate();
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO][NN]
        if (!await unitOfWork.ExistsAsync<Domain._PROPERTYTYPE_>(input._SIMPLE__PROPERTYNAME_, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO][N]
        if (input._SIMPLE__PROPERTYNAME_.HasValue && !await unitOfWork.ExistsAsync<Domain._PROPERTYTYPE_>(input._SIMPLE__PROPERTYNAME_.Value, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        return errors;
    }
}

#endregion foreach MODEL
