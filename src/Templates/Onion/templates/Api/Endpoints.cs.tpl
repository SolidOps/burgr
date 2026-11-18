#nullable enable

using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using _MODULE_NAMESPACE_.Application;

namespace _MODULE_NAMESPACE_.Api;

public static partial class _MODULE_Endpoints
{
    public static IEndpointRouteBuilder Map_MODULE_Endpoints(this IEndpointRouteBuilder app)
    {
        #region foreach MODEL[AG][R]
        MapCLASSNAMEEndpoints(app.MapGroup("/Slug_CLASSNAMES_").WithTags("_CLASSNAMES_"));
        #endregion foreach MODEL
        return app;
    }

    #region foreach MODEL[AG][R]
    private static void MapCLASSNAMEEndpoints(RouteGroupBuilder group)
    {
        #region to remove if NOT_GETBYQUERY
        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult());
        #endregion to remove if NOT_GETBYQUERY

        #region to remove if NOT_GETBYID
        group.MapGet("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult());
        #endregion to remove if NOT_GETBYID

        #region to remove if NOT_ADD
        group.MapPost("/", async (CLASSNAMEInput input, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/Slug_CLASSNAMES_/{dto.Id}"));
        #endregion to remove if NOT_ADD

        #region to remove if NOT_UPDATE
        group.MapPut("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEInput input, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.UpdateAsync(id, input, cancellationToken)).ToHttpResult());
        #endregion to remove if NOT_UPDATE

        #region to remove if NOT_REMOVE
        group.MapDelete("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.RemoveAsync(id, cancellationToken)).ToHttpResult());
        #endregion to remove if NOT_REMOVE
    }

    #endregion foreach MODEL
}
