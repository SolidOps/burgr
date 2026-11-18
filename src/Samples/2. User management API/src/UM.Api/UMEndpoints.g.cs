#nullable enable
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SolidOps.UM.Application;
namespace SolidOps.UM.Api;
public static partial class UMEndpoints
{
    public static IEndpointRouteBuilder MapUMEndpoints(this IEndpointRouteBuilder app)
    {
        // Object [AG][R]
        MapUserEndpoints(app.MapGroup("/users").WithTags("Users"));

        MapInviteEndpoints(app.MapGroup("/invites").WithTags("Invites"));

        // Service 
        MapAuthenticationEndpoints(app.MapGroup("/authentication").WithTags("Authentication"));

        MapInvitesEndpoints(app.MapGroup("/invites").WithTags("Invites"));

        MapSelfUserCreationEndpoints(app.MapGroup("/self-user-creation").WithTags("SelfUserCreation"));

        MapServerStatusEndpoints(app.MapGroup("/server-status").WithTags("ServerStatus"));

        MapTokenValidationEndpoints(app.MapGroup("/token-validation").WithTags("TokenValidation"));

        MapUserCreationEndpoints(app.MapGroup("/user-creation").WithTags("UserCreation"));

        return app;
    }
    // Object [AG][R]
    private static void MapUserEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, UserService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult())

            .RequireRight("")

            ;

        group.MapGet("/{id}", async (Guid id, UserService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult())

            .RequireRight("")

            ;

        group.MapPost("/", async (UserInput input, UserService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/users/{dto.Id}"))

            .RequireRight("ManageUsers")

            ;

        group.MapPut("/{id}", async (Guid id, UserInput input, UserService service, CancellationToken cancellationToken) =>
            (await service.UpdateAsync(id, input, cancellationToken)).ToHttpResult())

            .RequireRight("ManageUsers")

            ;

        group.MapDelete("/{id}", async (Guid id, UserService service, CancellationToken cancellationToken) =>
            (await service.RemoveAsync(id, cancellationToken)).ToHttpResult())

            .RequireRight("ManageUsers")

            ;

    }

    private static void MapInviteEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, InviteService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult())

            .RequireRight("")

            ;

        group.MapGet("/{id}", async (Guid id, InviteService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult())

            .RequireRight("")

            ;

        group.MapPost("/", async (InviteInput input, InviteService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/invites/{dto.Id}"))

            .RequireRight("")

            ;

    }

    // Service 
    private static void MapAuthenticationEndpoints(RouteGroupBuilder group)
    {

        group.MapPost("/set-initial-password", async ([FromBody] SolidOps.UM.Application.LoginRequestDto request, IAuthenticationService service, CancellationToken cancellationToken) =>
            (await service.SetInitialPasswordAsync(request, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

        group.MapPost("/login", async ([FromBody] SolidOps.UM.Application.LoginRequestDto request, IAuthenticationService service, CancellationToken cancellationToken) =>
            (await service.LoginAsync(request, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

        group.MapGet("/need-initial-password", async (System.String email, IAuthenticationService service, CancellationToken cancellationToken) =>
            (await service.NeedInitialPasswordAsync(email, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

    }

    private static void MapInvitesEndpoints(RouteGroupBuilder group)
    {

        group.MapPost("/use-invite", async ([FromBody] SolidOps.UM.Application.InviteUseRequestDto request, IInvitesService service, CancellationToken cancellationToken) =>
            (await service.UseInviteAsync(request, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

        group.MapGet("/count-by-status", async (SolidOps.UM.Domain.InviteStatus status, IInvitesService service, CancellationToken cancellationToken) =>
            (await service.CountByStatusAsync(status, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .RequireRight("")

            ;

        group.MapGet("/check-invite", async (System.Guid inviteId, IInvitesService service, CancellationToken cancellationToken) =>
            (await service.CheckInviteAsync(inviteId, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

    }

    private static void MapSelfUserCreationEndpoints(RouteGroupBuilder group)
    {

        group.MapPost("/create-user", async ([FromBody] SolidOps.UM.Application.SelfUserCreationRequestDto request, ISelfUserCreationService service, CancellationToken cancellationToken) =>
            (await service.CreateUserAsync(request, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

    }

    private static void MapServerStatusEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/need-tech-user-password-update", async (System.String techUser, IServerStatusService service, CancellationToken cancellationToken) =>
            (await service.NeedTechUserPasswordUpdateAsync(techUser, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .AllowAnonymous()

            ;

    }

    private static void MapTokenValidationEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/validate", async (ITokenValidationService service, CancellationToken cancellationToken) =>
            (await service.ValidateAsync(cancellationToken)).ToHttpResult())
            .ValidateBody()

            .RequireRight("")

            ;

    }

    private static void MapUserCreationEndpoints(RouteGroupBuilder group)
    {

        group.MapPost("/create-user", async ([FromBody] SolidOps.UM.Application.UserCreationInfoDto userCreationInfo, IUserCreationService service, CancellationToken cancellationToken) =>
            (await service.CreateUserAsync(userCreationInfo, cancellationToken)).ToHttpResult())
            .ValidateBody()

            .RequireRight("CreateUser")

            ;

    }

}