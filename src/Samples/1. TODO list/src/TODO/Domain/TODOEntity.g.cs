using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain;
using SolidOps.SubZero;
// Object [AG][EN]
namespace SolidOps.TODO.Domain.AggregateRoots
{
    public partial class Item : BaseItem, IEntityOfDomain<Guid> 
    {
        private bool _parameterLess = false;
        public Guid Id { get; set; }

        private Item() : base()
        {
        }
        #region Factories
        public static Domain.AggregateRoots.Item Create(
                // Property [S][NO]
                System.String Name,

                System.DateTime DueDate,

                // Property [E][NO]
                SolidOps.TODO.Contracts.Enums.ItemStatusEnum Status,

                bool nothing = false
            )
        {
            var entity = new Domain.AggregateRoots.Item();
            // Property [S][NO]
            entity.Name = Name;

            entity.DueDate = DueDate;

            // Property [E][NO]
            entity.Status = Status;

            return entity;
        }
        #endregion
        #region Properties
        // Property [S][NO]
        public System.String Name { get; set; }

        public System.DateTime DueDate { get; set; }

        // Property [S][CA]
        public System.Int32 RemainingDays
        {
            get
            {
                if (ValueTypeHelper.AreEqual<System.Int32>(default, _remainingDays))
                {
                    _remainingDays = CalculateRemainingDays();
                }
                return _remainingDays;
            }
        }
        private System.Int32 _remainingDays = default;
        public void ForceRemainingDaysValue(System.Int32 value)
        {
            _remainingDays = value;
        }

        // Property [E][NO][NAR]
        public SolidOps.TODO.Contracts.Enums.ItemStatusEnum Status { get; set; }

        #endregion
        #region Steps
        public IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Name))
                return IOpsResult.Invalid("Name must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Status))
                return IOpsResult.Invalid("Status must not be null");

            // Property [S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Name) && this.Name.ToString().Length > 50)
                return IOpsResult.Invalid("Name length is over 50");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public void CopyValues(Item copy)
        {
            Id = copy.Id;
            // Property [S][NO][NP][PUO][NAR]
            Name = copy.Name;

            DueDate = copy.DueDate;

            // Property [E][NO][NP][PUO]
            Status = copy.Status;

        }
        #endregion
    }
}
// Object [AG][EN]
namespace SolidOps.TODO.Domain.AggregateRoots
{
    public abstract class BaseItem
    {
        public virtual IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            return IOpsResult.Ok();
        }

        #region Calculated Properties
        // Property [S][CA]
        protected virtual System.Int32 CalculateRemainingDays()
        {
            return default;
        }

        #endregion        
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
    }
}