using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared.Domain;
using SolidOps.TODO.Shared;
using MetaCorp.Template.Domain.Repositories;
using SolidOps.SubZero;

#region foreach MODEL[EN][AG]
namespace MetaCorp.Template.Domain._DOMAINTYPE_.Rules
{
    public abstract class BaseCLASSNAMERules : IEntityRules<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>
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

        public BaseCLASSNAMERules(IUserContext userContext, IServiceProvider serviceProvider)
        {
            this.userContext = userContext;
            this.serviceProvider = serviceProvider;
        }

        public virtual async Task<IOpsResult> OnBeforeAdd(Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            #region foreach RULE[BA]
            result = await _RULE_(Guid.Empty, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            result = entity.Validate(ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }

        public virtual async Task<IOpsResult> OnAfterAdd(_IDENTITY_KEY_TYPE_ id, Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            IOpsResult result;

            #region foreach RULE[AA]
            result = await _RULE_(id, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            #region to remove if NO_ADD_EVENT
            unitOfWork.AddEvent(new Contracts.Events.CLASSNAMEEvent("Add", Serializer.Serialize(entity, true)));
            #endregion to remove if NO_ADD_EVENT

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeUpdate(Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            #region foreach RULE[BU]
            result = await _RULE_(entity.Id, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            result = entity.Validate(ValidationStep.Update, unitOfWork);
            if (result.HasError) return result;

            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterUpdate(Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;

            IOpsResult result;

            #region foreach RULE[AU]
            result = await _RULE_(entity.Id, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            #region to remove if NO_UPDATE_EVENT
            unitOfWork.AddEvent(new Contracts.Events.CLASSNAMEEvent("Update", Serializer.Serialize(entity, true)));
            #endregion to remove if NO_UPDATE_EVENT

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnBeforeRemove(Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            #region foreach RULE[BR]
            result = await _RULE_(entity.Id, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            return IOpsResult.Ok();
        }
        public virtual async Task<IOpsResult> OnAfterRemove(Domain._DOMAINTYPE_.CLASSNAME entity, IUnitOfWork unitOfWork)
        {
            IOpsResult result;

            #region foreach RULE[AR]
            result = await _RULE_(entity.Id, entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;
            #endregion foreach RULE

            result = entity.Validate(ValidationStep.Delete, unitOfWork);
            if (result.HasError) return result;
            result = await Validate(entity, ValidationStep.Creation, unitOfWork);
            if (result.HasError) return result;

            #region to remove if NO_REMOVE_EVENT
            unitOfWork.AddEvent(new Contracts.Events.CLASSNAMEEvent("Remove", Serializer.Serialize(entity, true)));
            #endregion to remove if NO_REMOVE_EVENT

            return IOpsResult.Ok();
        }

        protected virtual async Task<IOpsResult> Validate(Domain._DOMAINTYPE_.CLASSNAME entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            var repository = serviceProvider.GetRequiredService<ICLASSNAMERepository>();
            #region foreach PROPERTY
            // VALIDATION RULE - PROPERTY_IS_UNIQUE
            {
                var result = await repository.GetSingleBy_SIMPLE__PROPERTYNAME_(entity._SIMPLE__PROPERTYNAME_, null);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("_PROPERTYNAME_", "CLASSNAME with this _PROPERTYNAME_ already exists");
                }
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY
            // VALIDATION RULE - MULTIPLE_UNIQUE_CONSTRAINT
            {
                var result = await repository.GetSingleBy_MULTIPLE_CONSTRAINT_(/*_CONSTRAINT_VALUES_*/);
                if (result != null && !result.Id.Equals(entity.Id))
                {
                    return IOpsResult.Invalid("_PROPERTYNAME_", "CLASSNAME with this _PROPERTYNAME_ already exists");
                }
            }
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][EN][AG][NN]
            {
                var repo = serviceProvider.GetRequiredService<DEPENDENCYNAMESPACE.Domain.Repositories._PROPERTYFULLINTERFACE_Repository>();
                var result = await repo.GetSingleById(entity._PROPERTYNAME_Id, null);
                if (result == null)
                {
                    return IOpsResult.Invalid("_PROPERTYNAME_", "_PROPERTYNAME_ does not exists");
                }
            }
            #endregion foreach PROPERTY

            return IOpsResult.Ok();
        }

        // Rules
        #region foreach RULE
        protected virtual async Task<IOpsResult> _RULE_(_IDENTITY_KEY_TYPE_ id, Domain._DOMAINTYPE_.CLASSNAME entity, ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            await Task.CompletedTask;
            return IOpsResult.Ok();
        }
        #endregion foreach RULE

    }

    public partial class CLASSNAMERules : BaseCLASSNAMERules
    {
        public CLASSNAMERules(IUserContext userContext, IServiceProvider serviceProvider) : base(userContext, serviceProvider)
        {

        }
    }
}

#endregion foreach MODEL