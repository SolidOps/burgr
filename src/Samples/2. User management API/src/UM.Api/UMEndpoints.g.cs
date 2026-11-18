#nullable enable
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

        return app;
    }
    // Object [AG][R]
    private static void MapUserEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, UserService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult());

        group.MapGet("/{id}", async (Guid id, UserService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult());

        group.MapPost("/", async (UserInput input, UserService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/users/{dto.Id}"));

        group.MapPut("/{id}", async (Guid id, UserInput input, UserService service, CancellationToken cancellationToken) =>
            (await service.UpdateAsync(id, input, cancellationToken)).ToHttpResult());

        group.MapDelete("/{id}", async (Guid id, UserService service, CancellationToken cancellationToken) =>
            (await service.RemoveAsync(id, cancellationToken)).ToHttpResult());

    }

    private static void MapInviteEndpoints(RouteGroupBuilder group)
    {

        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, InviteService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult());

        group.MapGet("/{id}", async (Guid id, InviteService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult());

        group.MapPost("/", async (InviteInput input, InviteService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/invites/{dto.Id}"));

    }

}