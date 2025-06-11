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
namespace SolidOps.UM.Presentation.Mappers
{
    // Object [EN][EXP]
    public partial class UserRightDTOMapper : IDTOMapper<Contracts.DTO.UserRightDTO, Domain.Entities.UserRight>
    {
        private readonly IServiceProvider serviceProvider;
        public UserRightDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.UserRightDTO dto);
        public void Initialize(Contracts.DTO.UserRightDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.UserRightWriteDTO source, Domain.Entities.UserRight target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForEntity(Contracts.DTO.UserRightDTO source, Domain.Entities.UserRight target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForDTO(Domain.Entities.UserRight source, Contracts.DTO.UserRightDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Entities.UserRight);
        }
        public object ConvertExtension(Domain.Entities.UserRight entity)
        {
            return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
        }
        public Contracts.DTO.UserRightDTO Convert(Domain.Entities.UserRight entity, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(entity, null, "", references);
        }
        public Contracts.DTO.UserRightDTO Convert(Domain.Entities.UserRight entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            if (references == null)
                references = new Dictionary<string, Dictionary<string, object>>();
            if (entity.ComposedId == null)
            {
                entity.SetId(entity.Id);
            }
            preventLazyLoading.Add("UserRight|" + entity.ComposedId);
            Contracts.DTO.UserRightDTO dto = new();

            dto.Id = entity.ComposedId;

            if (!references.ContainsKey("UserRight"))
            {
                references.Add("UserRight", new Dictionary<string, object>());
            }
            if (references["UserRight"].ContainsKey(dto.Id))
            {
                return references["UserRight"][dto.Id] as Contracts.DTO.UserRightDTO;
            }
            references["UserRight"].Add(dto.Id, dto);
            // Get Calculated values

            // Property [M][NO][PUO][EN][AG]
            dto.UserId = entity.UserId.ToString();

            dto.RightId = entity.RightId.ToString();

            // Property [M][NO][NP][CA][PUO][EN][AG][TR][NAR]
            if (entity.User != null && !preventLazyLoading.Contains(parentPath + "User"))
            {
                if (references.ContainsKey("User") && references["User"].ContainsKey(entity.User.ComposedId))
                {
                    dto.User = references["User"][entity.User.ComposedId] as SolidOps.UM.Contracts.DTO.UserDTO;
                }
                else
                {
                    dto.User = new UserDTOMapper(serviceProvider).Convert(entity.User, preventLazyLoading, parentPath + "User" + ".", references);
                }
            }

            if (entity.Right != null && !preventLazyLoading.Contains(parentPath + "Right"))
            {
                if (references.ContainsKey("Right") && references["Right"].ContainsKey(entity.Right.ComposedId))
                {
                    dto.Right = references["Right"][entity.Right.ComposedId] as SolidOps.UM.Contracts.DTO.RightDTO;
                }
                else
                {
                    dto.Right = new RightDTOMapper(serviceProvider).Convert(entity.Right, preventLazyLoading, parentPath + "Right" + ".", references);
                }
            }

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Entities.UserRight Convert(Contracts.DTO.UserRightWriteDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Entities.UserRight entity = Domain.Entities.UserRight.CreateEmpty();

            // Property [M][R][NO][PUO][EN][AG]
            entity.UserId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.UserId);

            entity.RightId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.RightId);

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.User != null)
                entity.User = new UserDTOMapper(serviceProvider).Convert(dataTransferObject.User, serviceProvider);

            // navigation
            if (dataTransferObject.Right != null)
                entity.Right = new RightDTOMapper(serviceProvider).Convert(dataTransferObject.Right, serviceProvider);
            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public bool Patch(Domain.Entities.UserRight entity, Contracts.DTO.UserRightPatchDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return false;
            bool patched = false;

            // Property [M][R][NO][PUO][EN][AG]
            if (!string.IsNullOrEmpty(dataTransferObject.UserId))
            {
                var id = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.UserId);
                if (!entity.UserId.Equals(id))
                {
                    patched = true;
                    entity.UserId = id;
                }
            }

