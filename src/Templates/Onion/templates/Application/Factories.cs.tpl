#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _MODULE_NAMESPACE_.Application;

#region foreach MODEL[AG][EN]
/// <summary>The factories of CLASSNAME declared by the model; a new object is not saved by its factory.</summary>
public partial interface ICLASSNAMEFactory
{
    #region foreach FACTORY_METHOD
    Task<Result<Domain.CLASSNAME>> FACTORYNAMEAsync(/*PARAMETER_DEFINITION*//*COMMA*/CancellationToken cancellationToken = default);
    #endregion foreach FACTORY_METHOD
}

// the methods of ICLASSNAMEFactory are written by hand in another part of this class
public partial class CLASSNAMEFactory(
    #region foreach DEPENDENCY[AG]
    Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository,
    #endregion foreach DEPENDENCY
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider) : ICLASSNAMEFactory
{
    #region foreach DEPENDENCY[AG]
    protected Domain.IDEPENDENCYTYPERepository DEPENDENCYTYPERepository { get; } = DEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    protected IUnitOfWork UnitOfWork { get; } = unitOfWork;

    /// <summary>The services this class needs besides the repositories of its model dependencies.</summary>
    protected IServiceProvider Services { get; } = serviceProvider;
}

#endregion foreach MODEL
