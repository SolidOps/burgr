#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _MODULE_NAMESPACE_.Application;

#region foreach DOMAIN_SERVICE
public partial interface ISERVICENAMEService
{
    #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
    Task<Result> _DOVOIDACTION_Async(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN
    Task<Result<_IDENTITY_KEY_TYPE_>> _DOIDENTITYACTION_Async(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
    Task<Result<_SIMPLE__TYPE_>> _DOSIMPLEACTION_Async(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN
    Task<Result<_PROPERTYTYPE_Dto/*_ISNULL_*/>> _DOMODELACTION_Async(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    Task<Result<List<_PROPERTYTYPE_Dto>>> _DOMODELLISTACTION_Async(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
}

// the methods of ISERVICENAMEService are written by hand in another part of this class
public partial class SERVICENAMEService(
    #region foreach DEPENDENCY[AG]
    Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository,
    #endregion foreach DEPENDENCY
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IServiceProvider serviceProvider) : ISERVICENAMEService
{
    #region foreach DEPENDENCY[AG]
    protected Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository { get; } = DEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;

    protected ICurrentUser CurrentUser { get; } = currentUser;

    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}

#endregion foreach DOMAIN_SERVICE
