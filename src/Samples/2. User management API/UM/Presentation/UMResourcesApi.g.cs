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
using SolidOps.UM.Domain.Repositories;
using SolidOps.UM.Domain.Queries;
using SolidOps.UM.Presentation.Mappers;
using System.Diagnostics;
namespace SolidOps.UM.Presentation.Controllers
{
    // Object [R][!CACHE]
    [Route("um/user")]
    public partial class UserController : BaseController
    {
        private readonly IUserRepository repository;
        private readonly ILogger<UserController> logger;
        private readonly IOutputSerializer serializer;
        public UserController(IUserRepository repository
            , IServiceProvider serviceProvider
            , IExecutionContext executionContext
            , ILoggerFactory loggerFactory
            , IExtendedConfiguration configuration
            , IOutputSerializer serializer
            ) : base(executionContext, configuration, serviceProvider)
        {
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<UserController>();
            this.serializer = serializer;
        }

        // GET: api/User
        [HttpGet(Name = "GetUMUsers")]

        public async Task<IActionResult> GetList(
            [FromQuery] string filter = null,
            [FromQuery] string orderBy = null,
            [FromQuery] string includes = null,
            [FromQuery] int? maxResults = null,
            [FromQuery] string continuationId = null)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("");

            IEnumerable<Contracts.DTO.UserDTO> result;
            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserController GetList", UnitOfWorkType.Read))
            {
                var listFilter = new UserQueryFilter(filter, orderBy, maxResults, continuationId);
                var entities = await this.repository.GetList(listFilter, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetByQuery(ref preventLazyLoading);
                result = new UserDTOMapper(serviceProvider).Convert(entities, preventLazyLoading, string.Empty, new Dictionary<string, Dictionary<string, object>>());
                if (listFilter != null && !string.IsNullOrEmpty(listFilter.ContinuationId))
                    HttpContext.Response.Headers.Add("ContinuationId", listFilter.ContinuationId);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetByQuery(ref List<string> preventLazyLoading);

        // GET: api/User/{id}
        [HttpGet("{id}", Name = "GetUMUser")]

        public async Task<IActionResult> GetSingleById(Guid id, [FromQuery] string includes = null)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("");

            Contracts.DTO.UserDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserController GetSingleById", UnitOfWorkType.Read))
            {
                var entity = await this.repository.GetSingleById(id, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetById(ref preventLazyLoading);
                result = new UserDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetById(ref List<string> preventLazyLoading);

        // POST: api/User
        [HttpPost(Name = "PostUMUser")]

        public async Task<IActionResult> Post([FromBody] Contracts.DTO.UserWriteDTO value)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("ManageUsers");

            Contracts.DTO.UserDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserController Post", UnitOfWorkType.Write))
            {
                var newEntity = new UserDTOMapper(serviceProvider).Convert(value, serviceProvider);
                var addResult = await this.repository.Add(newEntity);
                if (addResult.HasError) return Failure(addResult.Error);
                var entity = await this.repository.GetSingleById(addResult.Data, null, true);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForPost(ref preventLazyLoading);
                result = new UserDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            HttpContext.Response.Headers.Add("Location", result.Id.ToString());
            return Ok();
        }
        partial void SetPreventLazyLoadingForPost(ref List<string> preventLazyLoading);

        // PUT: api/User/{id}
        [HttpPut("{id}", Name = "PutUMUser")]

        public async Task<IActionResult> Put(Guid id, [FromBody] Contracts.DTO.UserWriteDTO value)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("ManageUsers");

            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserController Put", UnitOfWorkType.Write))
            {
                var updatedEntity = new UserDTOMapper(serviceProvider).Convert(value, serviceProvider);
                updatedEntity.SetId(id);
                var result = await this.repository.Update(updatedEntity);
                if (result.HasError) return Failure(result.Error);
                unitOfWork.Complete();
            }
            return Ok();
        }
        // PATCH: api/User/{id}
        [HttpPatch("{id}", Name = "PatchUMUser")]

        public async Task<IActionResult> Patch(Guid id, [FromBody] Contracts.DTO.UserPatchDTO value)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("ManageUsers");

            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "UserController Put", UnitOfWorkType.Write))
            {
                var entity = await this.repository.GetSingleById(id);
                if (new UserDTOMapper(serviceProvider).Patch(entity, value, serviceProvider))
                {
                    var result = await this.repository.Update(entity);
                    if (result.HasError) return Failure(result.Error);
                    entity = await this.repository.GetSingleById(id, null, true);
                }
                unitOfWork.Complete();
            }
            return Ok();
        }

    }