            if (!string.IsNullOrEmpty(dataTransferObject.RightId))
            {
                var id = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.RightId);
                if (!entity.RightId.Equals(id))
                {
                    patched = true;
                    entity.RightId = id;
                }
            }

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.User != null)
            {
                if (new UserDTOMapper(serviceProvider).Patch(entity.User, dataTransferObject.User, serviceProvider))
                {
                    patched = true;
                }
            }

            // navigation
            if (dataTransferObject.Right != null)
            {
                if (new RightDTOMapper(serviceProvider).Patch(entity.Right, dataTransferObject.Right, serviceProvider))
                {
                    patched = true;
                }
            }

            return patched;
        }
        public Domain.Entities.UserRight Convert(Contracts.DTO.UserRightDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Entities.UserRight entity = Domain.Entities.UserRight.CreateEmpty();

            if (dataTransferObject.Id != default)
                entity.SetId(dataTransferObject.Id);

            // Property [M][R][NO][PUO][EN][AG]
            entity.UserId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.UserId);

            entity.RightId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.RightId);

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.User != null)
                entity.User = new UserDTOMapper(serviceProvider).Convert(dataTransferObject.User, serviceProvider);

            // navigation
            if (dataTransferObject.Right != null)
                entity.Right = new RightDTOMapper(serviceProvider).Convert(dataTransferObject.Right, serviceProvider);

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public List<Contracts.DTO.UserRightDTO> Convert(List<Domain.Entities.UserRight> list, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(list, null, "", references);
        }
        public List<Contracts.DTO.UserRightDTO> Convert(List<Domain.Entities.UserRight> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.UserRightDTO> dtos = new();
            foreach (var entity in list)
            {
                if (!preventLazyLoading.Contains("UserRight|" + entity.ComposedId))
                {
                    var dto = Convert(entity, preventLazyLoading, parentPath, references);
                    if (dto != null)
                        dtos.Add(dto);
                }
            }
            return dtos;
        }
        public List<Domain.Entities.UserRight> Convert(List<Contracts.DTO.UserRightWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.Entities.UserRight> entities, List<Contracts.DTO.UserRightPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.Entities.UserRight> Convert(List<Contracts.DTO.UserRightDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.UserRightDTO> Convert(IEnumerable<Domain.Entities.UserRight> collection, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(collection, null, "", references);
        }
        public List<Contracts.DTO.UserRightDTO> Convert(IEnumerable<Domain.Entities.UserRight> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.UserRightDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Entities.UserRight> Convert(IEnumerable<Contracts.DTO.UserRightWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.Entities.UserRight> entities, IEnumerable<Contracts.DTO.UserRightPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.Entities.UserRight> Convert(IEnumerable<Contracts.DTO.UserRightDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.UserRight> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    public partial class RightDTOMapper : IDTOMapper<Contracts.DTO.RightDTO, Domain.Entities.Right>
    {
        private readonly IServiceProvider serviceProvider;
        public RightDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.RightDTO dto);
        public void Initialize(Contracts.DTO.RightDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.RightWriteDTO source, Domain.Entities.Right target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForEntity(Contracts.DTO.RightDTO source, Domain.Entities.Right target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForDTO(Domain.Entities.Right source, Contracts.DTO.RightDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Entities.Right);
        }
        public object ConvertExtension(Domain.Entities.Right entity)
        {
            return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
        }
        public Contracts.DTO.RightDTO Convert(Domain.Entities.Right entity, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(entity, null, "", references);
        }
        public Contracts.DTO.RightDTO Convert(Domain.Entities.Right entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            if (references == null)
                references = new Dictionary<string, Dictionary<string, object>>();
            if (entity.ComposedId == null)
            {
                entity.SetId(entity.Id);
            }
            preventLazyLoading.Add("Right|" + entity.ComposedId);
            Contracts.DTO.RightDTO dto = new();

            dto.Id = entity.ComposedId;

            if (!references.ContainsKey("Right"))
            {
                references.Add("Right", new Dictionary<string, object>());
            }
            if (references["Right"].ContainsKey(dto.Id))
            {
                return references["Right"][dto.Id] as Contracts.DTO.RightDTO;
            }
            references["Right"].Add(dto.Id, dto);
            // Get Calculated values

            // Property [S][NO][NP][CA][PUO]
            dto.Name = entity.Name;

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Entities.Right Convert(Contracts.DTO.RightWriteDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Entities.Right entity = Domain.Entities.Right.CreateEmpty();
            // Property [S][NO][PUO]
            entity.Name = dataTransferObject.Name;

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public bool Patch(Domain.Entities.Right entity, Contracts.DTO.RightPatchDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return false;
            bool patched = false;
            // Property [S][NO][PUO]
            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.Name, entity.Name))
            {
                patched = true;
                entity.Name = dataTransferObject.Name;
            }

            return patched;
        }
        public Domain.Entities.Right Convert(Contracts.DTO.RightDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Entities.Right entity = Domain.Entities.Right.CreateEmpty();

            if (dataTransferObject.Id != default)
                entity.SetId(dataTransferObject.Id);

            // Property [S][NO][PUO]
            entity.Name = dataTransferObject.Name;

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public List<Contracts.DTO.RightDTO> Convert(List<Domain.Entities.Right> list, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(list, null, "", references);
        }
        public List<Contracts.DTO.RightDTO> Convert(List<Domain.Entities.Right> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.RightDTO> dtos = new();
            foreach (var entity in list)
            {
                if (!preventLazyLoading.Contains("Right|" + entity.ComposedId))
                {
                    var dto = Convert(entity, preventLazyLoading, parentPath, references);
                    if (dto != null)
                        dtos.Add(dto);
                }
            }
            return dtos;
        }
        public List<Domain.Entities.Right> Convert(List<Contracts.DTO.RightWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.Entities.Right> entities, List<Contracts.DTO.RightPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.Entities.Right> Convert(List<Contracts.DTO.RightDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.RightDTO> Convert(IEnumerable<Domain.Entities.Right> collection, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(collection, null, "", references);
        }
        public List<Contracts.DTO.RightDTO> Convert(IEnumerable<Domain.Entities.Right> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.RightDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Entities.Right> Convert(IEnumerable<Contracts.DTO.RightWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.Entities.Right> entities, IEnumerable<Contracts.DTO.RightPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.Entities.Right> Convert(IEnumerable<Contracts.DTO.RightDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Entities.Right> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    // Object [AG][EXP]
    public partial class UserDTOMapper : IDTOMapper<Contracts.DTO.UserDTO, Domain.AggregateRoots.User>
    {
        private readonly IServiceProvider serviceProvider;
        public UserDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.UserDTO dto);
        public void Initialize(Contracts.DTO.UserDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.UserWriteDTO source, Domain.AggregateRoots.User target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForEntity(Contracts.DTO.UserDTO source, Domain.AggregateRoots.User target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForDTO(Domain.AggregateRoots.User source, Contracts.DTO.UserDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.AggregateRoots.User);
        }
        public object ConvertExtension(Domain.AggregateRoots.User entity)
        {
            return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
        }
        public Contracts.DTO.UserDTO Convert(Domain.AggregateRoots.User entity, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(entity, null, "", references);
        }
        public Contracts.DTO.UserDTO Convert(Domain.AggregateRoots.User entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            if (references == null)
                references = new Dictionary<string, Dictionary<string, object>>();
            if (entity.ComposedId == null)
            {
                entity.SetId(entity.Id);
            }
            preventLazyLoading.Add("User|" + entity.ComposedId);
            Contracts.DTO.UserDTO dto = new();

            dto.Id = entity.ComposedId;

            if (!references.ContainsKey("User"))
            {
                references.Add("User", new Dictionary<string, object>());
            }
            if (references["User"].ContainsKey(dto.Id))
            {
                return references["User"][dto.Id] as Contracts.DTO.UserDTO;
            }
            references["User"].Add(dto.Id, dto);
            // Get Calculated values
            // Property [S][CA]
            _ = entity.Rights;

            // Property [S][NO][NP][CA][PUO]
            dto.Email = entity.Email;

            dto.Provider = entity.Provider;

            dto.TechnicalUser = entity.TechnicalUser;

            dto.Rights = entity.Rights;

            // Property [M][NA][PUO][AR]
            if (entity.UserRights != null && !preventLazyLoading.Contains(parentPath + "UserRights"))
            {
                dto.UserRights = new UserRightDTOMapper(serviceProvider).Convert(entity.UserRights, preventLazyLoading, parentPath + "UserRights" + ".", references);
            }

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.AggregateRoots.User Convert(Contracts.DTO.UserWriteDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.AggregateRoots.User entity = Domain.AggregateRoots.User.CreateEmpty();
            // Property [S][NO][PUO]
            entity.Email = dataTransferObject.Email;

            entity.Provider = dataTransferObject.Provider;

            entity.TechnicalUser = dataTransferObject.TechnicalUser;

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.UserRights != null)
                entity.UserRights = new UserRightDTOMapper(serviceProvider).Convert(dataTransferObject.UserRights, serviceProvider);

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public bool Patch(Domain.AggregateRoots.User entity, Contracts.DTO.UserPatchDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return false;
            bool patched = false;
            // Property [S][NO][PUO]
            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.Email, entity.Email))
            {
                patched = true;
                entity.Email = dataTransferObject.Email;
            }

            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.Provider, entity.Provider))
            {
                patched = true;
                entity.Provider = dataTransferObject.Provider;
            }

            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.TechnicalUser, entity.TechnicalUser))
            {
                patched = true;
                entity.TechnicalUser = dataTransferObject.TechnicalUser.Value;
            }

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.UserRights != null)
            {
                if (new UserRightDTOMapper(serviceProvider).Patch(entity.UserRights, dataTransferObject.UserRights, serviceProvider))
                {
                    patched = true;
                }
            }

            return patched;
        }
        public Domain.AggregateRoots.User Convert(Contracts.DTO.UserDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.AggregateRoots.User entity = Domain.AggregateRoots.User.CreateEmpty();

            if (dataTransferObject.Id != default)
                entity.SetId(dataTransferObject.Id);

            // Property [S][NO][PUO]
            entity.Email = dataTransferObject.Email;

            entity.Provider = dataTransferObject.Provider;

            entity.TechnicalUser = dataTransferObject.TechnicalUser;

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.UserRights != null)
                entity.UserRights = new UserRightDTOMapper(serviceProvider).Convert(dataTransferObject.UserRights, serviceProvider);

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public List<Contracts.DTO.UserDTO> Convert(List<Domain.AggregateRoots.User> list, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(list, null, "", references);
        }
        public List<Contracts.DTO.UserDTO> Convert(List<Domain.AggregateRoots.User> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.UserDTO> dtos = new();
            foreach (var entity in list)
            {
                if (!preventLazyLoading.Contains("User|" + entity.ComposedId))
                {
                    var dto = Convert(entity, preventLazyLoading, parentPath, references);
                    if (dto != null)
                        dtos.Add(dto);
                }
            }
            return dtos;
        }
        public List<Domain.AggregateRoots.User> Convert(List<Contracts.DTO.UserWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.AggregateRoots.User> entities, List<Contracts.DTO.UserPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.AggregateRoots.User> Convert(List<Contracts.DTO.UserDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.UserDTO> Convert(IEnumerable<Domain.AggregateRoots.User> collection, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(collection, null, "", references);
        }
        public List<Contracts.DTO.UserDTO> Convert(IEnumerable<Domain.AggregateRoots.User> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.UserDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.AggregateRoots.User> Convert(IEnumerable<Contracts.DTO.UserWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.AggregateRoots.User> entities, IEnumerable<Contracts.DTO.UserPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.AggregateRoots.User> Convert(IEnumerable<Contracts.DTO.UserDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.User> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    public partial class InviteDTOMapper : IDTOMapper<Contracts.DTO.InviteDTO, Domain.AggregateRoots.Invite>
    {
        private readonly IServiceProvider serviceProvider;
        public InviteDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.InviteDTO dto);
        public void Initialize(Contracts.DTO.InviteDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.InviteWriteDTO source, Domain.AggregateRoots.Invite target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForEntity(Contracts.DTO.InviteDTO source, Domain.AggregateRoots.Invite target, IServiceProvider serviceProvider);
        partial void AdditionalConversionForDTO(Domain.AggregateRoots.Invite source, Contracts.DTO.InviteDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.AggregateRoots.Invite);
        }
        public object ConvertExtension(Domain.AggregateRoots.Invite entity)
        {
            return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
        }
        public Contracts.DTO.InviteDTO Convert(Domain.AggregateRoots.Invite entity, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(entity, null, "", references);
        }
        public Contracts.DTO.InviteDTO Convert(Domain.AggregateRoots.Invite entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            if (references == null)
                references = new Dictionary<string, Dictionary<string, object>>();
            if (entity.ComposedId == null)
            {
                entity.SetId(entity.Id);
            }
            preventLazyLoading.Add("Invite|" + entity.ComposedId);
            Contracts.DTO.InviteDTO dto = new();

            dto.Id = entity.ComposedId;

            if (!references.ContainsKey("Invite"))
            {
                references.Add("Invite", new Dictionary<string, object>());
            }
            if (references["Invite"].ContainsKey(dto.Id))
            {
                return references["Invite"][dto.Id] as Contracts.DTO.InviteDTO;
            }
            references["Invite"].Add(dto.Id, dto);
            // Get Calculated values

            // Property [S][NO][NP][CA][PUO]
            dto.Email = entity.Email;

            dto.CreatorName = entity.CreatorName;

            dto.CreatorMessage = entity.CreatorMessage;

            // Property [E][NO][NP][CA][PUO]
            dto.Status = DisplayConverter.GetEnumData<SolidOps.UM.Contracts.Enums.InviteStatusEnum, SolidOps.UM.Contracts.Enums.InviteStatusEnum, SolidOps.UM.Contracts.Enums.InviteStatusEnum>(entity.Status);

            // Property [M][NO][PUO][EN][AG]
            dto.CreatorId = entity.CreatorId.ToString();

            // Property [M][NO][NP][CA][PUO][EN][AG][TR][NAR]
            if (entity.Creator != null && !preventLazyLoading.Contains(parentPath + "Creator"))
            {
                if (references.ContainsKey("User") && references["User"].ContainsKey(entity.Creator.ComposedId))
                {
                    dto.Creator = references["User"][entity.Creator.ComposedId] as SolidOps.UM.Contracts.DTO.UserDTO;
                }
                else
                {
                    dto.Creator = new UserDTOMapper(serviceProvider).Convert(entity.Creator, preventLazyLoading, parentPath + "Creator" + ".", references);
                }
            }

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.AggregateRoots.Invite Convert(Contracts.DTO.InviteWriteDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.AggregateRoots.Invite entity = Domain.AggregateRoots.Invite.CreateEmpty();
            // Property [S][NO][PUO]
            entity.Email = dataTransferObject.Email;

            entity.CreatorName = dataTransferObject.CreatorName;

            entity.CreatorMessage = dataTransferObject.CreatorMessage;

            // Property [E][NO][PUO]
            entity.Status = DisplayConverter.GetEnumData<SolidOps.UM.Contracts.Enums.InviteStatusEnum, Contracts.Enums.InviteStatusEnum, SolidOps.UM.Contracts.Enums.InviteStatusEnum>(dataTransferObject.Status);

            // Property [M][R][NO][PUO][EN][AG]
            entity.CreatorId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.CreatorId);

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.Creator != null)
                entity.Creator = new UserDTOMapper(serviceProvider).Convert(dataTransferObject.Creator, serviceProvider);

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public bool Patch(Domain.AggregateRoots.Invite entity, Contracts.DTO.InvitePatchDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return false;
            bool patched = false;
            // Property [S][NO][PUO]
            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.Email, entity.Email))
            {
                patched = true;
                entity.Email = dataTransferObject.Email;
            }

            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.CreatorName, entity.CreatorName))
            {
                patched = true;
                entity.CreatorName = dataTransferObject.CreatorName;
            }

            if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject.CreatorMessage, entity.CreatorMessage))
            {
                patched = true;
                entity.CreatorMessage = dataTransferObject.CreatorMessage;
            }

            // Property [E][NO][PUO]
            if (!Equals(dataTransferObject.Status, default) && entity.Status != dataTransferObject.Status)
            {
                patched = true;
                entity.Status = DisplayConverter.GetEnumData<SolidOps.UM.Contracts.Enums.InviteStatusEnum, Contracts.Enums.InviteStatusEnum, SolidOps.UM.Contracts.Enums.InviteStatusEnum>(dataTransferObject.Status);
            }

            // Property [M][R][NO][PUO][EN][AG]
            if (!string.IsNullOrEmpty(dataTransferObject.CreatorId))
            {
                var id = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.CreatorId);
                if (!entity.CreatorId.Equals(id))
                {
                    patched = true;
                    entity.CreatorId = id;
                }
            }

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.Creator != null)
            {
                if (new UserDTOMapper(serviceProvider).Patch(entity.Creator, dataTransferObject.Creator, serviceProvider))
                {
                    patched = true;
                }
            }

            return patched;
        }
        public Domain.AggregateRoots.Invite Convert(Contracts.DTO.InviteDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.AggregateRoots.Invite entity = Domain.AggregateRoots.Invite.CreateEmpty();

            if (dataTransferObject.Id != default)
                entity.SetId(dataTransferObject.Id);

            // Property [S][NO][PUO]
            entity.Email = dataTransferObject.Email;

            entity.CreatorName = dataTransferObject.CreatorName;

            entity.CreatorMessage = dataTransferObject.CreatorMessage;

            // Property [E][NO][PUO]
            entity.Status = DisplayConverter.GetEnumData<SolidOps.UM.Contracts.Enums.InviteStatusEnum, Contracts.Enums.InviteStatusEnum, SolidOps.UM.Contracts.Enums.InviteStatusEnum>(dataTransferObject.Status);

            // Property [M][R][NO][PUO][EN][AG]
            entity.CreatorId = IdentityKeyHelper<Guid>.ReadString(dataTransferObject.CreatorId);

            // Property [M][NO][PUO][EN][AG][NA]
            // navigation
            if (dataTransferObject.Creator != null)
                entity.Creator = new UserDTOMapper(serviceProvider).Convert(dataTransferObject.Creator, serviceProvider);

            AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);
            return entity;
        }
        public List<Contracts.DTO.InviteDTO> Convert(List<Domain.AggregateRoots.Invite> list, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(list, null, "", references);
        }
        public List<Contracts.DTO.InviteDTO> Convert(List<Domain.AggregateRoots.Invite> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.InviteDTO> dtos = new();
            foreach (var entity in list)
            {
                if (!preventLazyLoading.Contains("Invite|" + entity.ComposedId))
                {
                    var dto = Convert(entity, preventLazyLoading, parentPath, references);
                    if (dto != null)
                        dtos.Add(dto);
                }
            }
            return dtos;
        }
        public List<Domain.AggregateRoots.Invite> Convert(List<Contracts.DTO.InviteWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.AggregateRoots.Invite> entities, List<Contracts.DTO.InvitePatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.AggregateRoots.Invite> Convert(List<Contracts.DTO.InviteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.InviteDTO> Convert(IEnumerable<Domain.AggregateRoots.Invite> collection, Dictionary<string, Dictionary<string, object>> references)
        {
            return Convert(collection, null, "", references);
        }
        public List<Contracts.DTO.InviteDTO> Convert(IEnumerable<Domain.AggregateRoots.Invite> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.InviteDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.AggregateRoots.Invite> Convert(IEnumerable<Contracts.DTO.InviteWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public bool Patch(List<Domain.AggregateRoots.Invite> entities, IEnumerable<Contracts.DTO.InvitePatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return false;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
            }
            return false;
        }
        public List<Domain.AggregateRoots.Invite> Convert(IEnumerable<Contracts.DTO.InviteDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.AggregateRoots.Invite> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    // Object [TR][EXP]
    public partial class InviteResultDTOMapper : IDTOMapper<Contracts.DTO.InviteResultDTO, Domain.Transients.InviteResult>
    {
        private readonly IServiceProvider serviceProvider;
        public InviteResultDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.InviteResultDTO dto);
        public void Initialize(Contracts.DTO.InviteResultDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.InviteResultDTO source, Domain.Transients.InviteResult target);
        partial void AdditionalConversionForDTO(Domain.Transients.InviteResult source, Contracts.DTO.InviteResultDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Transients.InviteResult);
        }
        public object ConvertExtension(Domain.Transients.InviteResult entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.InviteResultDTO Convert(Domain.Transients.InviteResult entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.InviteResultDTO Convert(Domain.Transients.InviteResult entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            // Get Calculated values

            Contracts.DTO.InviteResultDTO dto = new();
            // Property [S][NO][NP][CA][PUO]
            dto.Email = entity.Email;

            dto.Creator = entity.Creator;

            dto.Message = entity.Message;

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Transients.InviteResult Convert(Contracts.DTO.InviteResultDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Transients.InviteResult entity = new();
            // Property [S][NO][NP][PUO]
            entity.Email = dataTransferObject.Email;

            entity.Creator = dataTransferObject.Creator;

            entity.Message = dataTransferObject.Message;

            AdditionalConversionForEntity(dataTransferObject, entity);
            return entity;
        }
        public List<Contracts.DTO.InviteResultDTO> Convert(List<Domain.Transients.InviteResult> list)
        {
            return Convert(list, null, "", null);
        }
        public List<Contracts.DTO.InviteResultDTO> Convert(List<Domain.Transients.InviteResult> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.InviteResultDTO> dtos = new();
            foreach (var entity in list)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.InviteResult> Convert(List<Contracts.DTO.InviteResultDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.InviteResult> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.InviteResultDTO> Convert(IEnumerable<Domain.Transients.InviteResult> collection)
        {
            return Convert(collection, null, "", null);
        }
        public List<Contracts.DTO.InviteResultDTO> Convert(IEnumerable<Domain.Transients.InviteResult> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.InviteResultDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.InviteResult> Convert(IEnumerable<Contracts.DTO.InviteResultDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.InviteResult> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    public partial class LoginRequestDTOMapper : IDTOMapper<Contracts.DTO.LoginRequestDTO, Domain.Transients.LoginRequest>
    {
        private readonly IServiceProvider serviceProvider;
        public LoginRequestDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.LoginRequestDTO dto);
        public void Initialize(Contracts.DTO.LoginRequestDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.LoginRequestDTO source, Domain.Transients.LoginRequest target);
        partial void AdditionalConversionForDTO(Domain.Transients.LoginRequest source, Contracts.DTO.LoginRequestDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Transients.LoginRequest);
        }
        public object ConvertExtension(Domain.Transients.LoginRequest entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.LoginRequestDTO Convert(Domain.Transients.LoginRequest entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.LoginRequestDTO Convert(Domain.Transients.LoginRequest entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            // Get Calculated values

            Contracts.DTO.LoginRequestDTO dto = new();
            // Property [S][NO][NP][CA][PUO]
            dto.Login = entity.Login;

            dto.Password = entity.Password;

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Transients.LoginRequest Convert(Contracts.DTO.LoginRequestDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Transients.LoginRequest entity = new();
            // Property [S][NO][NP][PUO]
            entity.Login = dataTransferObject.Login;

            entity.Password = dataTransferObject.Password;

            AdditionalConversionForEntity(dataTransferObject, entity);
            return entity;
        }
        public List<Contracts.DTO.LoginRequestDTO> Convert(List<Domain.Transients.LoginRequest> list)
        {
            return Convert(list, null, "", null);
        }
        public List<Contracts.DTO.LoginRequestDTO> Convert(List<Domain.Transients.LoginRequest> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.LoginRequestDTO> dtos = new();
            foreach (var entity in list)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.LoginRequest> Convert(List<Contracts.DTO.LoginRequestDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.LoginRequest> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.LoginRequestDTO> Convert(IEnumerable<Domain.Transients.LoginRequest> collection)
        {
            return Convert(collection, null, "", null);
        }
        public List<Contracts.DTO.LoginRequestDTO> Convert(IEnumerable<Domain.Transients.LoginRequest> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.LoginRequestDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.LoginRequest> Convert(IEnumerable<Contracts.DTO.LoginRequestDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.LoginRequest> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    public partial class SelfUserCreationRequestDTOMapper : IDTOMapper<Contracts.DTO.SelfUserCreationRequestDTO, Domain.Transients.SelfUserCreationRequest>
    {
        private readonly IServiceProvider serviceProvider;
        public SelfUserCreationRequestDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.SelfUserCreationRequestDTO dto);
        public void Initialize(Contracts.DTO.SelfUserCreationRequestDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.SelfUserCreationRequestDTO source, Domain.Transients.SelfUserCreationRequest target);
        partial void AdditionalConversionForDTO(Domain.Transients.SelfUserCreationRequest source, Contracts.DTO.SelfUserCreationRequestDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Transients.SelfUserCreationRequest);
        }
        public object ConvertExtension(Domain.Transients.SelfUserCreationRequest entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.SelfUserCreationRequestDTO Convert(Domain.Transients.SelfUserCreationRequest entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.SelfUserCreationRequestDTO Convert(Domain.Transients.SelfUserCreationRequest entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            // Get Calculated values

            Contracts.DTO.SelfUserCreationRequestDTO dto = new();
            // Property [S][NO][NP][CA][PUO]
            dto.Email = entity.Email;

            dto.Password = entity.Password;

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Transients.SelfUserCreationRequest Convert(Contracts.DTO.SelfUserCreationRequestDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Transients.SelfUserCreationRequest entity = new();
            // Property [S][NO][NP][PUO]
            entity.Email = dataTransferObject.Email;

            entity.Password = dataTransferObject.Password;

            AdditionalConversionForEntity(dataTransferObject, entity);
            return entity;
        }
        public List<Contracts.DTO.SelfUserCreationRequestDTO> Convert(List<Domain.Transients.SelfUserCreationRequest> list)
        {
            return Convert(list, null, "", null);
        }
        public List<Contracts.DTO.SelfUserCreationRequestDTO> Convert(List<Domain.Transients.SelfUserCreationRequest> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.SelfUserCreationRequestDTO> dtos = new();
            foreach (var entity in list)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.SelfUserCreationRequest> Convert(List<Contracts.DTO.SelfUserCreationRequestDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.SelfUserCreationRequest> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.SelfUserCreationRequestDTO> Convert(IEnumerable<Domain.Transients.SelfUserCreationRequest> collection)
        {
            return Convert(collection, null, "", null);
        }
        public List<Contracts.DTO.SelfUserCreationRequestDTO> Convert(IEnumerable<Domain.Transients.SelfUserCreationRequest> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.SelfUserCreationRequestDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.SelfUserCreationRequest> Convert(IEnumerable<Contracts.DTO.SelfUserCreationRequestDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.SelfUserCreationRequest> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

    public partial class UserCreationInfoDTOMapper : IDTOMapper<Contracts.DTO.UserCreationInfoDTO, Domain.Transients.UserCreationInfo>
    {
        private readonly IServiceProvider serviceProvider;
        public UserCreationInfoDTOMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        partial void PartialInitialize(Contracts.DTO.UserCreationInfoDTO dto);
        public void Initialize(Contracts.DTO.UserCreationInfoDTO dto)
        {
            PartialInitialize(dto);
        }
        partial void AdditionalConversionForEntity(Contracts.DTO.UserCreationInfoDTO source, Domain.Transients.UserCreationInfo target);
        partial void AdditionalConversionForDTO(Domain.Transients.UserCreationInfo source, Contracts.DTO.UserCreationInfoDTO target);
        public object ConvertExtension(object entity)
        {
            return ConvertExtension(entity as Domain.Transients.UserCreationInfo);
        }
        public object ConvertExtension(Domain.Transients.UserCreationInfo entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.UserCreationInfoDTO Convert(Domain.Transients.UserCreationInfo entity)
        {
            return Convert(entity, null, "", null);
        }
        public Contracts.DTO.UserCreationInfoDTO Convert(Domain.Transients.UserCreationInfo entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (entity == null)
                return null;
            if (preventLazyLoading == null)
                preventLazyLoading = new List<string>();
            // Get Calculated values

            Contracts.DTO.UserCreationInfoDTO dto = new();
            // Property [S][NO][NP][CA][PUO]
            dto.UserEmail = entity.UserEmail;

            dto.Password = entity.Password;

            // Property [M][NO][NP][CA][PUO]
            if (entity.Rights != null && !preventLazyLoading.Contains(parentPath + "Rights"))
                dto.Rights = new UserRightDTOMapper(serviceProvider).Convert(entity.Rights, preventLazyLoading, parentPath + "Rights" + ".", references);

            AdditionalConversionForDTO(entity, dto);
            Initialize(dto);
            return dto;
        }
        public Domain.Transients.UserCreationInfo Convert(Contracts.DTO.UserCreationInfoDTO dataTransferObject, IServiceProvider serviceProvider)
        {
            if (dataTransferObject == null)
                return null;
            Domain.Transients.UserCreationInfo entity = new();
            // Property [S][NO][NP][PUO]
            entity.UserEmail = dataTransferObject.UserEmail;

            entity.Password = dataTransferObject.Password;

            AdditionalConversionForEntity(dataTransferObject, entity);
            return entity;
        }
        public List<Contracts.DTO.UserCreationInfoDTO> Convert(List<Domain.Transients.UserCreationInfo> list)
        {
            return Convert(list, null, "", null);
        }
        public List<Contracts.DTO.UserCreationInfoDTO> Convert(List<Domain.Transients.UserCreationInfo> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (list == null)
                return null;
            List<Contracts.DTO.UserCreationInfoDTO> dtos = new();
            foreach (var entity in list)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.UserCreationInfo> Convert(List<Contracts.DTO.UserCreationInfoDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.UserCreationInfo> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
        public List<Contracts.DTO.UserCreationInfoDTO> Convert(IEnumerable<Domain.Transients.UserCreationInfo> collection)
        {
            return Convert(collection, null, "", null);
        }
        public List<Contracts.DTO.UserCreationInfoDTO> Convert(IEnumerable<Domain.Transients.UserCreationInfo> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
        {
            if (collection == null)
                return null;
            List<Contracts.DTO.UserCreationInfoDTO> dtos = new();
            foreach (var entity in collection)
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
            return dtos;
        }
        public List<Domain.Transients.UserCreationInfo> Convert(IEnumerable<Contracts.DTO.UserCreationInfoDTO> dataTransferObjects, IServiceProvider serviceProvider)
        {
            if (dataTransferObjects == null)
                return null;
            List<Domain.Transients.UserCreationInfo> collection = new();
            foreach (var dto in dataTransferObjects)
            {
                var entity = Convert(dto, serviceProvider);
                if (entity != null)
                    collection.Add(entity);
            }
            return collection;
        }
    }

}