using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Contracts.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace MetaCorp.Template.Contracts.Endpoints;

#region to remove if NO_API
public class TemplateServiceAccess
{
    public const string Name = "Template";

    public static AppServiceClient CreateClient(IServiceProvider serviceProvider, string authorization)
    {
        return CreateClient(serviceProvider.GetRequiredService<IInternalCommunicationService>(), authorization);
    }
    public static AppServiceClient CreateClient(IInternalCommunicationService internalCommService, string authorization)
    {
        AppServiceClient client = new AppServiceClient(internalCommService.GetClient(Name, authorization), false, false, internalCommService.GetMessageHandler(Name));
        return client;
    }
}
#endregion to remove if NO_API

public static class TemplateAppServiceClientExtension
{
    #region foreach DOMAIN_USECASE
    #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
    public static async Task<IOpsResult> TemplateFacade_USECASENAME__DOVOIDACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(ensureSuccess, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOVOIDACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }
    #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN
    #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    public static async Task<IOpsResult<string>> TemplateFacade_USECASENAME__DOIDENTITYACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(ensureSuccess, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOIDENTITYACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        requestParameters.DeserializeResponse = false;
        var result = await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();
        return IOpsResult.Ok(result);
    }
    #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    public static async Task<IOpsResult<_SIMPLE__TYPE_>> TemplateFacade_USECASENAME__DOSIMPLEACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/_SIMPLE__TYPE_>(ensureSuccess, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOSIMPLEACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        var result = await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/_SIMPLE__TYPE_>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<_SIMPLE__TYPE_>();
        return IOpsResult.Ok(result);
    }
    #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    public static async Task<IOpsResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> TemplateFacade_USECASENAME__DOMODELACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>(ensureSuccess, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOMODELACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        var result = await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>();
        return IOpsResult.Ok(result);
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    public static async Task<IOpsResult<List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>> TemplateFacade_USECASENAME__DOMODELLISTACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>(ensureSuccess, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOMODELLISTACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        var result = await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>();
        return IOpsResult.Ok(result);
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    #endregion foreach DOMAIN_USECASE

    #region foreach MODEL[R]

    #region to remove if NOT_GETBYQUERY
    public static async Task<IOpsResult<IEnumerable<Contracts.DTO.CLASSNAMEDTO>>> TemplateAPI_GetCLASSNAMEs(this AppServiceClient client, Contracts.DTO.CLASSNAMEQueryFilterDTO filter = null, List<string> includes = null, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO, IEnumerable<Contracts.DTO.CLASSNAMEDTO>>(ensureSuccess)
        {
            Method = "GET",
            Uri = "SlugTemplate/SlugCLASSNAME"
        };

        var queryParam = "";
        if (filter != null)
        {
            if (filter.Filter != null)
            {
                queryParam += "&filter=" + filter.Filter;
            }
            if (filter.OrderBy != null)
            {
                queryParam += "&orderBy=" + string.Join("|", filter.OrderBy.Select(ob => (ob.Way == OrderByWay.Descending ? "-" : "") + ob.Member));
            }
            if (filter.MaxResults.HasValue)
            {
                queryParam += "&maxResults=" + filter.MaxResults.Value;
            }
            if (filter.ContinuationId != null)
            {
                queryParam += "&continuationId=" + filter.ContinuationId;
            }
        }

        if (includes != null)
        {
            queryParam += "&includes=" + string.Join("|", includes);
        }

        if (queryParam != string.Empty)
        {
            requestParameters.Uri += $"/?{queryParam.Substring(1)}";
        }

        var result = await client.Send<Contracts.DTO.CLASSNAMEDTO, IEnumerable<Contracts.DTO.CLASSNAMEDTO>>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<IEnumerable<Contracts.DTO.CLASSNAMEDTO>>();

        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }

        return IOpsResult.Ok(result);
    }
    #endregion to remove if NOT_GETBYQUERY

    #region to remove if NOT_GETBYID
    public static async Task<IOpsResult<Contracts.DTO.CLASSNAMEDTO>> TemplateAPI_GetCLASSNAME(this AppServiceClient client, string id, List<string> includes = null, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO>(ensureSuccess)
        {
            Method = "GET",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id
        };

        var queryParam = "";
        if (includes != null)
        {
            queryParam += "&includes=" + string.Join("|", includes);
        }

        if (queryParam != string.Empty)
        {
            requestParameters.Uri += $"/?{queryParam.Substring(1)}";
        }

        var result = await client.Send<Contracts.DTO.CLASSNAMEDTO, Contracts.DTO.CLASSNAMEDTO>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<Contracts.DTO.CLASSNAMEDTO>();

        return IOpsResult.Ok(result);
    }
    #endregion to remove if NOT_GETBYID

    #region to remove if NOT_ADD
    public static async Task<IOpsResult<string>> TemplateAPI_AddCLASSNAME(this AppServiceClient client, Contracts.DTO.CLASSNAMEWriteDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEWriteDTO, string>(ensureSuccess)
        {
            Method = "POST",
            Uri = "SlugTemplate/SlugCLASSNAME",
            Data = value,
            DeserializeResponse = false
        };
        var result = await client.Send<Contracts.DTO.CLASSNAMEWriteDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();

        return IOpsResult.Ok(result);
    }

    #region to remove if NOT_GETBYID
    public static async Task<IOpsResult<Contracts.DTO.CLASSNAMEDTO>> TemplateAPI_AddCLASSNAMEAndGet(this AppServiceClient client, Contracts.DTO.CLASSNAMEWriteDTO value, bool ensureSuccess = true)
    {
        var result = await client.TemplateAPI_AddCLASSNAME(value, ensureSuccess);
        if (result.HasError) return result.ToResult<Contracts.DTO.CLASSNAMEDTO>();

        return await client.TemplateAPI_GetCLASSNAME(result.Data);
    }
    #endregion to remove if NOT_GETBYID

    #endregion to remove if NOT_ADD

    #region to remove if NOT_UPDATE
    public static async Task<IOpsResult> TemplateAPI_UpdateCLASSNAME(this AppServiceClient client, string id, Contracts.DTO.CLASSNAMEWriteDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEWriteDTO, string>(ensureSuccess)
        {
            Method = "PUT",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            Data = value,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.CLASSNAMEWriteDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);

        return IOpsResult.Ok();
    }

    public static async Task<IOpsResult> TemplateAPI_PatchCLASSNAME(this AppServiceClient client, string id, Contracts.DTO.CLASSNAMEPatchDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEPatchDTO, string>(ensureSuccess)
        {
            Method = "PATCH",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            Data = value
        };
        await client.Send<Contracts.DTO.CLASSNAMEPatchDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);

        return IOpsResult.Ok();
    }
    #endregion to remove if NOT_UPDATE

    #region to remove if NOT_REMOVE

    public static async Task<IOpsResult> TemplateAPI_RemoveCLASSNAME(this AppServiceClient client, string id, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO, string>(ensureSuccess)
        {
            Method = "DELETE",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.CLASSNAMEDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);

        return IOpsResult.Ok();
    }

    #endregion to remove if NOT_REMOVE
    #endregion foreach MODEL
}