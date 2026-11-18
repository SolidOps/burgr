#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _MODULE_NAMESPACE_.Domain;

namespace _MODULE_NAMESPACE_.Application;

#region foreach MODEL[AG][R]
public partial class CLASSNAMEService(
    Domain.ICLASSNAMERepository repository,
    #region foreach DEPENDENCY[AG]
    Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository,
    #endregion foreach DEPENDENCY
    IUnitOfWork unitOfWork,
    IEventPublisher events,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider)
{
    protected Domain.ICLASSNAMERepository Repository { get; } = repository;

    #region foreach DEPENDENCY[AG]
    protected Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository { get; } = DEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;

    protected IEventPublisher Events { get; } = events;

    protected ICurrentUser CurrentUser { get; } = currentUser;

    /// <summary>The services the rules need besides the repositories of the model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;

    #region to remove if NOT_GETBYQUERY
    public async Task<Result<Page<CLASSNAMEDto>>> QueryAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var errors = Paging.Validate(request, CLASSNAMEDto.SortableProperties);
        if (errors.Count > 0)
        {
            return Error.Invalid(errors);
        }

        var page = await Repository.QueryAsync(request, cancellationToken);
        return page.Map(CLASSNAMEDto.From);
    }
    #endregion to remove if NOT_GETBYQUERY

    #region to remove if NOT_GETBYID
    public async Task<Result<CLASSNAMEDto>> GetAsync(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
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
        #region foreach RULE[BA]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        Repository.Add(aggregate);
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        #region foreach RULE[AA]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        #region to remove if NO_ADD_EVENT
        await Events.PublishAsync(new CLASSNAMEEvent(EventContext.Add, CLASSNAMEDto.From(aggregate)), cancellationToken);
        #endregion to remove if NO_ADD_EVENT
        return CLASSNAMEDto.From(aggregate);
    }
    #endregion to remove if NOT_ADD

    #region to remove if NOT_UPDATE
    public async Task<Result<CLASSNAMEDto>> UpdateAsync(_IDENTITY_KEY_TYPE_ id, CLASSNAMEInput input, CancellationToken cancellationToken = default)
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
        #region foreach RULE[BU]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        #region foreach RULE[AU]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        #region to remove if NO_UPDATE_EVENT
        await Events.PublishAsync(new CLASSNAMEEvent(EventContext.Update, CLASSNAMEDto.From(aggregate)), cancellationToken);
        #endregion to remove if NO_UPDATE_EVENT
        return CLASSNAMEDto.From(aggregate);
    }
    #endregion to remove if NOT_UPDATE

    #region to remove if NOT_REMOVE
    public async Task<Result> RemoveAsync(_IDENTITY_KEY_TYPE_ id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.FindAsync(id, cancellationToken);
        if (aggregate is null)
        {
            return Error.NotFound();
        }

        #region foreach RULE[BR]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        Repository.Remove(aggregate);
        if (await UnitOfWork.SaveChangesAsync(cancellationToken) is { } error)
        {
            return error;
        }
        #region foreach RULE[AR]
        if (await _RULE_Async(aggregate, cancellationToken) is { } _RULE_Error)
        {
            return _RULE_Error;
        }
        #endregion foreach RULE
        #region to remove if NO_REMOVE_EVENT
        await Events.PublishAsync(new CLASSNAMEEvent(EventContext.Remove, CLASSNAMEDto.From(aggregate)), cancellationToken);
        #endregion to remove if NO_REMOVE_EVENT
        return Result.Success();
    }
    #endregion to remove if NOT_REMOVE

    #region foreach RULE
    /// <summary>The rule _RULE_ of the model, written by hand: null to go on, or the error that stops the use case.</summary>
    private partial Task<Error?> _RULE_Async(Domain.CLASSNAME aggregate, CancellationToken cancellationToken);
    #endregion foreach RULE

    // the input's own rules, then the existence of the aggregates and entities it references
    private async Task<Dictionary<string, string[]>> ValidateAsync(CLASSNAMEInput input, CancellationToken cancellationToken)
    {
        var errors = input.Validate();
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO][NN]
        if (!await UnitOfWork.ExistsAsync<Domain._PROPERTYTYPE_>(input._SIMPLE__PROPERTYNAME_, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO][N]
        if (input._SIMPLE__PROPERTYNAME_.HasValue && !await UnitOfWork.ExistsAsync<Domain._PROPERTYTYPE_>(input._SIMPLE__PROPERTYNAME_.Value, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        return errors;
    }
}

#endregion foreach MODEL
