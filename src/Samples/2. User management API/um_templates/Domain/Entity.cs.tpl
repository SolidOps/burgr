using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Text.Json.Serialization;

#region foreach MODEL[AG][EN]
namespace MetaCorp.Template.Domain._DOMAINTYPE_
{
    public partial class CLASSNAME : BaseCLASSNAME, IDomainEntity<_IDENTITY_KEY_TYPE_>, IEFEntity<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME> /*, INTERFACE */
    {
        private bool _parameterLess = false;
        _IDENTITY_KEY_TYPE_ IEFEntity<_IDENTITY_KEY_TYPE_, CLASSNAME>.Id { get { return this.Id; } set { this.SetId(value); } }

        #region to remove at generation
        public static readonly int FIELDSIZE = 0;
        #endregion to remove at generation

        private CLASSNAME() : base()
        {

        }

        #region Factories

        public static Domain._DOMAINTYPE_.CLASSNAME Create(
        #region foreach PROPERTY[S][NO]
                _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_,
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
                DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_,
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][EN][AG]
                _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id,
        #endregion foreach PROPERTY
                bool nothing = false
            )
        {
            var entity = new Domain._DOMAINTYPE_.CLASSNAME();
            #region foreach PROPERTY[S][NO]
            entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[E][NO]
            entity._ENUM__PROPERTYNAME_ = _ENUM__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][EN][AG]
            entity._PROPERTYNAME_Id = _PROPERTYNAME_Id;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][VO][AR][NN]
            entity._PROPERTYNAME_ = [];
            #endregion foreach PROPERTY
            return entity;
        }

        public static Domain._DOMAINTYPE_.CLASSNAME CreateEmpty()
        {
            var entity = new Domain._DOMAINTYPE_.CLASSNAME();
            return entity;
        }

        public static Domain._DOMAINTYPE_.CLASSNAME CreateFromJson(string json)
        {
            var entity = new Domain._DOMAINTYPE_.CLASSNAME();
            try
            {
                json = entity.RepairInput(json);
                var copy = Serializer.Deserialize<Domain._DOMAINTYPE_.CLASSNAME>(json);
                entity.CopyValues(copy);
            }
            catch (Exception e)
            {
                entity.RepairAfterException(e, json);
            }
            return entity;
        }

        public static List<Domain._DOMAINTYPE_.CLASSNAME> CreateListFromJson(string json)
        {
            var list = Serializer.Deserialize<List<object>>(json);
            var result = new List<Domain._DOMAINTYPE_.CLASSNAME>();
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

        #region foreach PROPERTY[S][NO]
        public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][CA]
        public _SIMPLE__TYPE_ _CALCULATED__SIMPLE__PROPERTYNAME_
        {
            get
            {
                if (ValueTypeHelper.AreEqual<_SIMPLE__TYPE_>(default, _CALCULATED__SIMPLE__FIELDNAME_))
                {
                    _CALCULATED__SIMPLE__FIELDNAME_ = Calculate_CALCULATED__SIMPLE__PROPERTYNAME_();
                }
                return _CALCULATED__SIMPLE__FIELDNAME_;
            }
        }
        private _SIMPLE__TYPE_ _CALCULATED__SIMPLE__FIELDNAME_ = default;
        public void Force_CALCULATED__SIMPLE__PROPERTYNAME_Value(_SIMPLE__TYPE_ value)
        {
            _CALCULATED__SIMPLE__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NP]
        public _SIMPLE__TYPE_ _NONPERSISTED__SIMPLE__PROPERTYNAME_
        {
            get;
            set;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NAR]
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA][NAR]
        private DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _CALCULATED__ENUM__FIELDNAME_ = default;
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _CALCULATED__ENUM__PROPERTYNAME_
        {
            get
            {
                if (_CALCULATED__ENUM__FIELDNAME_ == default)
                {
                    _CALCULATED__ENUM__FIELDNAME_ = Calculate_CALCULATED__ENUM__PROPERTYNAME_();
                }
                return _CALCULATED__ENUM__FIELDNAME_;
            }
        }
        public void Force_CALCULATED__ENUM__PROPERTYNAME_Value(DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ value)
        {
            _CALCULATED__ENUM__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NP][NAR]
        public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA][AR]
        private List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__CALCULATED__ENUM__FIELDNAME_ = default;
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__CALCULATED__ENUM__PROPERTYNAME_
        {
            get
            {
                if (_FORLIST__CALCULATED__ENUM__FIELDNAME_ == default)
                {
                    _FORLIST__CALCULATED__ENUM__FIELDNAME_ = Calculate_FORLIST__CALCULATED__ENUM__PROPERTYNAME_();
                }
                return _FORLIST__CALCULATED__ENUM__FIELDNAME_;
            }
        }
        public void Force_FORLIST__CALCULATED__ENUM__PROPERTYNAME_Value(List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> value)
        {
            _FORLIST__CALCULATED__ENUM__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NP][AR]
        public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][EN][AG]
        public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id { get; set; } = default;


        private _DOMAINTYPE_._PROPERTYTYPE_ _FIELDNAME_ = null;
        public _DOMAINTYPE_._PROPERTYTYPE_ _PROPERTYNAME_
        {
            get
            {
                if (_FIELDNAME_ == null && LazyLoadingEnabled)
                {
                    LazyLoad("_PROPERTYNAME_");
                    if (_FIELDNAME_ != null)
                    {
                        _FIELDNAME_.LazyLoadingEnabled = LazyLoadingEnabled;
                        _FIELDNAME_.SetId(_FIELDNAME_.Id);
                    }
                }

                return _FIELDNAME_;
            }
            set
            {
                _FIELDNAME_ = value;
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][VO][NAR]
        public ValueObjects._PROPERTYTYPE_ _VO__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][VO][AR]
        public List<ValueObjects._PROPERTYTYPE_> _VO__PROPERTYNAME_ { get; set; } = new List<ValueObjects._PROPERTYTYPE_>();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][CA][NAR]
        private _DOMAINTYPE_._PROPERTYTYPE_ _CALCULATED__FIELDNAME_ = null;
        [JsonInclude]
        public _DOMAINTYPE_._PROPERTYTYPE_ _CALCULATED__PROPERTYNAME_
        {
            get
            {
                if (_CALCULATED__FIELDNAME_ == default)
                {
                    _CALCULATED__FIELDNAME_ = Calculate_CALCULATED__PROPERTYNAME_();
                }
                return _CALCULATED__FIELDNAME_;
            }
            private set
            {
                // for serialization purpose
                _CALCULATED__FIELDNAME_ = value;
            }
        }
        public void Force_CALCULATED__PROPERTYNAME_Value(_PROPERTYTYPE_ value)
        {
            _CALCULATED__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][CA][AR]
        private List<_DOMAINTYPE_._PROPERTYTYPE_> _CALCULATED__FIELDNAME_ = null;
        [JsonInclude]
        public List<_DOMAINTYPE_._PROPERTYTYPE_> _CALCULATED__PROPERTYNAME_
        {
            get
            {
                if (_CALCULATED__FIELDNAME_ == default)
                {
                    _CALCULATED__FIELDNAME_ = Calculate_CALCULATED__PROPERTYNAME_();
                }
                return _CALCULATED__FIELDNAME_;
            }
            private set
            {
                // for serialization purpose
                _CALCULATED__FIELDNAME_ = value;
            }
        }
        public void Force_CALCULATED__PROPERTYNAME_Value(List<_DOMAINTYPE_._PROPERTYTYPE_> value)
        {
            _CALCULATED__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NP][NAR]
        public _DOMAINTYPE_._PROPERTYTYPE_ _NONPERSISTED__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NP][AR]
        public List<_DOMAINTYPE_._PROPERTYTYPE_> _NONPERSISTED__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NP][NAR]
        public _DOMAINTYPE_._PROPERTYTYPE_ _REF__NONPERSISTED__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NP][AR]
        public List<_DOMAINTYPE_._PROPERTYTYPE_> _REF__REF__NONPERSISTED__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][EN][AG]
        public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _REF__PROPERTYNAME_Id { get; set; } = default;

        private DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__FIELDNAME_;
        public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__PROPERTYNAME_
        {
            get
            {
                if (_REF__FIELDNAME_ == null && LazyLoadingEnabled)
                {
                    LazyLoad("_REF__PROPERTYNAME_");
                    if (_REF__FIELDNAME_ != null)
                    {
                        _REF__FIELDNAME_.LazyLoadingEnabled = LazyLoadingEnabled;
                        _REF__FIELDNAME_.SetId(_REF__FIELDNAME_.Id);
                    }
                }

                return _REF__FIELDNAME_;
            }
            set
            {
                _REF__FIELDNAME_ = value;
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][VO]
        public DEPENDENCYNAMESPACE.Domain.ValueObjects._PROPERTYTYPE_ _REF__VO__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][SNA]
        private _DOMAINTYPE_._PROPERTYTYPE_ _NAVIGATION__FIELDNAME_;
        public _DOMAINTYPE_._PROPERTYTYPE_ _NAVIGATION__PROPERTYNAME_
        {
            get
            {
                if (_NAVIGATION__FIELDNAME_ == null && LazyLoadingEnabled)
                {
                    LazyLoad("_NAVIGATION__PROPERTYNAME_");
                    if (_NAVIGATION__FIELDNAME_ != null)
                    {
                        _NAVIGATION__FIELDNAME_.LazyLoadingEnabled = LazyLoadingEnabled;
                        _NAVIGATION__FIELDNAME_.SetId(_NAVIGATION__FIELDNAME_.Id);
                    }
                }
                return _NAVIGATION__FIELDNAME_;
            }
            set => _NAVIGATION__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][SNA]
        private DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__NAVIGATION__FIELDNAME_;
        public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__NAVIGATION__PROPERTYNAME_
        {
            get
            {
                if (_REF__NAVIGATION__FIELDNAME_ == null && LazyLoadingEnabled)
                {
                    LazyLoad("_REF__NAVIGATION__PROPERTYNAME_");
                    if (_REF__NAVIGATION__FIELDNAME_ != null)
                    {
                        _REF__NAVIGATION__FIELDNAME_.LazyLoadingEnabled = LazyLoadingEnabled;
                        _REF__NAVIGATION__FIELDNAME_.SetId(_REF__NAVIGATION__FIELDNAME_.Id);
                    }
                }
                return _REF__NAVIGATION__FIELDNAME_;
            }
            set => _REF__NAVIGATION__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][LNA][EN][AG]
        private List<_DOMAINTYPE_._PROPERTYTYPE_> _NAVIGATION__NAVIGATION__FIELDNAME_;
        public List<_DOMAINTYPE_._PROPERTYTYPE_> _NAVIGATION__NAVIGATION__PROPERTYNAME_
        {
            get
            {
                if ((_NAVIGATION__NAVIGATION__FIELDNAME_ == null || !_NAVIGATION__NAVIGATION__FIELDNAME_.Any()) && LazyLoadingEnabled)
                {
                    LazyLoad("_NAVIGATION__NAVIGATION__PROPERTYNAME_");
                    if (_NAVIGATION__NAVIGATION__FIELDNAME_ != null)
                    {
                        foreach (var item in _NAVIGATION__NAVIGATION__FIELDNAME_)
                        {
                            item.LazyLoadingEnabled = LazyLoadingEnabled;
                            item.SetId(item.Id);
                        }
                    }
                }
                return _NAVIGATION__NAVIGATION__FIELDNAME_;
            }
            set => _NAVIGATION__NAVIGATION__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][LNA][EN][AG]
        private List<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_> _REF__NAVIGATION__NAVIGATION__FIELDNAME_;
        public List<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_> _REF__NAVIGATION__NAVIGATION__PROPERTYNAME_
        {
            get
            {
                if ((_REF__NAVIGATION__NAVIGATION__FIELDNAME_ == null || !_REF__NAVIGATION__NAVIGATION__FIELDNAME_.Any()) && LazyLoadingEnabled)
                {
                    LazyLoad("_REF__NAVIGATION__NAVIGATION__PROPERTYNAME_");
                    if (_REF__NAVIGATION__NAVIGATION__FIELDNAME_ != null)
                    {
                        foreach (var item in _REF__NAVIGATION__NAVIGATION__FIELDNAME_)
                        {
                            item.LazyLoadingEnabled = LazyLoadingEnabled;
                            item.SetId(item.Id);
                        }
                    }
                }
                return _REF__NAVIGATION__NAVIGATION__FIELDNAME_;
            }
            set => _REF__NAVIGATION__NAVIGATION__FIELDNAME_ = value;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][LNA][VO]
        public IReadOnlyCollection<ValueObjects._PROPERTYTYPE_> _VO__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; } = new List<ValueObjects._PROPERTYTYPE_>();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][LNA][VO]
        public IReadOnlyCollection<DEPENDENCYNAMESPACE.Domain.ValueObjects._PROPERTYTYPE_> _VO__REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; } = new List<ValueObjects._PROPERTYTYPE_>();
        #endregion foreach PROPERTY

        #endregion

        #region Methods

        public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            #region foreach PROPERTY
            // VALIDATION RULE - NULLABLE_PROPERTY
            // Test
            if (ValueTypeHelper.IsNull(this._SIMPLE__PROPERTYNAME_))
                return IOpsResult.Invalid("_SIMPLE__PROPERTYNAME_ must not be null");

            #endregion foreach PROPERTY

            #region foreach PROPERTY[S][NO]
            // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
            if (!ValueTypeHelper.IsNull(this._SIMPLE__PROPERTYNAME_) && this._SIMPLE__PROPERTYNAME_.ToString().Length > FIELDSIZE)
                return IOpsResult.Invalid("_SIMPLE__PROPERTYNAME_ length is over FIELDSIZE");

            #endregion foreach PROPERTY

            return PerformAdditionalValidation(validationStep, unitOfWork);
        }

        public override void CopyValues(CLASSNAME copy)
        {
            #region foreach PROPERTY[S][NO][NP][PUO][NAR]
            _SIMPLE__PROPERTYNAME_ = copy._SIMPLE__PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[S][NO][NP][PUO][AR]
            _SIMPLE__PROPERTYNAME_ = copy._SIMPLE__PROPERTYNAME_;
            #endregion foreach PROPERTY


            #region foreach PROPERTY[E][NO][NP][PUO]
            _ENUM__PROPERTYNAME_ = copy._ENUM__PROPERTYNAME_;
            #endregion foreach PROPERTY


            #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
            _PROPERTYNAME_Id = copy._PROPERTYNAME_Id;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][NP][NA][PUO][NAR][EN][AG][TR]
            _PROPERTYNAME_ = copy._PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][NP][NA][PUO][NAR][VO]
            _PROPERTYNAME_ = copy._PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][NP][NA][PUO][AR][EN][AG]
            _PROPERTYNAME_ = copy._PROPERTYNAME_;
            #endregion foreach PROPERTY

            #region foreach PROPERTY[M][R][NO][NP][NA][PUO][AR][VO]
            // value
            _PROPERTYNAME_ = copy._PROPERTYNAME_;
            #endregion foreach PROPERTY
        }

        public override string ToString()
        {
            var value = base.ToString();
            #region to remove if NO_LABEL
            value = this._LABEL_;
            #endregion to remove if NO_LABEL
            return value;
        }

        #endregion
    }
}
#endregion foreach MODEL

