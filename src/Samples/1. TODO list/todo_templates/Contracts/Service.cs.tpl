using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;

namespace MetaCorp.Template.Contracts.Services;

#region foreach DOMAIN_SERVICE
public partial interface ISERVICENAMEService
{
    #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN
    Task<IOpsResult> _DOVOIDACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN
    Task<IOpsResult<_IDENTITY_KEY_TYPE_>> _DOIDENTITYACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN
    Task<IOpsResult<_SIMPLE__TYPE_>> _DOSIMPLEACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN
    Task<IOpsResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> _DOMODELACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
}
#endregion foreach DOMAIN_SERVICE