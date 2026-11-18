#nullable enable

using System;
using System.Collections.Generic;

namespace _MODULE_NAMESPACE_.Domain;

#region foreach ENUMTYPE
public enum _ENUMTYPE_
{
    VALUES
}

#endregion foreach ENUMTYPE
#region foreach MODEL[AG]
public partial class CLASSNAME : AggregateRoot
{
}

#endregion foreach MODEL
#region foreach MODEL[EN]
public partial class CLASSNAME : Entity
{
}

#endregion foreach MODEL
#region foreach MODEL[AG][EN]
public partial class CLASSNAME
{
    #region foreach PROPERTY[S][NO][NAR]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR]
    public _ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][EN]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    public _PROPERTYTYPE_? _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][VO]
    public _PROPERTYTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; } = null!;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][AR][VO]
    public List<_PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][AG][EN]
    public List<_PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[S][CA][NAR]
    public _SIMPLE__TYPE_ _PROPERTYNAME_ => Calculate_PROPERTYNAME_();
    private partial _SIMPLE__TYPE_ Calculate_PROPERTYNAME_();
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][CA][NAR]
    public _ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ => Calculate_PROPERTYNAME_();
    private partial _ENUMTYPE_/*_ISNULL_*/ Calculate_PROPERTYNAME_();
    #endregion foreach PROPERTY
}

#endregion foreach MODEL
#region foreach MODEL[VO]
public sealed partial record CLASSNAME
{
    #region foreach PROPERTY[S][NO][NAR]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; init; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR]
    public _ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; init; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][EN]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; init; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][VO]
    public _PROPERTYTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; init; } = null!;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][AR][VO]
    public List<_PROPERTYTYPE_> _PROPERTYNAME_ { get; init; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[S][CA][NAR]
    public _SIMPLE__TYPE_ _PROPERTYNAME_ => Calculate_PROPERTYNAME_();
    private partial _SIMPLE__TYPE_ Calculate_PROPERTYNAME_();
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][CA][NAR]
    public _ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ => Calculate_PROPERTYNAME_();
    private partial _ENUMTYPE_/*_ISNULL_*/ Calculate_PROPERTYNAME_();
    #endregion foreach PROPERTY
}

#endregion foreach MODEL
