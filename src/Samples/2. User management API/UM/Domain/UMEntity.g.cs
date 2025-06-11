using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Text.Json.Serialization;
// Object [AG][EN]
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class LocalUser : BaseLocalUser, IDomainEntity<Guid>, IEFEntity<Guid, Domain.AggregateRoots.LocalUser> 
    {
        private bool _parameterLess = false;
        Guid IEFEntity<Guid, LocalUser>.Id { get { return this.Id; } set { this.SetId(value); } }

        private LocalUser() : base()
        {
        }
        #region Factories
        public static Domain.AggregateRoots.LocalUser Create(
        // Property [S][NO]
                System.String Name,

                System.String HashedPassword,

                bool nothing = false
            )
        {
            var entity = new Domain.AggregateRoots.LocalUser();
            // Property [S][NO]
            entity.Name = Name;

            entity.HashedPassword = HashedPassword;

            return entity;
        }
        public static Domain.AggregateRoots.LocalUser CreateEmpty()
        {
            var entity = new Domain.AggregateRoots.LocalUser();
            return entity;
        }
        public static Domain.AggregateRoots.LocalUser CreateFromJson(string json)
        {
            var entity = new Domain.AggregateRoots.LocalUser();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain.AggregateRoots.LocalUser>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }
        public static List<Domain.AggregateRoots.LocalUser> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain.AggregateRoots.LocalUser>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var itemJson = Serializer.Serialize(item);
                    var entity = CreateFromJson(itemJson);
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion
        #region Properties
        // Property [S][NO]
        public System.String Name { get; set; }

        public System.String HashedPassword { get; set; }

        #endregion
        #region Methods
        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Name))
                return IOpsResult.Invalid("Name must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.HashedPassword))
                return IOpsResult.Invalid("HashedPassword must not be null");

            // Property [S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Name) && this.Name.ToString().Length > 50)
                return IOpsResult.Invalid("Name length is over 50");

            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.HashedPassword) && this.HashedPassword.ToString().Length > 255)
                return IOpsResult.Invalid("HashedPassword length is over 255");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public override void CopyValues(LocalUser copy)
        {
            // Property [S][NO][NP][PUO][NAR]
            Name = copy.Name;

            HashedPassword = copy.HashedPassword;

        }
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
        #endregion
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class User : BaseUser, IDomainEntity<Guid>, IEFEntity<Guid, Domain.AggregateRoots.User> 
    {
        private bool _parameterLess = false;
        Guid IEFEntity<Guid, User>.Id { get { return this.Id; } set { this.SetId(value); } }

        private User() : base()
        {
        }
        #region Factories
        public static Domain.AggregateRoots.User Create(
        // Property [S][NO]
                System.String Email,

                System.String Provider,

                System.Boolean TechnicalUser,

                bool nothing = false
            )
        {
            var entity = new Domain.AggregateRoots.User();
            // Property [S][NO]
            entity.Email = Email;

            entity.Provider = Provider;

            entity.TechnicalUser = TechnicalUser;

            return entity;
        }
        public static Domain.AggregateRoots.User CreateEmpty()
        {
            var entity = new Domain.AggregateRoots.User();
            return entity;
        }
        public static Domain.AggregateRoots.User CreateFromJson(string json)
        {
            var entity = new Domain.AggregateRoots.User();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain.AggregateRoots.User>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }
        public static List<Domain.AggregateRoots.User> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain.AggregateRoots.User>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var itemJson = Serializer.Serialize(item);
                    var entity = CreateFromJson(itemJson);
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion
        #region Properties
        // Property [S][NO]
        public System.String Email { get; set; }

        public System.String Provider { get; set; }

        public System.Boolean TechnicalUser { get; set; }

        // Property [S][CA]
        public List<System.String> Rights
        {
            get
            {
                if (ValueTypeHelper.AreEqual<List<System.String>>(default, _rights))
                {
                    _rights = CalculateRights();
                }
                return _rights;
            }
        }
        private List<System.String> _rights = default;
        public void ForceRightsValue(List<System.String> value)
        {
            _rights = value;
        }

        // Property [M][LNA][EN][AG]
        private List<Entities.UserRight> _userRights;
        public List<Entities.UserRight> UserRights
        {
            get
            {
                if ((_userRights == null || !_userRights.Any()) && LazyLoadingEnabled)
                {
                    LazyLoad("UserRights");
                    if (_userRights != null)
                    {
                        foreach (var item in _userRights)
                        {
                            item.LazyLoadingEnabled = LazyLoadingEnabled;
                            item.SetId(item.Id);
                        }
                    }
                }
                return _userRights;
            }
            set => _userRights = value;
        }

        #endregion
        #region Methods
        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Email))
                return IOpsResult.Invalid("Email must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Provider))
                return IOpsResult.Invalid("Provider must not be null");

            // Property [S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Email) && this.Email.ToString().Length > 50)
                return IOpsResult.Invalid("Email length is over 50");

            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Provider) && this.Provider.ToString().Length > 50)
                return IOpsResult.Invalid("Provider length is over 50");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public override void CopyValues(User copy)
        {
            // Property [S][NO][NP][PUO][NAR]
            Email = copy.Email;

            Provider = copy.Provider;

            TechnicalUser = copy.TechnicalUser;

            // Property [M][R][NO][NP][NA][PUO][AR][EN][AG]
            UserRights = copy.UserRights;

        }
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
        #endregion
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public partial class UserRight : BaseUserRight, IDomainEntity<Guid>, IEFEntity<Guid, Domain.Entities.UserRight> 
    {
        private bool _parameterLess = false;
        Guid IEFEntity<Guid, UserRight>.Id { get { return this.Id; } set { this.SetId(value); } }

        private UserRight() : base()
        {
        }
        #region Factories
        public static Domain.Entities.UserRight Create(

        // Property [M][R][NO][EN][AG]
                Guid UserId,

                Guid RightId,

                bool nothing = false
            )
        {
            var entity = new Domain.Entities.UserRight();

            // Property [M][R][NO][EN][AG]
            entity.UserId = UserId;

            entity.RightId = RightId;

            return entity;
        }
        public static Domain.Entities.UserRight CreateEmpty()
        {
            var entity = new Domain.Entities.UserRight();
            return entity;
        }
        public static Domain.Entities.UserRight CreateFromJson(string json)
        {
            var entity = new Domain.Entities.UserRight();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain.Entities.UserRight>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }
        public static List<Domain.Entities.UserRight> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain.Entities.UserRight>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var itemJson = Serializer.Serialize(item);
                    var entity = CreateFromJson(itemJson);
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion
        #region Properties

        // Property [M][NO][EN][AG]
        public Guid UserId { get; set; } = default;
        private AggregateRoots.User _user = null;
        public AggregateRoots.User User
        {
            get
            {
                if (_user == null && LazyLoadingEnabled)
                {
                    LazyLoad("User");
                    if (_user != null)
                    {
                        _user.LazyLoadingEnabled = LazyLoadingEnabled;
                        _user.SetId(_user.Id);
                    }
                }
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public Guid RightId { get; set; } = default;
        private Entities.Right _right = null;
        public Entities.Right Right
        {
            get
            {
                if (_right == null && LazyLoadingEnabled)
                {
                    LazyLoad("Right");
                    if (_right != null)
                    {
                        _right.LazyLoadingEnabled = LazyLoadingEnabled;
                        _right.SetId(_right.Id);
                    }
                }
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        #endregion
        #region Methods
        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.UserId))
                return IOpsResult.Invalid("UserId must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.RightId))
                return IOpsResult.Invalid("RightId must not be null");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public override void CopyValues(UserRight copy)
        {

            // Property [M][R][NO][PUO][EN][AG]
            UserId = copy.UserId;

            RightId = copy.RightId;

            // Property [M][R][NO][NP][NA][PUO][NAR][EN][AG][TR]
            User = copy.User;

            Right = copy.Right;

        }
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
        #endregion
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public partial class Right : BaseRight, IDomainEntity<Guid>, IEFEntity<Guid, Domain.Entities.Right> 
    {
        private bool _parameterLess = false;
        Guid IEFEntity<Guid, Right>.Id { get { return this.Id; } set { this.SetId(value); } }

        private Right() : base()
        {
        }
        #region Factories
        public static Domain.Entities.Right Create(
        // Property [S][NO]
                System.String Name,

                bool nothing = false
            )
        {
            var entity = new Domain.Entities.Right();
            // Property [S][NO]
            entity.Name = Name;

            return entity;
        }
        public static Domain.Entities.Right CreateEmpty()
        {
            var entity = new Domain.Entities.Right();
            return entity;
        }
        public static Domain.Entities.Right CreateFromJson(string json)
        {
            var entity = new Domain.Entities.Right();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain.Entities.Right>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }
        public static List<Domain.Entities.Right> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain.Entities.Right>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var itemJson = Serializer.Serialize(item);
                    var entity = CreateFromJson(itemJson);
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion
        #region Properties
        // Property [S][NO]
        public System.String Name { get; set; }

        #endregion
        #region Methods
        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Name))
                return IOpsResult.Invalid("Name must not be null");

            // Property [S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Name) && this.Name.ToString().Length > 50)
                return IOpsResult.Invalid("Name length is over 50");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public override void CopyValues(Right copy)
        {
            // Property [S][NO][NP][PUO][NAR]
            Name = copy.Name;

        }
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
        #endregion
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public partial class Invite : BaseInvite, IDomainEntity<Guid>, IEFEntity<Guid, Domain.AggregateRoots.Invite> 
    {
        private bool _parameterLess = false;
        Guid IEFEntity<Guid, Invite>.Id { get { return this.Id; } set { this.SetId(value); } }

        private Invite() : base()
        {
        }
        #region Factories
        public static Domain.AggregateRoots.Invite Create(
        // Property [S][NO]
                System.String Email,

                System.String CreatorName,

                System.String CreatorMessage,

        // Property [E][NO]
                SolidOps.UM.Contracts.Enums.InviteStatusEnum Status,

        // Property [M][R][NO][EN][AG]
                Guid CreatorId,

                bool nothing = false
            )
        {
            var entity = new Domain.AggregateRoots.Invite();
            // Property [S][NO]
            entity.Email = Email;

            entity.CreatorName = CreatorName;

            entity.CreatorMessage = CreatorMessage;

            // Property [E][NO]
            entity.Status = Status;

            // Property [M][R][NO][EN][AG]
            entity.CreatorId = CreatorId;

            return entity;
        }
        public static Domain.AggregateRoots.Invite CreateEmpty()
        {
            var entity = new Domain.AggregateRoots.Invite();
            return entity;
        }
        public static Domain.AggregateRoots.Invite CreateFromJson(string json)
        {
            var entity = new Domain.AggregateRoots.Invite();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain.AggregateRoots.Invite>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }
        public static List<Domain.AggregateRoots.Invite> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain.AggregateRoots.Invite>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    var itemJson = Serializer.Serialize(item);
                    var entity = CreateFromJson(itemJson);
                    result.Add(entity);
                }
            }
            return result;
        }
        #endregion
        #region Properties
        // Property [S][NO]
        public System.String Email { get; set; }

        public System.String CreatorName { get; set; }

        public System.String CreatorMessage { get; set; }

        // Property [E][NO][NAR]
        public SolidOps.UM.Contracts.Enums.InviteStatusEnum Status { get; set; }

        // Property [M][NO][EN][AG]
        public Guid CreatorId { get; set; } = default;
        private AggregateRoots.User _creator = null;
        public AggregateRoots.User Creator
        {
            get
            {
                if (_creator == null && LazyLoadingEnabled)
                {
                    LazyLoad("Creator");
                    if (_creator != null)
                    {
                        _creator.LazyLoadingEnabled = LazyLoadingEnabled;
                        _creator.SetId(_creator.Id);
                    }
                }
                return _creator;
            }
            set
            {
                _creator = value;
            }
        }

        #endregion
        #region Methods
        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            // Property 
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Email))
                return IOpsResult.Invalid("Email must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.Status))
                return IOpsResult.Invalid("Status must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.CreatorId))
                return IOpsResult.Invalid("CreatorId must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.CreatorName))
                return IOpsResult.Invalid("CreatorName must not be null");

            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this.CreatorMessage))
                return IOpsResult.Invalid("CreatorMessage must not be null");

            // Property [S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.Email) && this.Email.ToString().Length > 255)
                return IOpsResult.Invalid("Email length is over 255");

            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.CreatorName) && this.CreatorName.ToString().Length > 255)
                return IOpsResult.Invalid("CreatorName length is over 255");

            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this.CreatorMessage) && this.CreatorMessage.ToString().Length > 255)
                return IOpsResult.Invalid("CreatorMessage length is over 255");

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }
        public override void CopyValues(Invite copy)
        {
            // Property [S][NO][NP][PUO][NAR]
            Email = copy.Email;

            CreatorName = copy.CreatorName;

            CreatorMessage = copy.CreatorMessage;

            // Property [E][NO][NP][PUO]
            Status = copy.Status;

            // Property [M][R][NO][PUO][EN][AG]
            CreatorId = copy.CreatorId;

            // Property [M][R][NO][NP][NA][PUO][NAR][EN][AG][TR]
            Creator = copy.Creator;

        }
        public override string ToString()
        {
            var value = base.ToString();

            return value;
        }
        #endregion
    }
}
// Object [AG][EN]
namespace SolidOps.UM.Domain.AggregateRoots
{
    public abstract class BaseLocalUser : BaseDomainEntity<Guid, Domain.AggregateRoots.LocalUser>
    {
        protected virtual void LazyLoad(string navigationName)
        {
        }
        #region Calculated Properties

        #endregion
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public abstract class BaseUser : BaseDomainEntity<Guid, Domain.AggregateRoots.User>
    {
        protected virtual void LazyLoad(string navigationName)
        {
        }
        #region Calculated Properties
        // Property [S][CA]
        protected virtual List<System.String> CalculateRights()
        {
            return default;
        }

        #endregion
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public abstract class BaseUserRight : BaseDomainEntity<Guid, Domain.Entities.UserRight>
    {
        protected virtual void LazyLoad(string navigationName)
        {
        }
        #region Calculated Properties

        #endregion
    }
}
namespace SolidOps.UM.Domain.Entities
{
    public abstract class BaseRight : BaseDomainEntity<Guid, Domain.Entities.Right>
    {
        protected virtual void LazyLoad(string navigationName)
        {
        }
        #region Calculated Properties

        #endregion
    }
}
namespace SolidOps.UM.Domain.AggregateRoots
{
    public abstract class BaseInvite : BaseDomainEntity<Guid, Domain.AggregateRoots.Invite>
    {
        protected virtual void LazyLoad(string navigationName)
        {
        }
        #region Calculated Properties

        #endregion
    }
}