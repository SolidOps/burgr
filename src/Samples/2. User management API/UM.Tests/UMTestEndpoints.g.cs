using SolidOps.SubZero;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Contracts.DTO;
using SolidOps.UM.Shared.Tests;
using Microsoft.Extensions.DependencyInjection;
namespace SolidOps.UM.Tests.Endpoints;
public class UMTestsServiceAccess
{
    public const string Name = "UM";
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
public static class UMAppServiceClientExtension
{
    // UseCase 

    public static async Task UMFacade_Authentication_Login(this AppServiceClient client, SolidOps.UM.Contracts.DTO.LoginRequestDTO request, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.LoginRequestDTO, string>(assertParameters, "Post", "um/authentication/login", request);
        await client.Send<SolidOps.UM.Contracts.DTO.LoginRequestDTO, string>(requestParameters);
    }

    public static async Task UMFacade_Authentication_SetInitialPassword(this AppServiceClient client, System.String email, System.String password, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<string>(assertParameters, "Post", "um/authentication/set-initial-password" + "?email=" + UriHelper.Convert<System.String>(email) + "&password=" + UriHelper.Convert<System.String>(password), default);
        await client.Send<string>(requestParameters);
    }

    public static async Task<System.Boolean> UMFacade_Authentication_NeedInitialPassword(this AppServiceClient client, System.String email, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<System.Boolean>(assertParameters, "Get", "um/authentication/need-initial-password" + "?email=" + UriHelper.Convert<System.String>(email), default);
        return await client.Send<System.Boolean>(requestParameters);
    }

    public static async Task UMFacade_Invites_UseInvite(this AppServiceClient client, System.Guid inviteId, System.String password, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<string>(assertParameters, "Post", "um/invites/use-invite" + "?inviteId=" + UriHelper.Convert<System.Guid>(inviteId) + "&password=" + UriHelper.Convert<System.String>(password), default);
        await client.Send<string>(requestParameters);
    }

    public static async Task<SolidOps.UM.Contracts.DTO.InviteResultDTO> UMFacade_Invites_CheckInvite(this AppServiceClient client, System.Guid inviteId, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.InviteResultDTO>(assertParameters, "Get", "um/invites/check-invite" + "?inviteId=" + UriHelper.Convert<System.Guid>(inviteId), default);
        return await client.Send<SolidOps.UM.Contracts.DTO.InviteResultDTO>(requestParameters);
    }

    public static async Task<string> UMFacade_SelfUserCreation_CreateUser(this AppServiceClient client, SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO request, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO, string>(assertParameters, "Post", "um/self-user-creation/create-user", request);
        requestParameters.DeserializeResponse = false;
        var res = await client.Send<SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO, string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return res;
        }
        return default;
    }

    public static async Task<System.Boolean> UMFacade_ServerStatus_NeedTechUserPasswordUpdate(this AppServiceClient client, System.String techUser, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<System.Boolean>(assertParameters, "Get", "um/server-status/need-tech-user-password-update" + "?techUser=" + UriHelper.Convert<System.String>(techUser), default);
        return await client.Send<System.Boolean>(requestParameters);
    }

    public static async Task<System.String> UMFacade_TokenValidation_Validate(this AppServiceClient client, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<System.String>(assertParameters, "Get", "um/token-validation/validate", default);
        return await client.Send<System.String>(requestParameters);
    }

    public static async Task<string> UMFacade_UserCreation_CreateUser(this AppServiceClient client, SolidOps.UM.Contracts.DTO.UserCreationInfoDTO userCreationInfo, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<SolidOps.UM.Contracts.DTO.UserCreationInfoDTO, string>(assertParameters, "Post", "um/user-creation/create-user", userCreationInfo);
        requestParameters.DeserializeResponse = false;
        var res = await client.Send<SolidOps.UM.Contracts.DTO.UserCreationInfoDTO, string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return res;
        }
        return default;
    }

    // Object [R]

    public static async Task<IEnumerable<Contracts.DTO.UserDTO>> UMAPI_GetUsers(this AppServiceClient client, Contracts.DTO.UserQueryFilterDTO filter = null, List<string> includes = null, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserDTO, IEnumerable<Contracts.DTO.UserDTO>>(assertParameters)
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
        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }
        return result;
    }

    public static async Task<Contracts.DTO.UserDTO> UMAPI_GetUser(this AppServiceClient client, string id, List<string> includes = null, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserDTO>(assertParameters)
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
        return await client.Send<Contracts.DTO.UserDTO, Contracts.DTO.UserDTO>(requestParameters);
    }

    public static async Task<string> UMAPI_AddUser(this AppServiceClient client, Contracts.DTO.UserWriteDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserWriteDTO, string>(assertParameters)
        {
            Method = "POST",
            Uri = "um/user",
            Data = value,
            DeserializeResponse = false
        };
        var locationString = await client.Send<Contracts.DTO.UserWriteDTO, string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return locationString;
        }
        return null;
    }

    public static async Task<Contracts.DTO.UserDTO> UMAPI_AddUserAndGet(this AppServiceClient client, Contracts.DTO.UserWriteDTO value, AssertParameters assertParameters = null)
    {
        var id = await client.UMAPI_AddUser(value, assertParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return await client.UMAPI_GetUser(id);
        }
        return null;
    }

    public static async Task UMAPI_UpdateUser(this AppServiceClient client, string id, Contracts.DTO.UserWriteDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserWriteDTO, string>(assertParameters)
        {
            Method = "PUT",
            Uri = "um/user/" + id,
            Data = value,
            DeserializeResponse = false
        };
        await client.Send<Contracts.DTO.UserWriteDTO, string>(requestParameters);
    }
    public static async Task UMAPI_PatchUser(this AppServiceClient client, string id, Contracts.DTO.UserPatchDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.UserPatchDTO, string>(assertParameters)
        {
            Method = "PATCH",
            Uri = "um/user/" + id,
            Data = value
        };
        await client.Send<Contracts.DTO.UserPatchDTO, string>(requestParameters);
    }

    public static async Task<IEnumerable<Contracts.DTO.InviteDTO>> UMAPI_GetInvites(this AppServiceClient client, Contracts.DTO.InviteQueryFilterDTO filter = null, List<string> includes = null, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.InviteDTO, IEnumerable<Contracts.DTO.InviteDTO>>(assertParameters)
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
        if (filter != null && filter.MaxResults > 0)
        {
            filter.ContinuationId = requestParameters.ResponseContinuationId;
        }
        return result;
    }

    public static async Task<string> UMAPI_AddInvite(this AppServiceClient client, Contracts.DTO.InviteWriteDTO value, AssertParameters assertParameters = null)
    {
        var requestParameters = new RequestParameters<Contracts.DTO.InviteWriteDTO, string>(assertParameters)
        {
            Method = "POST",
            Uri = "um/invite",
            Data = value,
            DeserializeResponse = false
        };
        var locationString = await client.Send<Contracts.DTO.InviteWriteDTO, string>(requestParameters);
        if (assertParameters == null || assertParameters.EnsureSuccess)
        {
            return locationString;
        }
        return null;
    }

}