#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaCorp.Template.Contracts;

#region foreach MODEL[AG]
public sealed partial class CLASSNAMEDto
{
    public _IDENTITY_KEY_TYPE_ Id { get; set; }

    #region foreach PROPERTY[S][NO][NAR][PUO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[S][CA][NAR][PUO]
    public _SIMPLE__TYPE_ _PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][CA][NAR][PUO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][PUO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][AG][PUO]
    public List<_PROPERTYTYPE_Dto> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    public static CLASSNAMEDto From(Domain.CLASSNAME entity) => new()
    {
        Id = entity.Id,
        #region foreach PROPERTY[S][NO][CA][NAR][PUO]
        _PROPERTYNAME_ = entity._PROPERTYNAME_,
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO][CA][NAR][PUO]
        _PROPERTYNAME_ = entity._PROPERTYNAME_,
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][PUO]
        _SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_,
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][LNA][AG][PUO]
        _PROPERTYNAME_ = entity._PROPERTYNAME_.Select(_PROPERTYTYPE_Dto.From).ToList(),
        #endregion foreach PROPERTY
    };
}

public sealed partial class CLASSNAMEInput
{
    #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR][PUO][NRO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][PUO][NRO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(_SIMPLE__PROPERTYNAME_))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_SIMPLE__PROPERTYNAME_ is required"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (_SIMPLE__PROPERTYNAME_?.Length > FIELDSIZE)
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_SIMPLE__PROPERTYNAME_ is longer than FIELDSIZE characters"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO][NAR][PUO][NRO][NN]
        if (!Enum.IsDefined(_PROPERTYNAME_))
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is not a valid value"];
        }
        #endregion foreach PROPERTY
        OnValidating(errors);
        return errors;
    }

    partial void OnValidating(Dictionary<string, string[]> errors);

    public void ApplyTo(Domain.CLASSNAME entity)
    {
        #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
        entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO][NAR][PUO][NRO]
        entity._PROPERTYNAME_ = _PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][PUO][NRO]
        entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY
    }
}

#endregion foreach MODEL
