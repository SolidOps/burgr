#nullable enable

using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        #region foreach DOMAIN_SERVICE
        MapSERVICENAMEEndpoints(app.MapGroup("/SlugSERVICENAME").WithTags("SERVICENAME"));
        #endregion foreach DOMAIN_SERVICE
        return app;
    }

    #region foreach MODEL[AG][R]
    private static void MapCLASSNAMEEndpoints(RouteGroupBuilder group)
    {
        #region to remove if NOT_GETBYQUERY
        group.MapGet("/", async (int? page, int? pageSize, string? orderBy, bool? descending, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.QueryAsync(HttpResults.ToPageRequest(page, pageSize, orderBy, descending), cancellationToken)).ToHttpResult())
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion to remove if NOT_GETBYQUERY

        #region to remove if NOT_GETBYID
        group.MapGet("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.GetAsync(id, cancellationToken)).ToHttpResult())
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion to remove if NOT_GETBYID

        #region to remove if NOT_ADD
        group.MapPost("/", async (CLASSNAMEInput input, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.AddAsync(input, cancellationToken)).ToCreatedHttpResult(dto => $"/Slug_CLASSNAMES_/{dto.Id}"))
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion to remove if NOT_ADD

        #region to remove if NOT_UPDATE
        group.MapPut("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEInput input, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.UpdateAsync(id, input, cancellationToken)).ToHttpResult())
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion to remove if NOT_UPDATE

        #region to remove if NOT_REMOVE
        group.MapDelete("/{id}", async (_IDENTITY_KEY_TYPE_ id, CLASSNAMEService service, CancellationToken cancellationToken) =>
            (await service.RemoveAsync(id, cancellationToken)).ToHttpResult())
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion to remove if NOT_REMOVE
    }

    #endregion foreach MODEL
    #region foreach DOMAIN_SERVICE
    private static void MapSERVICENAMEEndpoints(RouteGroupBuilder group)
    {
        #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN[EXT]
        group.Map_VERB_("/_DOVOIDACTIONURL_", async (/*PARAMETER_DEF_API*//*COMMA*/ISERVICENAMEService service, CancellationToken cancellationToken) =>
            (await service._DOVOIDACTION_Async(/*PARAMETERS*//*COMMA*/cancellationToken)).ToHttpResult())
            .ValidateBody()
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN[EXT]
        group.Map_VERB_("/_DOIDENTITYACTIONURL_", async (/*PARAMETER_DEF_API*//*COMMA*/ISERVICENAMEService service, CancellationToken cancellationToken) =>
            (await service._DOIDENTITYACTION_Async(/*PARAMETERS*//*COMMA*/cancellationToken)).ToHttpResult())
            .ValidateBody()
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN[EXT]
        group.Map_VERB_("/_DOSIMPLEACTIONURL_", async (/*PARAMETER_DEF_API*//*COMMA*/ISERVICENAMEService service, CancellationToken cancellationToken) =>
            (await service._DOSIMPLEACTION_Async(/*PARAMETERS*//*COMMA*/cancellationToken)).ToHttpResult())
            .ValidateBody()
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN[EXT]
        group.Map_VERB_("/_DOMODELACTIONURL_", async (/*PARAMETER_DEF_API*//*COMMA*/ISERVICENAMEService service, CancellationToken cancellationToken) =>
            (await service._DOMODELACTION_Async(/*PARAMETERS*//*COMMA*/cancellationToken)).ToHttpResult())
            .ValidateBody()
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN[EXT]
        group.Map_VERB_("/_DOMODELLISTACTIONURL_", async (/*PARAMETER_DEF_API*//*COMMA*/ISERVICENAMEService service, CancellationToken cancellationToken) =>
            (await service._DOMODELLISTACTION_Async(/*PARAMETERS*//*COMMA*/cancellationToken)).ToHttpResult())
            .ValidateBody()
            #region to remove if ANONYMOUS
            .RequireRight("MANDATORYRIGHT")
            #endregion to remove if ANONYMOUS
            #region to remove if NOT_ANONYMOUS
            .AllowAnonymous()
            #endregion to remove if NOT_ANONYMOUS
            ;
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
    }

    #endregion foreach DOMAIN_SERVICE
}