#region foreach MODEL[AG][EN]
namespace MetaCorp.Template.Domain._DOMAINTYPE_
{
    public abstract class BaseCLASSNAME : BaseDomainEntity<_IDENTITY_KEY_TYPE_, Domain._DOMAINTYPE_.CLASSNAME>
    {
        protected virtual void LazyLoad(string navigationName)
        {

        }

        #region Calculated Properties

        #region foreach PROPERTY[S][CA]
        protected virtual _SIMPLE__TYPE_ Calculate_CALCULATED__SIMPLE__PROPERTYNAME_()
        {
            return default;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA][NAR]
        protected virtual DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ Calculate_CALCULATED__ENUM__PROPERTYNAME_()
        {
            return default;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA][AR]
        protected virtual List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> Calculate_FORLIST__CALCULATED__ENUM__PROPERTYNAME_()
        {
            return default;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][CA][NAR]
        protected virtual _DOMAINTYPE_._PROPERTYTYPE_ Calculate_CALCULATED__PROPERTYNAME_()
        {
            return default;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][CA][AR]
        protected virtual List<_DOMAINTYPE_._PROPERTYTYPE_> Calculate_CALCULATED__PROPERTYNAME_()
        {
            return default;
        }
        #endregion foreach PROPERTY

        #endregion
    }
}
#endregion foreach MODEL
