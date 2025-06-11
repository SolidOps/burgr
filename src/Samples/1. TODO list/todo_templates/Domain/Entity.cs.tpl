using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain;
using SolidOps.SubZero;

#region foreach MODEL[AG][EN]
namespace MetaCorp.Template.Domain._DOMAINTYPE_
{
    public partial class CLASSNAME : BaseCLASSNAME, IEntityOfDomain<_IDENTITY_KEY_TYPE_> /*, INTERFACE */
    {
        private bool _parameterLess = false;
        public _IDENTITY_KEY_TYPE_ Id { get; set; }

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


        public _DOMAINTYPE_._PROPERTYTYPE_ _PROPERTYNAME_ {get; set; }
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

        public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__PROPERTYNAME_ { get; set; }        
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][VO]
        public DEPENDENCYNAMESPACE.Domain.ValueObjects._PROPERTYTYPE_ _REF__VO__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][SNA]
        public _DOMAINTYPE_._PROPERTYTYPE_ _NAVIGATION__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][SNA]
        public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__NAVIGATION__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][LNA][EN][AG]
        public List<_DOMAINTYPE_._PROPERTYTYPE_> _NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][LNA][EN][AG]
        public List<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_> _REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][LNA][VO]
        public IReadOnlyCollection<ValueObjects._PROPERTYTYPE_> _VO__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; } = new List<ValueObjects._PROPERTYTYPE_>();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][LNA][VO]
        public IReadOnlyCollection<DEPENDENCYNAMESPACE.Domain.ValueObjects._PROPERTYTYPE_> _VO__REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; } = new List<ValueObjects._PROPERTYTYPE_>();
        #endregion foreach PROPERTY

        #endregion

        #region Methods

        public IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
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

        public void CopyValues(CLASSNAME copy)
        {
            Id = copy.Id;

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

        #endregion
    }
}
#endregion foreach MODEL

#region foreach MODEL[AG][EN]
namespace MetaCorp.Template.Domain._DOMAINTYPE_
{
    public abstract class BaseCLASSNAME
    {
        public virtual IOpsResult PerformAdditionalValidation(ValidationStep validationStep, IUnitOfWork unitOfWork)
        {
            return IOpsResult.Ok();
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

        public override string ToString()
        {
            var value = base.ToString();
            #region to remove if NO_LABEL
            value = this._LABEL_;
            #endregion to remove if NO_LABEL
            return value;
        }
    }
}
#endregion foreach MODEL
