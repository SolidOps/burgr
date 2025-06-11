using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace MetaCorp.Template.Application.UseCases;

#region foreach DOMAIN_USECASE[I]
public partial interface IUSECASENAMEUseCase : IDomainUseCase
{
    #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
    Task<IOpsResult> _DOVOIDACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN

    #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    Task<IOpsResult<_IDENTITY_KEY_TYPE_>> _DOIDENTITYACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN

    #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    Task<IOpsResult<_SIMPLE__TYPE_>> _DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN

    #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    Task<IOpsResult<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_>> _DOMODELACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

    #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_>>> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
}

public partial class USECASENAMEUseCase : BaseUSECASENAMEUseCase, IUSECASENAMEUseCase
{
    private readonly IExecutionContext executionContext;
    #region foreach DEPENDENCY[EN][AG]
    private readonly DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository _dependencyDEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    public USECASENAMEUseCase(IExecutionContext executionContext, IServiceProvider serviceProvider, ILoggerFactory loggerFactory
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
        ) : base(serviceProvider, loggerFactory)
    {
        this.executionContext = executionContext;
        #region foreach DEPENDENCY[EN][AG]
        _dependencyDEPENDENCYTYPERepository = dependencyDEPENDENCYTYPERepository ?? throw new ArgumentNullException(nameof(dependencyDEPENDENCYTYPERepository));
        #endregion foreach DEPENDENCY
    }
}

#endregion foreach DOMAIN_USECASE

#region foreach DOMAIN_USECASE[I]
public abstract class BaseUSECASENAMEUseCase
{
    protected readonly ILogger<IUSECASENAMEUseCase> logger;
    protected readonly IServiceProvider serviceProvider;

    public BaseUSECASENAMEUseCase(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        this.serviceProvider = serviceProvider;
        this.logger = loggerFactory.CreateLogger<IUSECASENAMEUseCase>();
    }

    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
    public virtual Task<IOpsResult> _DOVOIDACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOVOIDACTION_");
    }
    #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN

    #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    public virtual Task<IOpsResult<_IDENTITY_KEY_TYPE_>> _DOIDENTITYACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOIDENTITYACTION_");
    }
    #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN

    #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    public virtual Task<IOpsResult<_SIMPLE__TYPE_>> _DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOSIMPLEACTION_");
    }
    #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN

    #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    public virtual Task<IOpsResult<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_>> _DOMODELACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOMODELACTION_");
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

    #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    public virtual Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_>>> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOMODELLISTACTION_");
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
}
#endregion foreach DOMAIN_USECASE