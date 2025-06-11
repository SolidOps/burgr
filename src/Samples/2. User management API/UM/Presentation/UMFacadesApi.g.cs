using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Application.UseCases;
using System.Diagnostics;
using SolidOps.UM.Shared.Presentation;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Contracts.Results;
namespace SolidOps.UM.Presentation.Controllers;
// UseCase 
[Route("um/authentication/[action]")]
public partial class AuthenticationController : BaseController
{
    private readonly IAuthenticationUseCase service;
    private readonly IOutputSerializer serializer;
    public AuthenticationController(IAuthenticationUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpPost(Name = "UMFacade_Authentication_Login")]

    [AllowAnonymous]

    public async Task<IActionResult> Login([FromBody] SolidOps.UM.Contracts.DTO.LoginRequestDTO request)
    {

        IOpsResult result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "AuthenticationController Login", UnitOfWorkType.Write))
        {
            result = await this.service.Login(new SolidOps.UM.Presentation.Mappers.LoginRequestDTOMapper(serviceProvider).Convert(request, serviceProvider));
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForLogin(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersForLogin(Microsoft.AspNetCore.Http.HttpContext httpContext);

    [HttpPost(Name = "UMFacade_Authentication_SetInitialPassword")]

    [AllowAnonymous]

    public async Task<IActionResult> SetInitialPassword(System.String email, System.String password)
    {

        IOpsResult result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "AuthenticationController SetInitialPassword", UnitOfWorkType.Write))
        {
            result = await this.service.SetInitialPassword(email, password);
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForSetInitialPassword(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersForSetInitialPassword(Microsoft.AspNetCore.Http.HttpContext httpContext);

    [HttpGet(Name = "UMFacade_Authentication_NeedInitialPassword")]

    [AllowAnonymous]

    public async Task<IActionResult> NeedInitialPassword(System.String email)
    {

        IOpsResult<System.Boolean> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "AuthenticationController NeedInitialPassword", UnitOfWorkType.Read))
        {
            result = await this.service.NeedInitialPassword(email);
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForNeedInitialPassword(HttpContext);
        if (result.Data.GetType().IsArray)
            return Content(serializer.Serialize(result.Data), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        return Ok(result.Data);
    }
    partial void AddAdditionalHeadersForNeedInitialPassword(Microsoft.AspNetCore.Http.HttpContext httpContext);

}
[Route("um/invites/[action]")]
public partial class InvitesController : BaseController
{
    private readonly IInvitesUseCase service;
    private readonly IOutputSerializer serializer;
    public InvitesController(IInvitesUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpPost(Name = "UMFacade_Invites_UseInvite")]

    [AllowAnonymous]

    public async Task<IActionResult> UseInvite(System.Guid inviteId, System.String password)
    {

        IOpsResult result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "InvitesController UseInvite", UnitOfWorkType.Write))
        {
            result = await this.service.UseInvite(inviteId, password);
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForUseInvite(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersForUseInvite(Microsoft.AspNetCore.Http.HttpContext httpContext);

    [HttpGet(Name = "UMFacade_Invites_CheckInvite")]

    [AllowAnonymous]

    public async Task<IActionResult> CheckInvite(System.Guid inviteId)
    {

        SolidOps.UM.Contracts.DTO.InviteResultDTO result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "InvitesController CheckInvite", UnitOfWorkType.Read))
        {
            var domainResult = await this.service.CheckInvite(inviteId);
            if (domainResult.HasError) return Failure(domainResult.Error);
            List<string> preventLazyLoading = new();
            SetPreventLazyLoadingForCheckInvite(ref preventLazyLoading);
            result = new SolidOps.UM.Presentation.Mappers.InviteResultDTOMapper(serviceProvider).Convert(domainResult.Data, preventLazyLoading, string.Empty, null);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForCheckInvite(HttpContext);
        return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
    }
    partial void SetPreventLazyLoadingForCheckInvite(ref List<string> preventLazyLoading);
    partial void AddAdditionalHeadersForCheckInvite(Microsoft.AspNetCore.Http.HttpContext httpContext);

}
[Route("um/self-user-creation/[action]")]
public partial class SelfUserCreationController : BaseController
{
    private readonly ISelfUserCreationUseCase service;
    private readonly IOutputSerializer serializer;
    public SelfUserCreationController(ISelfUserCreationUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpPost(Name = "UMFacade_SelfUserCreation_CreateUser")]

    [AllowAnonymous]

    public async Task<IActionResult> CreateUser([FromBody] SolidOps.UM.Contracts.DTO.SelfUserCreationRequestDTO request)
    {

        IOpsResult<Guid> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "SelfUserCreationController CreateUser", UnitOfWorkType.Write))
        {
            result = await this.service.CreateUser(new SolidOps.UM.Presentation.Mappers.SelfUserCreationRequestDTOMapper(serviceProvider).Convert(request, serviceProvider));
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        HttpContext.Response.Headers.Add("Location", new[] { $"{result.Data}" });
        AddAdditionalHeadersForCreateUser(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersForCreateUser(Microsoft.AspNetCore.Http.HttpContext httpContext);

}
[Route("um/server-status/[action]")]
public partial class ServerStatusController : BaseController
{
    private readonly IServerStatusUseCase service;
    private readonly IOutputSerializer serializer;
    public ServerStatusController(IServerStatusUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpGet(Name = "UMFacade_ServerStatus_NeedTechUserPasswordUpdate")]

    [AllowAnonymous]

    public async Task<IActionResult> NeedTechUserPasswordUpdate(System.String techUser)
    {

        IOpsResult<System.Boolean> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "ServerStatusController NeedTechUserPasswordUpdate", UnitOfWorkType.Read))
        {
            result = await this.service.NeedTechUserPasswordUpdate(techUser);
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForNeedTechUserPasswordUpdate(HttpContext);
        if (result.Data.GetType().IsArray)
            return Content(serializer.Serialize(result.Data), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        return Ok(result.Data);
    }
    partial void AddAdditionalHeadersForNeedTechUserPasswordUpdate(Microsoft.AspNetCore.Http.HttpContext httpContext);

}
[Route("um/token-validation/[action]")]
public partial class TokenValidationController : BaseController
{
    private readonly ITokenValidationUseCase service;
    private readonly IOutputSerializer serializer;
    public TokenValidationController(ITokenValidationUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpGet(Name = "UMFacade_TokenValidation_Validate")]

    public async Task<IActionResult> Validate()
    {

        executionContext.MandatoryRights.Add("");
        executionContext.OwnershipOverrideRights.Add("");

        IOpsResult<System.String> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "TokenValidationController Validate", UnitOfWorkType.Read))
        {
            result = await this.service.Validate();
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        AddAdditionalHeadersForValidate(HttpContext);
        if (result.Data.GetType().IsArray)
            return Content(serializer.Serialize(result.Data), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        return Ok(result.Data);
    }
    partial void AddAdditionalHeadersForValidate(Microsoft.AspNetCore.Http.HttpContext httpContext);

}
[Route("um/user-creation/[action]")]
public partial class UserCreationController : BaseController
{
    private readonly IUserCreationUseCase service;
    private readonly IOutputSerializer serializer;
    public UserCreationController(IUserCreationUseCase service
        , IExecutionContext executionContext
        , IServiceProvider serviceProvider        
        , IExtendedConfiguration configuration
        , IOutputSerializer serializer
        ) : base(executionContext, configuration, serviceProvider)
    {
        this.service = service;
        this.serializer = serializer;
    }

    [HttpPost(Name = "UMFacade_UserCreation_CreateUser")]

    public async Task<IActionResult> CreateUser([FromBody] SolidOps.UM.Contracts.DTO.UserCreationInfoDTO userCreationInfo)
    {

        executionContext.MandatoryRights.Add("CreateUser");
        executionContext.OwnershipOverrideRights.Add("");

        IOpsResult<Guid> result;
        using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserCreationController CreateUser", UnitOfWorkType.Write))
        {
            result = await this.service.CreateUser(new SolidOps.UM.Presentation.Mappers.UserCreationInfoDTOMapper(serviceProvider).Convert(userCreationInfo, serviceProvider));
            if (result.HasError) return Failure(result.Error);
            unitOfWork.Complete();
        }
        HttpContext.Response.Headers.Add("Location", new[] { $"{result.Data}" });
        AddAdditionalHeadersForCreateUser(HttpContext);
        return Ok();
    }
    partial void AddAdditionalHeadersForCreateUser(Microsoft.AspNetCore.Http.HttpContext httpContext);

}