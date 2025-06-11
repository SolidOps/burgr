using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Contracts.DTO;
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Contracts.Endpoints;
public class UMServiceAccess
{
    public const string Name = "UM";
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
public static class UMAppServiceClientExtension
{
    // UseCase 

    public static async Task<IOpsResult> UMFacade_Authentication_Login(this AppServiceClient client, SolidOps.UM.Contracts.DTO.LoginRequestDTO request, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.LoginRequestDTO, string>(ensureSuccess, "Post", "um/authentication/login", request);
        await client.Send<SolidOps.UM.Contracts.DTO.LoginRequestDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }

    public static async Task<IOpsResult> UMFacade_Authentication_SetInitialPassword(this AppServiceClient client, System.String email, System.String password, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<string>(ensureSuccess, "Post", "um/authentication/set-initial-password" + "?email=" + UriHelper.Convert<System.String>(email) + "&password=" + UriHelper.Convert<System.String>(password), default);
        await client.Send<string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }

    public static async Task<IOpsResult<System.Boolean>> UMFacade_Authentication_NeedInitialPassword(this AppServiceClient client, System.String email, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<System.Boolean>(ensureSuccess, "Get", "um/authentication/need-initial-password" + "?email=" + UriHelper.Convert<System.String>(email), default);
        var result = await client.Send<System.Boolean>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<System.Boolean>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult> UMFacade_Invites_UseInvite(this AppServiceClient client, System.Guid inviteId, System.String password, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<string>(ensureSuccess, "Post", "um/invites/use-invite" + "?inviteId=" + UriHelper.Convert<System.Guid>(inviteId) + "&password=" + UriHelper.Convert<System.String>(password), default);
        await client.Send<string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }

    public static async Task<IOpsResult<SolidOps.UM.Contracts.DTO.InviteResultDTO>> UMFacade_Invites_CheckInvite(this AppServiceClient client, System.Guid inviteId, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.InviteResultDTO>(ensureSuccess, "Get", "um/invites/check-invite" + "?inviteId=" + UriHelper.Convert<System.Guid>(inviteId), default);
        var result = await client.Send<SolidOps.UM.Contracts.DTO.InviteResultDTO>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<SolidOps.UM.Contracts.DTO.InviteResultDTO>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<string>> UMFacade_SelfUserCreation_CreateUser(this AppServiceClient client, SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO request, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO, string>(ensureSuccess, "Post", "um/self-user-creation/create-user", request);
        requestParameters.DeserializeResponse = false;
        var result = await client.Send<SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<System.Boolean>> UMFacade_ServerStatus_NeedTechUserPasswordUpdate(this AppServiceClient client, System.String techUser, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<System.Boolean>(ensureSuccess, "Get", "um/server-status/need-tech-user-password-update" + "?techUser=" + UriHelper.Convert<System.String>(techUser), default);
        var result = await client.Send<System.Boolean>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<System.Boolean>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<System.String>> UMFacade_TokenValidation_Validate(this AppServiceClient client, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<System.String>(ensureSuccess, "Get", "um/token-validation/validate", default);
        var result = await client.Send<System.String>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<System.String>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<string>> UMFacade_UserCreation_CreateUser(this AppServiceClient client, SolidOps.UM.Contracts.DTO.UserCreationInfoDTO userCreationInfo, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.UserCreationInfoDTO, string>(ensureSuccess, "Post", "um/user-creation/create-user", userCreationInfo);
        requestParameters.DeserializeResponse = false;
        var result = await client.Send<SolidOps.UM.Contracts.DTO.UserCreationInfoDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();
        return IOpsResult.Ok(result);
    }

    // Object [R]

    public static async Task<IOpsResult<IEnumerable<Contracts.DTO.UserDTO>>> UMAPI_GetUsers(this AppServiceClient client, Contracts.DTO.UserQueryFilterDTO filter = null, List<string> includes = null, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserDTO, IEnumerable<Contracts.DTO.UserDTO>>(ensureSuccess)
        {
            Method = "GET",
            Uri = "um/user"
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
        var result = await client.Send<Contracts.DTO.UserDTO, IEnumerable<Contracts.DTO.UserDTO>>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<IEnumerable<Contracts.DTO.UserDTO>>();
        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<Contracts.DTO.UserDTO>> UMAPI_GetUser(this AppServiceClient client, string id, List<string> includes = null, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserDTO>(ensureSuccess)
        {
            Method = "GET",
            Uri = "um/user/" + id
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
        var result = await client.Send<Contracts.DTO.UserDTO, Contracts.DTO.UserDTO>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<Contracts.DTO.UserDTO>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<string>> UMAPI_AddUser(this AppServiceClient client, Contracts.DTO.UserWriteDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserWriteDTO, string>(ensureSuccess)
        {
            Method = "POST",
            Uri = "um/user",
            Data = value,
            DeserializeResponse = false
        };
        var result = await client.Send<Contracts.DTO.UserWriteDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<Contracts.DTO.UserDTO>> UMAPI_AddUserAndGet(this AppServiceClient client, Contracts.DTO.UserWriteDTO value, bool ensureSuccess = true)
    {
        var result = await client.UMAPI_AddUser(value, ensureSuccess);
        if (result.HasError) return result.ToResult<Contracts.DTO.UserDTO>();
        return await client.UMAPI_GetUser(result.Data);
    }

    public static async Task<IOpsResult> UMAPI_UpdateUser(this AppServiceClient client, string id, Contracts.DTO.UserWriteDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserWriteDTO, string>(ensureSuccess)
        {
            Method = "PUT",
            Uri = "um/user/" + id,
            Data = value,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.UserWriteDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }
    public static async Task<IOpsResult> UMAPI_PatchUser(this AppServiceClient client, string id, Contracts.DTO.UserPatchDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserPatchDTO, string>(ensureSuccess)
        {
            Method = "PATCH",
            Uri = "um/user/" + id,
            Data = value
        };
        await client.Send<Contracts.DTO.UserPatchDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure);
        return IOpsResult.Ok();
    }

    public static async Task<IOpsResult<IEnumerable<Contracts.DTO.InviteDTO>>> UMAPI_GetInvites(this AppServiceClient client, Contracts.DTO.InviteQueryFilterDTO filter = null, List<string> includes = null, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.InviteDTO, IEnumerable<Contracts.DTO.InviteDTO>>(ensureSuccess)
        {
            Method = "GET",
            Uri = "um/invite"
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
        var result = await client.Send<Contracts.DTO.InviteDTO, IEnumerable<Contracts.DTO.InviteDTO>>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<IEnumerable<Contracts.DTO.InviteDTO>>();
        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }
        return IOpsResult.Ok(result);
    }

    public static async Task<IOpsResult<string>> UMAPI_AddInvite(this AppServiceClient client, Contracts.DTO.InviteWriteDTO value, bool ensureSuccess = true)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.InviteWriteDTO, string>(ensureSuccess)
        {
            Method = "POST",
            Uri = "um/invite",
            Data = value,
            DeserializeResponse = false
        };
        var result = await client.Send<Contracts.DTO.InviteWriteDTO, string>(requestParameters);
        if (!string.IsNullOrEmpty(requestParameters.Failure))
            return IOpsResult.Invalid(requestParameters.Failure).ToResult<string>();
        return IOpsResult.Ok(result);
    }

}