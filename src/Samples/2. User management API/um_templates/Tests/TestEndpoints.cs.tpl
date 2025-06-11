using SolidOps.SubZero;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Contracts.DTO;
using SolidOps.UM.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace MetaCorp.Template.Tests.Endpoints;

#region to remove if NO_API
public class TemplateTestsServiceAccess
{
    public const string Name = "Template";

    public static AppServiceClient CreateClient(IServiceProvider serviceProvider)
    {
        var internalCommService = serviceProvider.GetRequiredService<IInternalCommunicationService>();
        var executionContext = serviceProvider.GetRequiredService<IExecutionContext>();
        return CreateClient(internalCommService, executionContext.Authorization);
    }
    public static AppServiceClient CreateClient(IInternalCommunicationService internalCommService, string authorization)
    {
        AppServiceClient client = new TestServiceClient(internalCommService.GetClient(Name, authorization), false, false, internalCommService.GetMessageHandler(Name));
        return client;
    }
}
#endregion to remove if NO_API

public static class TemplateAppServiceClientExtension
{
    #region foreach DOMAIN_USECASE
    #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
    public static async Task TemplateFacade_USECASENAME__DOVOIDACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(assertParameters, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOVOIDACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(requestParameters);
    }
    #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN
    #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    public static async Task<string> TemplateFacade_USECASENAME__DOIDENTITYACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(assertParameters, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOIDENTITYACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        requestParameters.DeserializeResponse = false;
        var res = await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return res;
        }
        return default;
    }
    #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
    #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    public static async Task<_SIMPLE__TYPE_> TemplateFacade_USECASENAME__DOSIMPLEACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/_SIMPLE__TYPE_>(assertParameters, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOSIMPLEACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        return await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/_SIMPLE__TYPE_>(requestParameters);
    }
    #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
    #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    public static async Task<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO> TemplateFacade_USECASENAME__DOMODELACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>(assertParameters, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOMODELACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        return await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>(requestParameters);
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN
    #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    public static async Task<List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>> TemplateFacade_USECASENAME__DOMODELLISTACTION_(this AppServiceClient client/*COMMA*//*PARAMETER_DEFINITION*/, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>(assertParameters, "_VERB_", "SlugTemplate/SlugUSECASENAME/_DOMODELLISTACTIONURL_"/*PARAMETER_URL*//*COMMA_DATA*//*PARAMETER_DATA*/);
        return await client.Send</*PARAMETER_DATA_TYPE*//*COMMA_TYPE*/List<DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO>>(requestParameters);
    }
    #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
    #endregion foreach DOMAIN_USECASE

    #region foreach MODEL[R]

    #region to remove if NOT_GETBYQUERY
    public static async Task<IEnumerable<Contracts.DTO.CLASSNAMEDTO>> TemplateAPI_GetCLASSNAMEs(this AppServiceClient client, Contracts.DTO.CLASSNAMEQueryFilterDTO filter = null, List<string> includes = null, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO, IEnumerable<Contracts.DTO.CLASSNAMEDTO>>(assertParameters)
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
        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }
        return result;
    }
    #endregion to remove if NOT_GETBYQUERY

    #region to remove if NOT_GETBYID
    public static async Task<Contracts.DTO.CLASSNAMEDTO> TemplateAPI_GetCLASSNAME(this AppServiceClient client, string id, List<string> includes = null, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO>(assertParameters)
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

        return await client.Send<Contracts.DTO.CLASSNAMEDTO, Contracts.DTO.CLASSNAMEDTO>(requestParameters);
    }
    #endregion to remove if NOT_GETBYID

    #region to remove if NOT_ADD
    public static async Task<string> TemplateAPI_AddCLASSNAME(this AppServiceClient client, Contracts.DTO.CLASSNAMEWriteDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEWriteDTO, string>(assertParameters)
        {
            Method = "POST",
            Uri = "SlugTemplate/SlugCLASSNAME",
            Data = value,
            DeserializeResponse = false
        };
        var locationString = await client.Send<Contracts.DTO.CLASSNAMEWriteDTO, string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return locationString;
        }
        return null;
    }

    #region to remove if NOT_GETBYID
    public static async Task<Contracts.DTO.CLASSNAMEDTO> TemplateAPI_AddCLASSNAMEAndGet(this AppServiceClient client, Contracts.DTO.CLASSNAMEWriteDTO value, AssertParameters assertParameters = null)
    {
        var id = await client.TemplateAPI_AddCLASSNAME(value, assertParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return await client.TemplateAPI_GetCLASSNAME(id);
        }
        return null;
    }
    #endregion to remove if NOT_GETBYID

    #endregion to remove if NOT_ADD

    #region to remove if NOT_UPDATE
    public static async Task TemplateAPI_UpdateCLASSNAME(this AppServiceClient client, string id, Contracts.DTO.CLASSNAMEWriteDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEWriteDTO, string>(assertParameters)
        {
            Method = "PUT",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            Data = value,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.CLASSNAMEWriteDTO, string>(requestParameters);
    }

    public static async Task TemplateAPI_PatchCLASSNAME(this AppServiceClient client, string id, Contracts.DTO.CLASSNAMEPatchDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEPatchDTO, string>(assertParameters)
        {
            Method = "PATCH",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            Data = value
        };
        await client.Send<Contracts.DTO.CLASSNAMEPatchDTO, string>(requestParameters);
    }
    #endregion to remove if NOT_UPDATE

    #region to remove if NOT_REMOVE

    public static async Task TemplateAPI_RemoveCLASSNAME(this AppServiceClient client, string id, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.CLASSNAMEDTO, string>(assertParameters)
        {
            Method = "DELETE",
            Uri = "SlugTemplate/SlugCLASSNAME/" + id,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.CLASSNAMEDTO, string>(requestParameters);
    }

    #endregion to remove if NOT_REMOVE
    #endregion foreach MODEL
}