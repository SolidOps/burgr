#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace _MODULE_NAMESPACE_.Application;

#region foreach MODEL[AG][EN]
public sealed partial class CLASSNAMEDto
{
    public _IDENTITY_KEY_TYPE_ Id { get; set; }

    #region foreach PROPERTY[S][NO][CA][NAR][PUO]
    public _SIMPLE__TYPE_ _PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][CA][NAR][PUO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][VO][PUO]
    public Domain._PROPERTYTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; } = null!;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][AR][VO][PUO]
    public List<Domain._PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][AG][EN][PUO]
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
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO]
        _SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_,
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][PUO]
        _PROPERTYNAME_ = entity._PROPERTYNAME_,
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][AR][VO][PUO]
        _PROPERTYNAME_ = [.. entity._PROPERTYNAME_],
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][LNA][AG][EN][PUO]
        _PROPERTYNAME_ = entity._PROPERTYNAME_.Select(_PROPERTYTYPE_Dto.From).ToList(),
        #endregion foreach PROPERTY
    };
}

#endregion foreach MODEL
#region foreach MODEL[AG][R]
public sealed partial class CLASSNAMEDto
{
    /// <summary>The values accepted by the orderBy parameter of the query.</summary>
    public static readonly IReadOnlyCollection<string> SortableProperties =
    [
        "Id",
        #region foreach PROPERTY[S][NO][NAR][PUO]
        "_SIMPLE__PROPERTYNAME_",
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO][NAR][PUO]
        "_PROPERTYNAME_",
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO]
        "_SIMPLE__PROPERTYNAME_",
        #endregion foreach PROPERTY
    ];
}

public sealed partial class CLASSNAMEInput
{
    #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR][PUO][NRO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO]
    public Domain._PROPERTYTYPE_? _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][AR][VO][PUO][NRO]
    public List<Domain._PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][LNA][SY][EN][PUO][NRO]
    /// <summary>The whole list: a child without Id is added, a child missing from the list is removed.</summary>
    public List<_PROPERTYTYPE_Input> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    /// <summary>Required values, maximum lengths, enum values, value objects and synchronized children, then <see cref="OnValidating"/>.</summary>
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
        #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO][NN]
        if (_PROPERTYNAME_ is null)
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is required"];
        }
        else
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_, "_PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO][N]
        if (_PROPERTYNAME_ is not null)
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_, "_PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][AR][VO][PUO][NRO]
        for (var i = 0; i < _PROPERTYNAME_.Count; i++)
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_[i], $"_PROPERTYNAME_[{i}]", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][LNA][SY][EN][PUO][NRO]
        for (var i = 0; i < _PROPERTYNAME_.Count; i++)
        {
            foreach (var (key, messages) in _PROPERTYNAME_[i].Validate())
            {
                errors[$"_PROPERTYNAME_[{i}].{key}"] = messages;
            }
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
        #region foreach PROPERTY[M][NO][NAR][AG][EN][PUO][NRO]
        entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO]
        entity._PROPERTYNAME_ = _PROPERTYNAME_!;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][AR][VO][PUO][NRO]
        entity._PROPERTYNAME_ = [.. _PROPERTYNAME_];
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][LNA][SY][EN][PUO][NRO]
        entity._PROPERTYNAME_.RemoveAll(child => !_PROPERTYNAME_.Exists(input => Equals(input.Id, child.Id)));
        foreach (var childInput in _PROPERTYNAME_)
        {
            var child = childInput.Id is { } childId ? entity._PROPERTYNAME_.Find(c => c.Id.Equals(childId)) : null;
            if (child is null)
            {
                child = new Domain._PROPERTYTYPE_();
                entity._PROPERTYNAME_.Add(child);
            }
            childInput.ApplyTo(child);
        }
        #endregion foreach PROPERTY
    }
}

#endregion foreach MODEL
#region foreach MODEL[EN]
// a child of a synchronized navigation; its references to aggregate roots are set by the navigation
public sealed partial class CLASSNAMEInput
{
    /// <summary>The child to update; null for a new child.</summary>
    public _IDENTITY_KEY_TYPE_? Id { get; set; }

    #region foreach PROPERTY[S][NO][NAR][PUO][NRO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NO][NAR][PUO][NRO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][EN][PUO][NRO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO]
    public Domain._PROPERTYTYPE_? _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    /// <summary>Required values, maximum lengths, enum values and value objects, then <see cref="OnValidating"/>.</summary>
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
        #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO][NN]
        if (_PROPERTYNAME_ is null)
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is required"];
        }
        else
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_, "_PROPERTYNAME_", errors);
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
        #region foreach PROPERTY[M][NO][NAR][EN][PUO][NRO]
        entity._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][PUO][NRO]
        entity._PROPERTYNAME_ = _PROPERTYNAME_!;
        #endregion foreach PROPERTY
    }
}

