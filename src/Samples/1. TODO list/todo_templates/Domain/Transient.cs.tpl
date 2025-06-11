using SolidOps.TODO.Shared.Domain;
// using SolidOps.TODO.Shared.Core.CrossCutting.Extension; //necessary for Serialize
using System.Linq; //necessary for Cast
using System; //necessary for Cast
using System.Collections.Generic;

namespace MetaCorp.Template.Domain.Transients;

#region foreach MODEL[TR]
public partial class _TR_CLASSNAME : Base_TR_CLASSNAME
{
    #region foreach PROPERTY[S][NO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[S][NP]
    public _SIMPLE__TYPE_ _NONPERSISTED__SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR]
    public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NP][NAR]
    public DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ _NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][AR]
    public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NP][AR]
    public List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> _FORLIST__NONPERSISTED__ENUM__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][EN][AG]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _PROPERTYNAME_Id { get; set; } = default;

    public _DOMAINTYPE_._PROPERTYTYPE_ _PROPERTYNAME_ { get; set; } = null;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][VO][NAR]
    public ValueObjects._PROPERTYTYPE_ _VO__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][VO][AR]
    public List<ValueObjects._PROPERTYTYPE_> _VO__PROPERTYNAME_ { get; set; }
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

    public DEPENDENCYNAMESPACE.Domain._DOMAINTYPE_._PROPERTYTYPE_ _REF__PROPERTYNAME_ { get; set; } = null;
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
    public IReadOnlyCollection<ValueObjects._PROPERTYTYPE_> _VO__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[R][LNA][VO]
    public IReadOnlyCollection<DEPENDENCYNAMESPACE.Domain.ValueObjects._PROPERTYTYPE_> _VO__REF__NAVIGATION__NAVIGATION__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    public virtual void CopyValues(CLASSNAME copy)
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
}
#endregion foreach MODEL

#region foreach MODEL[TR]
public abstract class Base_TR_CLASSNAME : ITransientEntity /*, INTERFACE */
{
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
    protected virtual _SIMPLE__TYPE_ Calculate_CALCULATED__SIMPLE__PROPERTYNAME_()
    {
        return default;
    }
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
    protected virtual DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_ Calculate_CALCULATED__ENUM__PROPERTYNAME_()
    {
        return default;
    }
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
    protected virtual List<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_> Calculate_FORLIST__CALCULATED__ENUM__PROPERTYNAME_()
    {
        return default;
    }
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
    protected virtual _DOMAINTYPE_._PROPERTYTYPE_ Calculate_CALCULATED__PROPERTYNAME_()
    {
        return default;
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
    protected virtual List<_DOMAINTYPE_._PROPERTYTYPE_> Calculate_CALCULATED__PROPERTYNAME_()
    {
        return default;
    }
    #endregion foreach PROPERTY

}

#endregion foreach MODEL