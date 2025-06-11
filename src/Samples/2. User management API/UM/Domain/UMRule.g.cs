using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Domain.Repositories;
using SolidOps.SubZero;
// Object [EN][AG]
namespace SolidOps.UM.Domain.AggregateRoots.Rules
{
    public abstract class BaseLocalUserRules : IEntityRules<Guid, Domain.AggregateRoots.LocalUser>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IExecutionContext executionContext;
        protected readonly ILogger<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>> logger;
        protected readonly IServiceProvider serviceProvider;
        public BaseLocalUserRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.executionContext = executionContext;
            this.logger = loggerFactory.CreateLogger<IEntityRules<Guid, Domain.AggregateRoots.LocalUser>>();
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeAdd");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterAdd");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeUpdate");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterUpdate");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.AggregateRoots.LocalUser entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.AggregateRoots.LocalUser entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<ILocalUserRepository>();
            // Property 
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            {
                var result = await repository.GetSingleByName(entity.Name, null, validationStep == ValidationStep.Creation);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("Name", "LocalUser with this Name already exists");
                }
            }

            // Property 

            return IOpsResult.Ok();
        }
        // Rules

    }
    public partial class LocalUserRules : BaseLocalUserRules
    {
        public LocalUserRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) : base(executionContext, loggerFactory, serviceProvider)
        {
        }
    }
}
namespace SolidOps.UM.Domain.AggregateRoots.Rules
{
    public abstract class BaseUserRules : IEntityRules<Guid, Domain.AggregateRoots.User>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IExecutionContext executionContext;
        protected readonly ILogger<IEntityRules<Guid, Domain.AggregateRoots.User>> logger;
        protected readonly IServiceProvider serviceProvider;
        public BaseUserRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.executionContext = executionContext;
            this.logger = loggerFactory.CreateLogger<IEntityRules<Guid, Domain.AggregateRoots.User>>();
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeAdd");
            IOpsResult result;
            // Rule [BA]
            result = await CreateLocalUser(Guid.Empty, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterAdd");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeUpdate");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterUpdate");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.AggregateRoots.User entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.AggregateRoots.User entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<IUserRepository>();
            // Property 
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            {
                var result = await repository.GetSingleByEmail(entity.Email, null, validationStep == ValidationStep.Creation);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("Email", "User with this Email already exists");
                }
            }

            // Property 

            return IOpsResult.Ok();
        }
        // Rules
        // Rule 
        protected virtual async Task<IOpsResult> CreateLocalUser(Guid id, Domain.AggregateRoots.User entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            return IOpsResult.Ok();
        }

    }
    public partial class UserRules : BaseUserRules
    {
        public UserRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) : base(executionContext, loggerFactory, serviceProvider)
        {
        }
    }
}
namespace SolidOps.UM.Domain.Entities.Rules
{
    public abstract class BaseUserRightRules : IEntityRules<Guid, Domain.Entities.UserRight>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IExecutionContext executionContext;
        protected readonly ILogger<IEntityRules<Guid, Domain.Entities.UserRight>> logger;
        protected readonly IServiceProvider serviceProvider;
        public BaseUserRightRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.executionContext = executionContext;
            this.logger = loggerFactory.CreateLogger<IEntityRules<Guid, Domain.Entities.UserRight>>();
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeAdd");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterAdd");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeUpdate");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterUpdate");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.Entities.UserRight entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.Entities.UserRight entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<IUserRightRepository>();
            // Property 
            // Property 
            // Property [M][R][NO][EN][AG][NN]
            {
                var repo = serviceProvider.GetRequiredService<SolidOps.UM.Domain.Repositories.IUserRepository>();
                var result = await repo.GetSingleById(entity.UserId, null, validationStep == ValidationStep.Creation);
                if (result == null)
                {
                    return IOpsResult.Invalid("User", "User does not exists");
                }
            }

            {
                var repo = serviceProvider.GetRequiredService<SolidOps.UM.Domain.Repositories.IRightRepository>();
                var result = await repo.GetSingleById(entity.RightId, null, validationStep == ValidationStep.Creation);
                if (result == null)
                {
                    return IOpsResult.Invalid("Right", "Right does not exists");
                }
            }

            return IOpsResult.Ok();
        }
        // Rules

    }
    public partial class UserRightRules : BaseUserRightRules
    {
        public UserRightRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) : base(executionContext, loggerFactory, serviceProvider)
        {
        }
    }
}
namespace SolidOps.UM.Domain.Entities.Rules
{
    public abstract class BaseRightRules : IEntityRules<Guid, Domain.Entities.Right>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IExecutionContext executionContext;
        protected readonly ILogger<IEntityRules<Guid, Domain.Entities.Right>> logger;
        protected readonly IServiceProvider serviceProvider;
        public BaseRightRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.executionContext = executionContext;
            this.logger = loggerFactory.CreateLogger<IEntityRules<Guid, Domain.Entities.Right>>();
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeAdd");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterAdd");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeUpdate");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterUpdate");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.Entities.Right entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.Entities.Right entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<IRightRepository>();
            // Property 
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            {
                var result = await repository.GetSingleByName(entity.Name, null, validationStep == ValidationStep.Creation);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("Name", "Right with this Name already exists");
                }
            }

            // Property 

            return IOpsResult.Ok();
        }
        // Rules

    }
    public partial class RightRules : BaseRightRules
    {
        public RightRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) : base(executionContext, loggerFactory, serviceProvider)
        {
        }
    }
}
namespace SolidOps.UM.Domain.AggregateRoots.Rules
{
    public abstract class BaseInviteRules : IEntityRules<Guid, Domain.AggregateRoots.Invite>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IExecutionContext executionContext;
        protected readonly ILogger<IEntityRules<Guid, Domain.AggregateRoots.Invite>> logger;
        protected readonly IServiceProvider serviceProvider;
        public BaseInviteRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            this.executionContext = executionContext;
            this.logger = loggerFactory.CreateLogger<IEntityRules<Guid, Domain.AggregateRoots.Invite>>();
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeAdd");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterAdd");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeUpdate");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterUpdate");
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnBeforeRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.AggregateRoots.Invite entity, IUnitOfWork unitOfWork)
        {
            logger?.LogDebug("OnAfterRemove");
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.AggregateRoots.Invite entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<IInviteRepository>();
            // Property 
            // Property 
            // Property [M][R][NO][EN][AG][NN]
            {
                var repo = serviceProvider.GetRequiredService<SolidOps.UM.Domain.Repositories.IUserRepository>();
                var result = await repo.GetSingleById(entity.CreatorId, null, validationStep == ValidationStep.Creation);
                if (result == null)
                {
                    return IOpsResult.Invalid("Creator", "Creator does not exists");
                }
            }

            return IOpsResult.Ok();
        }
        // Rules

    }
    public partial class InviteRules : BaseInviteRules
    {
        public InviteRules(IExecutionContext executionContext, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) : base(executionContext, loggerFactory, serviceProvider)
        {
        }
    }
}