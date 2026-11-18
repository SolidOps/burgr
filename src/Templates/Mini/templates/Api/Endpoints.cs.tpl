#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using _MODULE_NAMESPACE_.Contracts;
using _MODULE_NAMESPACE_.Infrastructure;

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

    // a unique index or a foreign key violated by a concurrent change
    private static async Task<bool> TrySaveAsync(_MODULE_DbContext db, CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    #region foreach MODEL[AG][R]
    private static void MapCLASSNAMEEndpoints(RouteGroupBuilder group)
    {
        #region to remove if NOT_GETBYQUERY
        group.MapGet("/", async (_MODULE_DbContext db, CancellationToken cancellationToken) =>
        {
            var entities = await db._CLASSNAMES_.AsNoTracking().ToListAsync(cancellationToken);
            return TypedResults.Ok(entities.Select(CLASSNAMEDto.From).ToList());
        });
        #endregion to remove if NOT_GETBYQUERY

        #region to remove if NOT_GETBYID
        group.MapGet("/{id}", async Task<Results<Ok<CLASSNAMEDto>, NotFound>> (_IDENTITY_KEY_TYPE_ id, _MODULE_DbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db._CLASSNAMES_.AsNoTracking()
                #region foreach PROPERTY[M][LNA][AG]
                .Include(x => x._PROPERTYNAME_)
                #endregion foreach PROPERTY
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity is null ? TypedResults.NotFound() : TypedResults.Ok(CLASSNAMEDto.From(entity));
        });
        #endregion to remove if NOT_GETBYID

        #region to remove if NOT_ADD
        group.MapPost("/", async Task<Results<Created<CLASSNAMEDto>, ValidationProblem, Conflict>> (CLASSNAMEInput input, _MODULE_DbContext db, CancellationToken cancellationToken) =>
        {
            var errors = await ValidateCLASSNAMEAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }

            var entity = new Domain.CLASSNAME();
            input.ApplyTo(entity);
            db._CLASSNAMES_.Add(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Created($"/Slug_CLASSNAMES_/{entity.Id}", CLASSNAMEDto.From(entity));
        });
        #endregion to remove if NOT_ADD

        #region to remove if NOT_UPDATE
        group.MapPut("/{id}", async Task<Results<Ok<CLASSNAMEDto>, NotFound, ValidationProblem, Conflict>> (_IDENTITY_KEY_TYPE_ id, CLASSNAMEInput input, _MODULE_DbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db._CLASSNAMES_.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            var errors = await ValidateCLASSNAMEAsync(input, db, cancellationToken);
            if (errors.Count > 0)
            {
                return TypedResults.ValidationProblem(errors);
            }

            input.ApplyTo(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.Ok(CLASSNAMEDto.From(entity));
        });
        #endregion to remove if NOT_UPDATE

        #region to remove if NOT_REMOVE
        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound, Conflict>> (_IDENTITY_KEY_TYPE_ id, _MODULE_DbContext db, CancellationToken cancellationToken) =>
        {
            var entity = await db._CLASSNAMES_.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            db._CLASSNAMES_.Remove(entity);
            if (!await TrySaveAsync(db, cancellationToken))
            {
                return TypedResults.Conflict();
            }
            return TypedResults.NoContent();
        });
        #endregion to remove if NOT_REMOVE
    }

    private static async Task<Dictionary<string, string[]>> ValidateCLASSNAMEAsync(CLASSNAMEInput input, _MODULE_DbContext db, CancellationToken cancellationToken)
    {
        var errors = input.Validate();
        #region foreach PROPERTY[M][NO][NAR][AG][PUO][NRO][NN]
        if (!await db._PROPERTYTYPES_.AnyAsync(x => x.Id == input._SIMPLE__PROPERTYNAME_, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][PUO][NRO][N]
        if (input._SIMPLE__PROPERTYNAME_ != null && !await db._PROPERTYTYPES_.AnyAsync(x => x.Id == input._SIMPLE__PROPERTYNAME_, cancellationToken))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_PROPERTYTYPE_ not found"];
        }
        #endregion foreach PROPERTY
        return errors;
    }

    #endregion foreach MODEL
}