#endregion foreach MODEL
#region foreach MODEL[TR]
public sealed partial class CLASSNAMEDto : IValidatable
{
    #region foreach PROPERTY[S][NP][NAR][PUO]
    public _SIMPLE__TYPE_ _SIMPLE__PROPERTYNAME_ { get; set; } = _DEFAULT_VALUE_;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[E][NP][NAR][PUO]
    public Domain._ENUMTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][NAR][AG][EN][PUO]
    public _IDENTITY_KEY_TYPE_/*_ISNULL_*/ _SIMPLE__PROPERTYNAME_ { get; set; }
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][AR][AG][EN][TR][PUO]
    public List<_PROPERTYTYPE_Dto> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][NAR][TR][PUO]
    public _PROPERTYTYPE_Dto/*_ISNULL_*/ _PROPERTYNAME_ { get; set; } = null!;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][NAR][VO][PUO]
    public Domain._PROPERTYTYPE_/*_ISNULL_*/ _PROPERTYNAME_ { get; set; } = null!;
    #endregion foreach PROPERTY

    #region foreach PROPERTY[M][NP][AR][VO][PUO]
    public List<Domain._PROPERTYTYPE_> _PROPERTYNAME_ { get; set; } = [];
    #endregion foreach PROPERTY

    /// <summary>Required values, enum values and value objects, then <see cref="OnValidating"/>.</summary>
    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();
        #region foreach PROPERTY[S][NP][NAR][PUO]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(_SIMPLE__PROPERTYNAME_))
        {
            errors["_SIMPLE__PROPERTYNAME_"] = ["_SIMPLE__PROPERTYNAME_ is required"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NP][NAR][PUO][NN]
        if (!Enum.IsDefined(_PROPERTYNAME_))
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is not a valid value"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NP][NAR][VO][PUO][NN]
        if (_PROPERTYNAME_ is null)
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is required"];
        }
        else
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_, "_PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NP][NAR][VO][PUO][N]
        if (_PROPERTYNAME_ is not null)
        {
            _PROPERTYTYPE_Validation.Validate(_PROPERTYNAME_, "_PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NP][NAR][TR][PUO][NN]
        if (_PROPERTYNAME_ is null)
        {
            errors["_PROPERTYNAME_"] = ["_PROPERTYNAME_ is required"];
        }
        #endregion foreach PROPERTY
        OnValidating(errors);
        return errors;
    }

    partial void OnValidating(Dictionary<string, string[]> errors);
}

#endregion foreach MODEL
#region foreach MODEL[VO]
internal static partial class CLASSNAMEValidation
{
    public static void Validate(Domain.CLASSNAME value, string path, Dictionary<string, string[]> errors)
    {
        #region foreach PROPERTY[S][NO][NAR]
        // VALIDATION RULE - NULLABLE_PROPERTY
        if (string.IsNullOrWhiteSpace(value._SIMPLE__PROPERTYNAME_))
        {
            errors[path + "._SIMPLE__PROPERTYNAME_"] = ["_SIMPLE__PROPERTYNAME_ is required"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[S][NO][NAR]
        // VALIDATION RULE - PROPERTY_HAS_MAX_SIZE
        if (value._SIMPLE__PROPERTYNAME_?.Length > FIELDSIZE)
        {
            errors[path + "._SIMPLE__PROPERTYNAME_"] = ["_SIMPLE__PROPERTYNAME_ is longer than FIELDSIZE characters"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO][NAR][NN]
        if (!Enum.IsDefined(value._PROPERTYNAME_))
        {
            errors[path + "._PROPERTYNAME_"] = ["_PROPERTYNAME_ is not a valid value"];
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][NN]
        if (value._PROPERTYNAME_ is null)
        {
            errors[path + "._PROPERTYNAME_"] = ["_PROPERTYNAME_ is required"];
        }
        else
        {
            _PROPERTYTYPE_Validation.Validate(value._PROPERTYNAME_, path + "._PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][NAR][VO][N]
        if (value._PROPERTYNAME_ is not null)
        {
            _PROPERTYTYPE_Validation.Validate(value._PROPERTYNAME_, path + "._PROPERTYNAME_", errors);
        }
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][NO][AR][VO]
        for (var i = 0; i < value._PROPERTYNAME_.Count; i++)
        {
            _PROPERTYTYPE_Validation.Validate(value._PROPERTYNAME_[i], $"{path}._PROPERTYNAME_[{i}]", errors);
        }
        #endregion foreach PROPERTY
    }
}

#endregion foreach MODEL
