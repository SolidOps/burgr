using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;
using System.Text.Json.Serialization;
using SolidOps.UM.Shared.Contracts.Results;

namespace MetaCorp.Template.Domain.ValueObjects;

#region foreach MODEL[VO]
public partial class _VO_CLASSNAME : Base_VO_CLASSNAME
{
    public _VO_CLASSNAME()
    {
    }

    #region Factories
    public static Domain.ValueObjects.CLASSNAME Create(
    #region foreach PROPERTY[S][NO]
            _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_,
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO]
            DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_,
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][R][NO]
            // concrete
            _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id,
    #endregion foreach PROPERTY
            bool nothing = false
        )
    {
        var entity = new Domain.ValueObjects.CLASSNAME();
        #region foreach PROPERTY[S][NO]
        entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
        entity._ENUM__PROPERTYNAME_ = _ENUM__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][NAR]
        entity._PROPERTYNAME_Id = _PROPERTYNAME_Id;
        #endregion foreach PROPERTY
        return entity;
    }

    public static Domain.ValueObjects.CLASSNAME CreateEmpty()
    {
        var entity = new Domain.ValueObjects.CLASSNAME();
        return entity;
    }

    public static Domain.ValueObjects.CLASSNAME CreateFromJson(string json)
    {
        var entity = new Domain.ValueObjects.CLASSNAME();
        try
        {
            json = entity.RepairInput(json);
            var copy = Serializer.Deserialize<Domain.ValueObjects.CLASSNAME>(json);
            entity.CopyValues(copy);
        }
        catch (Exception e)
        {
            entity.RepairAfterException(e, json);
        }
        return entity;
    }

    public static List<Domain.ValueObjects.CLASSNAME> CreateListFromJson(string json)
    {
        var list = Serializer.Deserialize<List<object>>(json);
        var result = new List<Domain.ValueObjects.CLASSNAME>();
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

    #region foreach PROPERTY[S][NO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[S][NP]
    [JsonIgnore]
    public _SIMPLE__TYPE_ _NONPERSISTED__SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR]
    public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NP][NAR]
    [JsonIgnore]
    public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][AR]
    public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NP][AR]
    [JsonIgnore]
    public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][EN][AG]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id { get; set; } = default;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][NAR]
    public _DOMAINTYPE_._PROPERTYTYPE_ _NONPERSISTED__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][AR]
    public List<_DOMAINTYPE_._PROPERTYTYPE_> _NONPERSISTED__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][NP][NAR]
    [JsonIgnore]
    public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__NONPERSISTED__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][NN][NP][AR]
    [JsonIgnore]
    public List<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_> _REF__REF__NONPERSISTED__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][NN][NO][EN][AG]
    public _IDENTITY_KEY_TYPE_ _REF__PROPERTYNAME_Id { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][N][NO][EN][AG]
    public _IDENTITY_KEY_TYPE_? _NULL__REF__PROPERTYNAME_Id { get; set; } = null;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][SNA]
    [JsonIgnore]
    public _DOMAINTYPE_._PROPERTYTYPE_ _NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][SNA]
    [JsonIgnore]
    public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][EN][AG]
    [JsonIgnore]
    public List<_DOMAINTYPE_._PROPERTYTYPE_> _NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][LNA][EN][AG]
    [JsonIgnore]
    public List<DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_> _REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    public override IOpsResult Validate(ValidationStep validationStep, IUnitOfWork unitOfWork)
    {
        var result = base.Validate(validationStep, unitOfWork);
        if (result.HasError) return result;

        #region foreach PROPERTY[S][NO]
        // VALIDATION RULE - PROPERTY_HAS_MAXSIZE
        if (!ValueTypeHelper.IsNull(this._SIMPLE__PROPERTYNAME_) && this._SIMPLE__PROPERTYNAME_.ToString().Length > FIELDSIZE)
            return IOpsResult.Invalid("_SIMPLE__PROPERTYNAME_ length is over FIELDSIZE");

        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        // Test
        if (ValueTypeHelper.IsNull(this._SIMPLE__PROPERTYNAME_))
            return IOpsResult.Invalid("_SIMPLE__PROPERTYNAME_ must not be null");

        #endregion foreach PROPERTY

        return PerformAdditionalValidation(validationStep, unitOfWork);
    }

    public override void CopyValues(_VO_CLASSNAME copy)
    {
        #region foreach PROPERTY[S][NO][NP][PUO]
        _SIMPLE__PROPERTYNAME_ = copy._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO]
        _ENUM__PROPERTYNAME_ = copy._ENUM__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        _PROPERTYNAME_Id = copy._PROPERTYNAME_Id;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        _REF__PROPERTYNAME_Id = copy._REF__PROPERTYNAME_Id;
        #endregion foreach PROPERTY
    }
}

#endregion foreach MODEL

#region foreach MODEL[VO]
public abstract class Base_VO_CLASSNAME : BaseDomainValueObject<_VO_CLASSNAME>, IDomainObject /*, INTERFACE */
{
    #region foreach PROPERTY[S][CA]
    [JsonIgnore]
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
    protected virtual _SIMPLE__TYPE_ Calculate_CALCULATED__SIMPLE__PROPERTYNAME_()
    {
        return default;
    }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][CA][NAR]
    private DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _CALCULATED__ENUM__FIELDNAME_ = default;
    [JsonIgnore]
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
    protected virtual DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ Calculate_CALCULATED__ENUM__PROPERTYNAME_()
    {
        return default;
    }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][CA][AR]
    private List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__CALCULATED__ENUM__FIELDNAME_ = default;
    [JsonIgnore]
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
    protected virtual List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> Calculate_FORLIST__CALCULATED__ENUM__PROPERTYNAME_()
    {
        return default;
    }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][R][CA]
    private _DOMAINTYPE_._PROPERTYTYPE_ _CALCULATED__FIELDNAME_ = null;
    [JsonIgnore]
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
    }
    protected virtual _DOMAINTYPE_._PROPERTYTYPE_ Calculate_CALCULATED__PROPERTYNAME_()
    {
        return default;
    }
    #endregion foreach PROPERTY

    #region to remove at generation
    public static readonly int FIELDSIZE = 0;
    #endregion to remove at generation
}

#endregion foreach MODEL