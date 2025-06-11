using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;

namespace MetaCorp.Template.Contracts.UseCases;

#region foreach DOMAIN_USECASE[I]
public partial interface IUSECASENAMEUseCase
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
    Task<IOpsResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> _DOMODELACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

    #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    Task<IOpsResult<IEnumerable<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>> _DOMODELLISTACTION_(/*PARAMETER_DEFINITION*/);
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
}
#endregion foreach DOMAIN_USECASE