using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Domain.Repositories;
using SolidOps.SubZero;
// Object [EN][AG]
namespace SolidOps.TODO.Domain.AggregateRoots.Rules
{
    public abstract class BaseItemRules : IEntityRules<Guid, Domain.AggregateRoots.Item>
    {
        public int Priority
        {
            get
            {
                return 1;
            }
        }
        protected readonly IUserContext userContext;
        protected readonly IServiceProvider serviceProvider;
        public BaseItemRules(IUserContext userContext, IServiceProvider serviceProvider)
        {
            this.userContext = userContext;
            this.serviceProvider = serviceProvider;
        }
        public virtual async Task<IOpsResult> OnBeforeAdd(Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterAdd(Guid id, Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            IOpsResult result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain.AggregateRoots.Item entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        protected virtual async Task<IOpsResult> Validate(Domain.AggregateRoots.Item entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<IItemRepository>();
            // Property 
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            {
                var result = await repository.GetSingleByName(entity.Name, null);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("Name", "Item with this Name already exists");
                }
            }

            // Property 

            return IOpsResult.Ok();
        }
        // Rules

    }
    public partial class ItemRules : BaseItemRules
    {
        public ItemRules(IUserContext userContext, IServiceProvider serviceProvider) : base(userContext, serviceProvider)
        {
        }
    }
}