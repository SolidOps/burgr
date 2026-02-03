using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Contracts.Services;
using SolidOps.TODO.Shared;

namespace MetaCorp.Template.Application.Services;

#region foreach DOMAIN_SERVICE
public partial class SERVICENAMEService : BaseSERVICENAMEService, ISERVICENAMEService
{
    private readonly IUserContext userContext;
    #region foreach DEPENDENCY[EN][AG]
    private readonly DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository _dependencyDEPENDENCYTYPERepository;
    #endregion foreach DEPENDENCY

    public SERVICENAMEService(IUserContext userContext, IServiceProvider serviceProvider
    #region foreach DEPENDENCY[EN][AG]
        , DEPENDENCYNAMESPACE.Domain.Repositories.IDEPENDENCYTYPERepository dependencyDEPENDENCYTYPERepository
    #endregion foreach DEPENDENCY
        ) : base(serviceProvider)
    {
        this.userContext = userContext;
        #region foreach DEPENDENCY[EN][AG]
        _dependencyDEPENDENCYTYPERepository = dependencyDEPENDENCYTYPERepository ?? throw new ArgumentNullException(nameof(dependencyDEPENDENCYTYPERepository));
        #endregion foreach DEPENDENCY
    }

    #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
    public virtual async Task<IOpsResult> _DOVOIDACTION_(/*PARAMETER_DEFINITION*/)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var result = await Internal_DOVOIDACTION_(/*PARAMETERS*/);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN
    public virtual async Task<IOpsResult<_IDENTITY_KEY_TYPE_>> _DOIDENTITYACTION_(/*PARAMETER_DEFINITION*/)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var result = await Internal_DOIDENTITYACTION_(/*PARAMETERS*/);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
    public virtual async Task<IOpsResult<_SIMPLE__TYPE_>> _DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var result = await Internal_DOSIMPLEACTION_(/*PARAMETERS*/);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN
    public virtual async Task<IOpsResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> _DOMODELACTION_(/*PARAMETER_DEFINITION*/)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var result = await Internal_DOMODELACTION_(/*PARAMETERS*/);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    public virtual async Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var result = await Internal_DOMODELLISTACTION_(/*PARAMETERS*/);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
}

#endregion foreach DOMAIN_SERVICE

#region foreach DOMAIN_SERVICE
public abstract class BaseSERVICENAMEService
{
    protected readonly IServiceProvider serviceProvider;

    public BaseSERVICENAMEService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
    public virtual Task<IOpsResult> Internal_DOVOIDACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOVOIDACTION_");
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN
    public virtual Task<IOpsResult<_IDENTITY_KEY_TYPE_>> Internal_DOIDENTITYACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOIDENTITYACTION_");
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
    public virtual Task<IOpsResult<_SIMPLE__TYPE_>> Internal_DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOSIMPLEACTION_");
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN
    public virtual Task<IOpsResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> Internal_DOMODELACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOMODELACTION_");
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    public virtual Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>> Internal_DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/)
    {
        throw new NotImplementedException("_DOMODELLISTACTION_");
    }
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
}
#endregion foreach DOMAIN_SERVICE