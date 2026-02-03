using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetaCorp.Template.Application.Services;
using System.Diagnostics;
using SolidOps.UM.Shared.Presentation;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Contracts.Results;

namespace MetaCorp.Template.Presentation.Controllers;

#region foreach DOMAIN_SERVICE
[Route("SlugTemplate/SlugSERVICENAME/[action]")]
public partial class SERVICENAMEController : BaseController
{
    private readonly ISERVICENAMEService service;
    private readonly IOutputSerializer serializer;

    public SERVICENAMEController(ISERVICENAMEService service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN[EXT]
    /*[Http_VERB_(Name = "TemplateFacade_SERVICENAME__DOVOIDACTION_")]*/
    #region to remove if NOT_ANONYMOUS
    [AllowAnonymous]
    #endregion to remove if NOT_ANONYMOUS
    public async Task<IActionResult> _DOVOIDACTION_(/*PARAMETER_DEF_API*/)
    {
        #region to remove if ANONYMOUS
        executionContext.MandatoryRights.Add("MANDATORYRIGHT");
        executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
        #endregion to remove if ANONYMOUS
        IOpsResult result;
        using (var unitOfWork = executionContext.StartUnitOfWork("Template", "SERVICENAMEController _DOVOIDACTION_", UnitOfWorkType.UNITOFWORKTYPE))
        {
            result = await this.service._DOVOIDACTION_(/*CONVERTED_PARAMETERS*/);
            if (result.HasError) return Failure(result.Error);

            unitOfWork.Complete();
        }
        AddAdditionalHeadersFor_DOVOIDACTION_(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersFor_DOVOIDACTION_(Microsoft.AspNetCore.Http.HttpContext httpContext);
    #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN[EXT]
    /*[Http_VERB_(Name = "TemplateFacade_SERVICENAME__DOIDENTITYACTION_")]*/
    #region to remove if NOT_ANONYMOUS
    [AllowAnonymous]
    #endregion to remove if NOT_ANONYMOUS
    public async Task<IActionResult> _DOIDENTITYACTION_(/*PARAMETER_DEF_API*/)
    {
        #region to remove if ANONYMOUS
        executionContext.MandatoryRights.Add("MANDATORYRIGHT");
        executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
        #endregion to remove if ANONYMOUS
        IOpsResult<_IDENTITY_KEY_TYPE_> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("Template", "SERVICENAMEController _DOIDENTITYACTION_", UnitOfWorkType.UNITOFWORKTYPE))
        {
            result = await this.service._DOIDENTITYACTION_(/*CONVERTED_PARAMETERS*/);
            if (result.HasError) return Failure(result.Error);

            unitOfWork.Complete();
        }
        HttpContext.Response.Headers.Add("Location", new[] { $"{result.Data}" });
        AddAdditionalHeadersFor_DOIDENTITYACTION_(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersFor_DOIDENTITYACTION_(Microsoft.AspNetCore.Http.HttpContext httpContext);
    #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN[EXT]
    /*[Http_VERB_(Name = "TemplateFacade_SERVICENAME__DOSIMPLEACTION_")]*/
    #region to remove if NOT_ANONYMOUS
    [AllowAnonymous]
    #endregion to remove if NOT_ANONYMOUS
    public async Task<IActionResult> _DOSIMPLEACTION_(/*PARAMETER_DEF_API*/)
    {
        #region to remove if ANONYMOUS
        executionContext.MandatoryRights.Add("MANDATORYRIGHT");
        executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
        #endregion to remove if ANONYMOUS
        IOpsResult<_SIMPLE__TYPE_> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("Template", "SERVICENAMEController _DOSIMPLEACTION_", UnitOfWorkType.UNITOFWORKTYPE))
        {
            result = await this.service._DOSIMPLEACTION_(/*CONVERTED_PARAMETERS*/);
            if (result.HasError) return Failure(result.Error);

            unitOfWork.Complete();
        }
        AddAdditionalHeadersFor_DOSIMPLEACTION_(HttpContext);
        if (result.Data.GetType().IsArray)
            return Content(serializer.Serialize(result.Data), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        return Ok(result.Data);
    }
    partial void AddAdditionalHeadersFor_DOSIMPLEACTION_(Microsoft.AspNetCore.Http.HttpContext httpContext);
    #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN[EXT]
    /*[Http_VERB_(Name = "TemplateFacade_SERVICENAME__DOMODELACTION_")]*/
    #region to remove if NOT_ANONYMOUS
    [AllowAnonymous]
    #endregion to remove if NOT_ANONYMOUS
    public async Task<IActionResult> _DOMODELACTION_(/*PARAMETER_DEF_API*/)
    {
        #region to remove if ANONYMOUS
        executionContext.MandatoryRights.Add("MANDATORYRIGHT");
        executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
        #endregion to remove if ANONYMOUS
        DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO result;
        using (var unitOfWork = executionContext.StartUnitOfWork("Template", "SERVICENAMEController _DOMODELACTION_", UnitOfWorkType.UNITOFWORKTYPE))
        {
            var domainResult = await this.service._DOMODELACTION_(/*CONVERTED_PARAMETERS*/);
            if (domainResult.HasError) return Failure(domainResult.Error);

            List<string> preventLazyLoading = new();
            SetPreventLazyLoadingFor_DOMODELACTION_(ref preventLazyLoading);
            result = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(domainResult.Data, preventLazyLoading, string.Empty, null);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersFor_DOMODELACTION_(HttpContext);
        return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
    }
    partial void SetPreventLazyLoadingFor_DOMODELACTION_(ref List<string> preventLazyLoading);
    partial void AddAdditionalHeadersFor_DOMODELACTION_(Microsoft.AspNetCore.Http.HttpContext httpContext);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN 

    #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN[EXT]
    /*[Http_VERB_(Name = "TemplateFacade_SERVICENAME__DOMODELLISTACTION_")]*/
    #region to remove if NOT_ANONYMOUS
    [AllowAnonymous]
    #endregion to remove if NOT_ANONYMOUS
    public async Task<IActionResult> _DOMODELLISTACTION_(/*PARAMETER_DEF_API*/)
    {
        #region to remove if ANONYMOUS
        executionContext.MandatoryRights.Add("MANDATORYRIGHT");
        executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
        #endregion to remove if ANONYMOUS
        List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("Template", "SERVICENAMEController _DOMODELLISTACTION_", UnitOfWorkType.UNITOFWORKTYPE))
        {
            var domainResult = await this.service._DOMODELLISTACTION_(/*CONVERTED_PARAMETERS*/);
            if (domainResult.HasError) return Failure(domainResult.Error);

            List<string> preventLazyLoading = new();
            SetPreventLazyLoadingFor_DOMODELLISTACTION_(ref preventLazyLoading);
            result = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(domainResult.Data.ToList(), preventLazyLoading, string.Empty, null);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersFor_DOMODELLISTACTION_(HttpContext);
        return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
    }
    partial void SetPreventLazyLoadingFor_DOMODELLISTACTION_(ref List<string> preventLazyLoading);
    partial void AddAdditionalHeadersFor_DOMODELLISTACTION_(Microsoft.AspNetCore.Http.HttpContext httpContext);
    #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN 
}
#endregion foreach DOMAIN_SERVICE