using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Presentation;
using SolidOps.UM.Shared.Presentation.ETag;
using SolidOps.UM.Shared.Presentation.ETag.Filters;
using SolidOps.UM.Shared.Application.Events;
using MetaCorp.Template.Domain.Repositories;
using MetaCorp.Template.Domain.Queries;
using MetaCorp.Template.Presentation.Mappers;
using System.Diagnostics;

namespace MetaCorp.Template.Presentation.Controllers
{
    #region foreach MODEL[R][!CACHE]
    [Route("SlugTemplate/SlugCLASSNAME")]
    public partial class CLASSNAMEController : BaseController
    {
        private readonly ICLASSNAMERepository repository;
        private readonly ILogger<CLASSNAMEController> logger;
        private readonly IOutputSerializer serializer;

        public CLASSNAMEController(ICLASSNAMERepository repository
            , IServiceProvider serviceProvider
            , IExecutionContext executionContext
            , ILoggerFactory loggerFactory
            , IExtendedConfiguration configuration
            , IOutputSerializer serializer
            ) : base(executionContext, configuration, serviceProvider)
        {
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<CLASSNAMEController>();
            this.serializer = serializer;
        }

        #region to remove if NOT_GETBYQUERY
        // GET: api/CLASSNAME
        [HttpGet(Name = "GetTemplateCLASSNAMEs")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> GetList(
            [FromQuery] string filter = null,
            [FromQuery] string orderBy = null,
            [FromQuery] string includes = null,
            [FromQuery] int? maxResults = null,
            [FromQuery] string continuationId = null)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            IEnumerable<Contracts.DTO.CLASSNAMEDTO> result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController GetList", UnitOfWorkType.Read))
            {
                var listFilter = new CLASSNAMEQueryFilter(filter, orderBy, maxResults, continuationId);
                var entities = await this.repository.GetList(listFilter, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetByQuery(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entities, preventLazyLoading, string.Empty, new Dictionary<string, Dictionary<string, object>>());
                if (listFilter != null && !string.IsNullOrEmpty(listFilter.ContinuationId))
                    HttpContext.Response.Headers.Add("ContinuationId", listFilter.ContinuationId);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetByQuery(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_GETBYQUERY

        #region to remove if NOT_GETBYID
        // GET: api/CLASSNAME/{id}
        [HttpGet("{id}", Name = "GetTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> GetSingleById(_IDENTITY_KEY_TYPE_ id, [FromQuery] string includes = null)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            Contracts.DTO.CLASSNAMEDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController GetSingleById", UnitOfWorkType.Read))
            {
                var entity = await this.repository.GetSingleById(id, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetById(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetById(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_GETBYID

        #region to remove if NOT_ADD
        // POST: api/CLASSNAME
        [HttpPost(Name = "PostTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Post([FromBody] Contracts.DTO.CLASSNAMEWriteDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            Contracts.DTO.CLASSNAMEDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Post", UnitOfWorkType.Write))
            {
                var newEntity = new CLASSNAMEDTOMapper(serviceProvider).Convert(value, serviceProvider);
                var addResult = await this.repository.Add(newEntity);
                if (addResult.HasError) return Failure(addResult.Error);

                var entity = await this.repository.GetSingleById(addResult.Data, null, true);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForPost(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            HttpContext.Response.Headers.Add("Location", result.Id.ToString());
            return Ok();
        }
        partial void SetPreventLazyLoadingForPost(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_ADD

        #region to remove if NOT_UPDATE
        // PUT: api/CLASSNAME/{id}
        [HttpPut("{id}", Name = "PutTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Put(_IDENTITY_KEY_TYPE_ id, [FromBody] Contracts.DTO.CLASSNAMEWriteDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Put", UnitOfWorkType.Write))
            {
                var updatedEntity = new CLASSNAMEDTOMapper(serviceProvider).Convert(value, serviceProvider);
                updatedEntity.SetId(id);
                var result = await this.repository.Update(updatedEntity);
                if (result.HasError) return Failure(result.Error);

                unitOfWork.Complete();
            }
            return Ok();
        }

        // PATCH: api/CLASSNAME/{id}
        [HttpPatch("{id}", Name = "PatchTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Patch(_IDENTITY_KEY_TYPE_ id, [FromBody] Contracts.DTO.CLASSNAMEPatchDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Put", UnitOfWorkType.Write))
            {
                var entity = await this.repository.GetSingleById(id);
                if (new CLASSNAMEDTOMapper(serviceProvider).Patch(entity, value, serviceProvider))
                {
                    var result = await this.repository.Update(entity);
                    if (result.HasError) return Failure(result.Error);

                    entity = await this.repository.GetSingleById(id, null, true);
                }
                unitOfWork.Complete();
            }
            return Ok();
        }
        #endregion to remove if NOT_UPDATE

        #region to remove if NOT_REMOVE

        // DELETE: api/CLASSNAME/{id}
        [HttpDelete("{id}", Name = "DeleteTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Delete(string id)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Delete", UnitOfWorkType.Write);
            var result = await this.repository.RemoveWithId(id);
            if (result.HasError) return Failure(result.Error);

            unitOfWork.Complete();
            return Ok();

        }

        #endregion to remove if NOT_REMOVE
    }
    #endregion foreach MODEL

    #region foreach MODEL[R][CACHE]
    [Route("SlugTemplate/SlugCLASSNAME")]
    public partial class CLASSNAMEController : BaseController
    {
        private readonly ICLASSNAMERepository repository;
        private readonly ILogger<CLASSNAMEController> logger;
        private readonly IOutputSerializer serializer;

        public CLASSNAMEController(ICLASSNAMERepository repository
            , IServiceProvider serviceProvider
            , IExecutionContext executionContext
            , ILoggerFactory loggerFactory
            , IExtendedConfiguration configuration
            , IOutputSerializer serializer
            ) : base(executionContext, configuration, serviceProvider)
        {
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<CLASSNAMEController>();
            this.serializer = serializer;
        }

        #region to remove if NOT_GETBYQUERY
        // GET: api/CLASSNAME/{id}
        [HttpGet(Name = "GetTemplateCLASSNAMEs")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        [ServiceFilter(typeof(GetListETagFilter<Domain._DOMAINTYPE_.CLASSNAME, _IDENTITY_KEY_TYPE_>))]
        public async Task<IActionResult> GetList(
            [FromQuery] string filter = null,
            [FromQuery] string orderBy = null,
            [FromQuery] string includes = null,
            [FromQuery] int? maxResults = null,
            [FromQuery] string continuationId = null)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            IEnumerable<Contracts.DTO.CLASSNAMEDTO> result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController GetList", UnitOfWorkType.Read))
            {
                var listFilter = new CLASSNAMEQueryFilter(filter, orderBy, maxResults, continuationId);
                var entities = await this.repository.GetList(listFilter, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetByQuery(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entities, preventLazyLoading, string.Empty, new Dictionary<string, Dictionary<string, object>>());
                if (listFilter != null && !string.IsNullOrEmpty(listFilter.ContinuationId))
                    HttpContext.Response.Headers.Add("ContinuationId", listFilter.ContinuationId);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetByQuery(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_GETBYQUERY

        #region to remove if NOT_GETBYID
        // GET: api/CLASSNAME/{id}
        [HttpGet("{id}", Name = "GetTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        [ServiceFilter(typeof(GetByIdETagFilter<Domain._DOMAINTYPE_.CLASSNAME, _IDENTITY_KEY_TYPE_>))]
        public async Task<IActionResult> GetSingleById(_IDENTITY_KEY_TYPE_ id, [FromQuery] string includes = null)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            Contracts.DTO.CLASSNAMEDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController GetSingleById", UnitOfWorkType.Read))
            {
                var entity = await this.repository.GetSingleById(id, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetById(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetById(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_GETBYID

        #region to remove if NOT_ADD
        // POST: api/CLASSNAME
        [HttpPost(Name = "PostTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Post([FromBody] Contracts.DTO.CLASSNAMEWriteDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            Contracts.DTO.CLASSNAMEDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Post", UnitOfWorkType.Write))
            {
                var newEntity = new CLASSNAMEDTOMapper(serviceProvider).Convert(value, serviceProvider);
                var addResult = await this.repository.Add(newEntity);
                if (addResult.HasError) return Failure(addResult.Error);

                var entity = await this.repository.GetSingleById(addResult.Data, null, true);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForPost(ref preventLazyLoading);
                result = new CLASSNAMEDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            HttpContext.Response.Headers.Add("Location", result.Id.ToString());
            return Ok();
        }
        partial void SetPreventLazyLoadingForPost(ref List<string> preventLazyLoading);
        #endregion to remove if NOT_ADD

        #region to remove if NOT_UPDATE
        // PUT: api/CLASSNAME/{id}
        [HttpPut("{id}", Name = "PutTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Put(_IDENTITY_KEY_TYPE_ id, [FromBody] Contracts.DTO.CLASSNAMEWriteDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Put", UnitOfWorkType.Write))
            {
                var updatedEntity = new CLASSNAMEDTOMapper(serviceProvider).Convert(value, serviceProvider);
                updatedEntity.SetId(id);
                var result = await this.repository.Update(updatedEntity);
                if (result.HasError) return Failure(result.Error);

                unitOfWork.Complete();
            }
            return Ok();
        }

        // PATCH: api/CLASSNAME/{id}
        [HttpPatch("{id}", Name = "PatchTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Patch(_IDENTITY_KEY_TYPE_ id, [FromBody] Contracts.DTO.CLASSNAMEPatchDTO value)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using (var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Put", UnitOfWorkType.Write))
            {
                var entity = await this.repository.GetSingleById(id);
                if (new CLASSNAMEDTOMapper(serviceProvider).Patch(entity, value, serviceProvider))
                {
                    var result = await this.repository.Update(entity);
                    if (result.HasError) return Failure(result.Error);

                    entity = await this.repository.GetSingleById(id, null, true);
                }
                unitOfWork.Complete();
            }
            return Ok();
        }
        #endregion to remove if NOT_UPDATE

        #region to remove if NOT_REMOVE

        // DELETE: api/CLASSNAME/{id}
        [HttpDelete("{id}", Name = "DeleteTemplateCLASSNAME")]
        #region to remove if NOT_ANONYMOUS
        [AllowAnonymous]
        #endregion to remove if NOT_ANONYMOUS
        public async Task<IActionResult> Delete(string id)
        {
            #region to remove if ANONYMOUS
            executionContext.MandatoryRights.Add("MANDATORYRIGHT");
            executionContext.OwnershipOverrideRights.Add("OWNERSHIPOVERRIDERIGHT");
            #endregion to remove if ANONYMOUS
            using var unitOfWork = executionContext.StartUnitOfWork("Template", "CLASSNAMEController Delete", UnitOfWorkType.Write);
            var result = await this.repository.RemoveWithId(id);
            if (result.HasError) return Failure(result.Error);

            unitOfWork.Complete();
            return Ok();
        }

        #endregion to remove if NOT_REMOVE
    }
    #endregion foreach MODEL
}

namespace MetaCorp.Template.API
{
    public partial class TemplateAPIServiceRegistrar : IServiceRegistrar
    {
        public int Priority => 2;
        public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
        {
            #region foreach MODEL[EN][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.CLASSNAMEDTO, Domain.Entities.CLASSNAME>, CLASSNAMEDTOMapper>();
            #endregion foreach MODEL

            #region foreach MODEL[AG][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.CLASSNAMEDTO, Domain.AggregateRoots.CLASSNAME>, CLASSNAMEDTOMapper>();
            #endregion foreach MODEL

            #region foreach MODEL[TR][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.CLASSNAMEDTO, Domain.Transients.CLASSNAME>, CLASSNAMEDTOMapper>();
            #endregion foreach MODEL

            // cache
            services.AddScoped(typeof(IEntityEventHandler<,,>), typeof(ETagsEntityEventHandler<,,>));
            services.AddSingleton<ETagConfiguration>();
            services.AddScoped(typeof(ETagFilter<,>));
            services.AddScoped(typeof(GetListETagFilter<,>));
            services.AddScoped(typeof(GetByIdETagFilter<,>));
            services.AddScoped(typeof(ConstantETagFilter<>));
            services.AddSingleton(typeof(IETagRepository<,>), typeof(ETagRepository<,>));

            ConfigureAdditionalServices(services, configuration);
        }

        partial void ConfigureAdditionalServices(IServiceCollection services, IExtendedConfiguration configuration);
    }
}


