#nullable enable

using System;
using System.Collections.Generic;

namespace MetaCorp.Template.Domain;

#region foreach ENUMTYPE
public enum _ENUMTYPE_
{
    VALUES
}

#endregion foreach ENUMTYPE
#region foreach MODEL[AG]
public partial class CLASSNAME
{
    public _IDENTITY_KEY_TYPE_ Id { get; set; }

    #region foreach PROPERTY[S][NO][NAR]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR]
    public _ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    public _PROPERTYTYPE_? _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][AG]
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