    [Route("um/invite")]
    public partial class InviteController : BaseController
    {
        private readonly IInviteRepository repository;
        private readonly ILogger<InviteController> logger;
        private readonly IOutputSerializer serializer;
        public InviteController(IInviteRepository repository
            , IServiceProvider serviceProvider
            , IExecutionContext executionContext
            , ILoggerFactory loggerFactory
            , IExtendedConfiguration configuration
            , IOutputSerializer serializer
            ) : base(executionContext, configuration, serviceProvider)
        {
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<InviteController>();
            this.serializer = serializer;
        }

        // GET: api/Invite
        [HttpGet(Name = "GetUMInvites")]

        public async Task<IActionResult> GetList(
            [FromQuery] string filter = null,
            [FromQuery] string orderBy = null,
            [FromQuery] string includes = null,
            [FromQuery] int? maxResults = null,
            [FromQuery] string continuationId = null)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("");

            IEnumerable<Contracts.DTO.InviteDTO> result;
            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteController GetList", UnitOfWorkType.Read))
            {
                var listFilter = new InviteQueryFilter(filter, orderBy, maxResults, continuationId);
                var entities = await this.repository.GetList(listFilter, includes);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForGetByQuery(ref preventLazyLoading);
                result = new InviteDTOMapper(serviceProvider).Convert(entities, preventLazyLoading, string.Empty, new Dictionary<string, Dictionary<string, object>>());
                if (listFilter != null && !string.IsNullOrEmpty(listFilter.ContinuationId))
                    HttpContext.Response.Headers.Add("ContinuationId", listFilter.ContinuationId);
                unitOfWork.Complete();
            }
            return Content(serializer.Serialize(result), new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }
        partial void SetPreventLazyLoadingForGetByQuery(ref List<string> preventLazyLoading);

        // POST: api/Invite
        [HttpPost(Name = "PostUMInvite")]

        public async Task<IActionResult> Post([FromBody] Contracts.DTO.InviteWriteDTO value)
        {

            executionContext.MandatoryRights.Add("");
            executionContext.OwnershipOverrideRights.Add("");

            Contracts.DTO.InviteDTO result;
            using (var unitOfWork = executionContext.StartUnitOfWork("UM", "InviteController Post", UnitOfWorkType.Write))
            {
                var newEntity = new InviteDTOMapper(serviceProvider).Convert(value, serviceProvider);
                var addResult = await this.repository.Add(newEntity);
                if (addResult.HasError) return Failure(addResult.Error);
                var entity = await this.repository.GetSingleById(addResult.Data, null, true);
                List<string> preventLazyLoading = new();
                SetPreventLazyLoadingForPost(ref preventLazyLoading);
                result = new InviteDTOMapper(serviceProvider).Convert(entity, preventLazyLoading, string.Empty, null);
                unitOfWork.Complete();
            }
            HttpContext.Response.Headers.Add("Location", result.Id.ToString());
            return Ok();
        }
        partial void SetPreventLazyLoadingForPost(ref List<string> preventLazyLoading);

    }

}
namespace SolidOps.UM.API
{
    public partial class UMAPIServiceRegistrar : IServiceRegistrar
    {
        public int Priority => 2;
        public void ConfigureServices(IServiceCollection services, IExtendedConfiguration configuration)
        {
            // Object [EN][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.UserRightDTO, Domain.Entities.UserRight>, UserRightDTOMapper>();

            services.AddTransient<IDTOMapper<Contracts.DTO.RightDTO, Domain.Entities.Right>, RightDTOMapper>();

            // Object [AG][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.UserDTO, Domain.AggregateRoots.User>, UserDTOMapper>();

            services.AddTransient<IDTOMapper<Contracts.DTO.InviteDTO, Domain.AggregateRoots.Invite>, InviteDTOMapper>();

            // Object [TR][EXP]
            services.AddTransient<IDTOMapper<Contracts.DTO.InviteResultDTO, Domain.Transients.InviteResult>, InviteResultDTOMapper>();

            services.AddTransient<IDTOMapper<Contracts.DTO.LoginRequestDTO, Domain.Transients.LoginRequest>, LoginRequestDTOMapper>();

            services.AddTransient<IDTOMapper<Contracts.DTO.SelfUserCreationRequestDTO, Domain.Transients.SelfUserCreationRequest>, SelfUserCreationRequestDTOMapper>();

            services.AddTransient<IDTOMapper<Contracts.DTO.UserCreationInfoDTO, Domain.Transients.UserCreationInfo>, UserCreationInfoDTOMapper>();

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