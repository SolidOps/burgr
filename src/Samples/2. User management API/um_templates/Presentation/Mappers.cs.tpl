using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.CrossCutting;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Presentation;
using SolidOps.UM.Shared.Presentation.ETag;
using SolidOps.UM.Shared.Presentation.ETag.Filters;
using SolidOps.UM.Shared.Application.Events;
using MetaCorp.Template.Domain.Repositories;
using MetaCorp.Template.Domain.Queries;
using MetaCorp.Template.Presentation.Mappers;
using System.Diagnostics;

namespace MetaCorp.Template.Presentation.Mappers;

#region foreach MODEL[EN][EXP]
public partial class CLASSNAMEDTOMapper : IDTOMapper<Contracts.DTO.CLASSNAMEDTO, Domain.Entities.CLASSNAME>
{
    private readonly IServiceProvider serviceProvider;

    public CLASSNAMEDTOMapper(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    partial void PartialInitialize(Contracts.DTO.CLASSNAMEDTO dto);

    public void Initialize(Contracts.DTO.CLASSNAMEDTO dto)
    {
        PartialInitialize(dto);
    }

    partial void AdditionalConversionForEntity(Contracts.DTO.CLASSNAMEWriteDTO source, Domain.Entities.CLASSNAME target, IServiceProvider serviceProvider);
    partial void AdditionalConversionForEntity(Contracts.DTO.CLASSNAMEDTO source, Domain.Entities.CLASSNAME target, IServiceProvider serviceProvider);
    partial void AdditionalConversionForDTO(Domain.Entities.CLASSNAME source, Contracts.DTO.CLASSNAMEDTO target);

    public object ConvertExtension(object entity)
    {
        return ConvertExtension(entity as Domain.Entities.CLASSNAME);
    }

    public object ConvertExtension(Domain.Entities.CLASSNAME entity)
    {
        return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.Entities.CLASSNAME entity, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(entity, null, "", references);
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.Entities.CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        if (references == null)
            references = new Dictionary<string, Dictionary<string, object>>();

        if (entity.ComposedId == null)
        {
            entity.SetId(entity.Id);
        }
        preventLazyLoading.Add("CLASSNAME|" + entity.ComposedId);

        Contracts.DTO.CLASSNAMEDTO dto = new();
        #region to remove if PRIVATE_ID
        dto.Id = entity.ComposedId;
        #endregion to remove if PRIVATE_ID

        if (!references.ContainsKey("CLASSNAME"))
        {
            references.Add("CLASSNAME", new Dictionary<string, object>());
        }
        if (references["CLASSNAME"].ContainsKey(dto.Id))
        {
            return references["CLASSNAME"][dto.Id] as Contracts.DTO.CLASSNAMEDTO;
        }
        references["CLASSNAME"].Add(dto.Id, dto);

        // Get Calculated values
        #region foreach PROPERTY[S][CA]
        _ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA]
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_") && !preventLazyLoading.Contains("_PROPERTYTYPE_|" + entity._PROPERTYNAME_.ComposedId))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        dto._PROPERTYNAME_Id = entity._PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][AR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][VO]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][NAR][EN][AG][TR]
        // nullable entity and transient
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][NAR][VO]
        // nullable value objects
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][AR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        dto._REF__PROPERTYNAME_Id = entity._REF__PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][CA][PUO]
        if (entity._REF__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_REF__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._REF__PROPERTYNAME_ = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._REF__PROPERTYNAME_, preventLazyLoading, parentPath + "_REF__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][N][NO][NP][CA][NA][PUO]
        if (entity._REF__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__REF__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._REF__PROPERTYNAME_ = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._REF__PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__REF__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        AdditionalConversionForDTO(entity, dto);

        Initialize(dto);

        return dto;
    }

    public Domain.Entities.CLASSNAME Convert(Contracts.DTO.CLASSNAMEWriteDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.Entities.CLASSNAME entity = Domain.Entities.CLASSNAME.CreateEmpty();

        #region foreach PROPERTY[S][NO][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        entity._PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_/*_ISNULL_*/>.ReadString(dataTransferObject._PROPERTYNAME_Id);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][VO][AR]
        // value object array
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][VO]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);

        return entity;
    }

    public bool Patch(Domain.Entities.CLASSNAME entity, Contracts.DTO.CLASSNAMEPatchDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return false;

        bool patched = false;
        #region foreach PROPERTY[S][NO][PUO]
        if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject._SIMPLE__PROPERTYNAME_, entity._SIMPLE__PROPERTYNAME_))
        {
            patched = true;
            entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_/*_ISPATCHNULLVALUE_*/;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        if (!Equals(dataTransferObject._ENUM__PROPERTYNAME_, default) && entity._ENUM__PROPERTYNAME_ != dataTransferObject._ENUM__PROPERTYNAME_)
        {
            patched = true;
            entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        if (!string.IsNullOrEmpty(dataTransferObject._PROPERTYNAME_Id))
        {
            var id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(dataTransferObject._PROPERTYNAME_Id);
            if (!entity._PROPERTYNAME_Id.Equals(id))
            {
                patched = true;
                entity._PROPERTYNAME_Id = id;
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
        {
            if (new _PROPERTYTYPE_DTOMapper(serviceProvider).Patch(entity._PROPERTYNAME_, dataTransferObject._PROPERTYNAME_, serviceProvider))
            {
                patched = true;
            }
        }
        #endregion foreach PROPERTY

        return patched;
    }

    public Domain.Entities.CLASSNAME Convert(Contracts.DTO.CLASSNAMEDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.Entities.CLASSNAME entity = Domain.Entities.CLASSNAME.CreateEmpty();
        #region to remove if PRIVATE_ID
        if (dataTransferObject.Id != default)
            entity.SetId(dataTransferObject.Id);
        #endregion to remove if PRIVATE_ID

        #region foreach PROPERTY[S][NO][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        entity._PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_/*_ISNULL_*/>.ReadString(dataTransferObject._PROPERTYNAME_Id);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][VO][AR]
        // value object array
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][VO]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);

        return entity;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.Entities.CLASSNAME> list, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(list, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.Entities.CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (list == null)
            return null;
        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            if (!preventLazyLoading.Contains("CLASSNAME|" + entity.ComposedId))
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
        }

        return dtos;
    }

    public List<Domain.Entities.CLASSNAME> Convert(List<Contracts.DTO.CLASSNAMEWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public bool Patch(List<Domain.Entities.CLASSNAME> entities, List<Contracts.DTO.CLASSNAMEPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return false;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {

        }

        return false;
    }

    public List<Domain.Entities.CLASSNAME> Convert(List<Contracts.DTO.CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.Entities.CLASSNAME> collection, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(collection, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.Entities.CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (collection == null)
            return null;

        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in collection)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.Entities.CLASSNAME> Convert(IEnumerable<Contracts.DTO.CLASSNAMEWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public bool Patch(List<Domain.Entities.CLASSNAME> entities, IEnumerable<Contracts.DTO.CLASSNAMEPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return false;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {

        }

        return false;
    }

    public List<Domain.Entities.CLASSNAME> Convert(IEnumerable<Contracts.DTO.CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Entities.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

}
#endregion foreach MODEL

#region foreach MODEL[AG][EXP]
public partial class CLASSNAMEDTOMapper : IDTOMapper<Contracts.DTO.CLASSNAMEDTO, Domain.AggregateRoots.CLASSNAME>
{
    private readonly IServiceProvider serviceProvider;

    public CLASSNAMEDTOMapper(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    partial void PartialInitialize(Contracts.DTO.CLASSNAMEDTO dto);

    public void Initialize(Contracts.DTO.CLASSNAMEDTO dto)
    {
        PartialInitialize(dto);
    }

    partial void AdditionalConversionForEntity(Contracts.DTO.CLASSNAMEWriteDTO source, Domain.AggregateRoots.CLASSNAME target, IServiceProvider serviceProvider);
    partial void AdditionalConversionForEntity(Contracts.DTO.CLASSNAMEDTO source, Domain.AggregateRoots.CLASSNAME target, IServiceProvider serviceProvider);
    partial void AdditionalConversionForDTO(Domain.AggregateRoots.CLASSNAME source, Contracts.DTO.CLASSNAMEDTO target);

    public object ConvertExtension(object entity)
    {
        return ConvertExtension(entity as Domain.AggregateRoots.CLASSNAME);
    }

    public object ConvertExtension(Domain.AggregateRoots.CLASSNAME entity)
    {
        return Convert(entity, null, "", new Dictionary<string, Dictionary<string, object>>());
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.AggregateRoots.CLASSNAME entity, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(entity, null, "", references);
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.AggregateRoots.CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        if (references == null)
            references = new Dictionary<string, Dictionary<string, object>>();

        if (entity.ComposedId == null)
        {
            entity.SetId(entity.Id);
        }
        preventLazyLoading.Add("CLASSNAME|" + entity.ComposedId);

        Contracts.DTO.CLASSNAMEDTO dto = new();
        #region to remove if PRIVATE_ID
        dto.Id = entity.ComposedId;
        #endregion to remove if PRIVATE_ID

        if (!references.ContainsKey("CLASSNAME"))
        {
            references.Add("CLASSNAME", new Dictionary<string, object>());
        }
        if (references["CLASSNAME"].ContainsKey(dto.Id))
        {
            return references["CLASSNAME"][dto.Id] as Contracts.DTO.CLASSNAMEDTO;
        }
        references["CLASSNAME"].Add(dto.Id, dto);

        // Get Calculated values
        #region foreach PROPERTY[S][CA]
        _ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA]
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_") && !preventLazyLoading.Contains("_PROPERTYTYPE_|" + entity._PROPERTYNAME_.ComposedId))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        dto._PROPERTYNAME_Id = entity._PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][AR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][VO]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][NAR][EN][AG][TR]
        // nullable entity and transient
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][NAR][VO]
        // nullable value objects
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NA][PUO][AR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__PROPERTYNAME_"))
        {
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        dto._REF__PROPERTYNAME_Id = entity._REF__PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][CA][PUO]
        if (entity._REF__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_REF__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._REF__PROPERTYNAME_ = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._REF__PROPERTYNAME_, preventLazyLoading, parentPath + "_REF__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][N][NO][NP][CA][NA][PUO]
        if (entity._REF__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_NULL__REF__PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.ComposedId))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.ComposedId] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
            }
            else
            {
                dto._REF__PROPERTYNAME_ = new DEPENDENCYNAMESPACE.Presentation.Mappers._PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._REF__PROPERTYNAME_, preventLazyLoading, parentPath + "_NULL__REF__PROPERTYNAME_" + ".", references);
            }
        }
        #endregion foreach PROPERTY

        AdditionalConversionForDTO(entity, dto);

        Initialize(dto);

        return dto;
    }

    public Domain.AggregateRoots.CLASSNAME Convert(Contracts.DTO.CLASSNAMEWriteDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.AggregateRoots.CLASSNAME entity = Domain.AggregateRoots.CLASSNAME.CreateEmpty();

        #region foreach PROPERTY[S][NO][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        entity._PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_/*_ISNULL_*/>.ReadString(dataTransferObject._PROPERTYNAME_Id);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][VO][AR]
        // value object array
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][VO]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);

        return entity;
    }

    public bool Patch(Domain.AggregateRoots.CLASSNAME entity, Contracts.DTO.CLASSNAMEPatchDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return false;

        bool patched = false;
        #region foreach PROPERTY[S][NO][PUO]
        if (ValueTypeHelper.AreSetAnfDifferent(dataTransferObject._SIMPLE__PROPERTYNAME_, entity._SIMPLE__PROPERTYNAME_))
        {
            patched = true;
            entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_/*_ISPATCHNULLVALUE_*/;
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        if (!Equals(dataTransferObject._ENUM__PROPERTYNAME_, default) && entity._ENUM__PROPERTYNAME_ != dataTransferObject._ENUM__PROPERTYNAME_)
        {
            patched = true;
            entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        if (!string.IsNullOrEmpty(dataTransferObject._PROPERTYNAME_Id))
        {
            var id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(dataTransferObject._PROPERTYNAME_Id);
            if (!entity._PROPERTYNAME_Id.Equals(id))
            {
                patched = true;
                entity._PROPERTYNAME_Id = id;
            }
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
        {
            if (new _PROPERTYTYPE_DTOMapper(serviceProvider).Patch(entity._PROPERTYNAME_, dataTransferObject._PROPERTYNAME_, serviceProvider))
            {
                patched = true;
            }
        }
        #endregion foreach PROPERTY

        return patched;
    }

    public Domain.AggregateRoots.CLASSNAME Convert(Contracts.DTO.CLASSNAMEDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.AggregateRoots.CLASSNAME entity = Domain.AggregateRoots.CLASSNAME.CreateEmpty();
        #region to remove if PRIVATE_ID
        if (dataTransferObject.Id != default)
            entity.SetId(dataTransferObject.Id);
        #endregion to remove if PRIVATE_ID

        #region foreach PROPERTY[S][NO][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][PUO][EN][AG]
        entity._PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_/*_ISNULL_*/>.ReadString(dataTransferObject._PROPERTYNAME_Id);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NA]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][VO][AR]
        // value object array
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][VO]
        // navigation
        if (dataTransferObject._PROPERTYNAME_ != null)
            entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);

        return entity;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.AggregateRoots.CLASSNAME> list, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(list, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.AggregateRoots.CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (list == null)
            return null;
        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            if (!preventLazyLoading.Contains("CLASSNAME|" + entity.ComposedId))
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
        }

        return dtos;
    }

    public List<Domain.AggregateRoots.CLASSNAME> Convert(List<Contracts.DTO.CLASSNAMEWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public bool Patch(List<Domain.AggregateRoots.CLASSNAME> entities, List<Contracts.DTO.CLASSNAMEPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return false;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {

        }

        return false;
    }

    public List<Domain.AggregateRoots.CLASSNAME> Convert(List<Contracts.DTO.CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.AggregateRoots.CLASSNAME> collection, Dictionary<string, Dictionary<string, object>> references)
    {
        return Convert(collection, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.AggregateRoots.CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (collection == null)
            return null;

        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in collection)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.AggregateRoots.CLASSNAME> Convert(IEnumerable<Contracts.DTO.CLASSNAMEWriteDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public bool Patch(List<Domain.AggregateRoots.CLASSNAME> entities, IEnumerable<Contracts.DTO.CLASSNAMEPatchDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return false;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {

        }

        return false;
    }

    public List<Domain.AggregateRoots.CLASSNAME> Convert(IEnumerable<Contracts.DTO.CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.AggregateRoots.CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

}
#endregion foreach MODEL

#region foreach MODEL[VO][EXP]
public partial class _VO_CLASSNAMEDTOMapper : IDTOMapper<Contracts.DTO._VO_CLASSNAMEDTO, Domain.ValueObjects._VO_CLASSNAME>
{
    private readonly IServiceProvider serviceProvider;

    public _VO_CLASSNAMEDTOMapper(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    partial void PartialInitialize(Contracts.DTO._VO_CLASSNAMEDTO dto);

    public void Initialize(Contracts.DTO._VO_CLASSNAMEDTO dto)
    {
        PartialInitialize(dto);
    }

    partial void AdditionalConversionForEntity(Contracts.DTO._VO_CLASSNAMEDTO source, Domain.ValueObjects._VO_CLASSNAME target, IServiceProvider serviceProvider);
    partial void AdditionalConversionForDTO(Domain.ValueObjects._VO_CLASSNAME source, Contracts.DTO._VO_CLASSNAMEDTO target);

    public object ConvertExtension(object entity)
    {
        return ConvertExtension(entity as Domain.ValueObjects._VO_CLASSNAME);
    }

    public object ConvertExtension(Domain.ValueObjects._VO_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }

    public Contracts.DTO._VO_CLASSNAMEDTO Convert(Domain.ValueObjects._VO_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }

    public Contracts.DTO._VO_CLASSNAMEDTO Convert(Domain.ValueObjects._VO_CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        // Get Calculated values
        #region foreach PROPERTY[S][CA]
        _ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA]
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA]
        if (entity._CALCULATED__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._CALCULATED__PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        Contracts.DTO._VO_CLASSNAMEDTO dto = new();

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][VO]
        if (entity._NONPERSISTED__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._NONPERSISTED__PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        #endregion foreach PROPERTY

        AdditionalConversionForDTO(entity, dto);

        Initialize(dto);

        return dto;
    }

    public Domain.ValueObjects._VO_CLASSNAME Convert(Contracts.DTO._VO_CLASSNAMEDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.ValueObjects._VO_CLASSNAME entity = Domain.ValueObjects.CLASSNAME.CreateEmpty();

        #region foreach PROPERTY[S][NO][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity, serviceProvider);

        return entity;
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(List<Domain.ValueObjects._VO_CLASSNAME> list)
    {
        return Convert(list, null, "", null);
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(List<Domain.ValueObjects._VO_CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (list == null)
            return null;
        List<Contracts.DTO._VO_CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.ValueObjects._VO_CLASSNAME> Convert(List<Contracts.DTO._VO_CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.ValueObjects._VO_CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(IEnumerable<Domain.ValueObjects._VO_CLASSNAME> collection)
    {
        return Convert(collection, null, "", null);
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(IEnumerable<Domain.ValueObjects._VO_CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (collection == null)
            return null;

        List<Contracts.DTO._VO_CLASSNAMEDTO> dtos = new();
        foreach (var entity in collection)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.ValueObjects._VO_CLASSNAME> Convert(IEnumerable<Contracts.DTO._VO_CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.ValueObjects._VO_CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

}
#endregion foreach MODEL

#region foreach MODEL[TR][EXP]
public partial class _TR_CLASSNAMEDTOMapper : IDTOMapper<Contracts.DTO._TR_CLASSNAMEDTO, Domain.Transients._TR_CLASSNAME>
{
    private readonly IServiceProvider serviceProvider;

    public _TR_CLASSNAMEDTOMapper(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    partial void PartialInitialize(Contracts.DTO._TR_CLASSNAMEDTO dto);

    public void Initialize(Contracts.DTO._TR_CLASSNAMEDTO dto)
    {
        PartialInitialize(dto);
    }

    partial void AdditionalConversionForEntity(Contracts.DTO._TR_CLASSNAMEDTO source, Domain.Transients._TR_CLASSNAME target);
    partial void AdditionalConversionForDTO(Domain.Transients._TR_CLASSNAME source, Contracts.DTO._TR_CLASSNAMEDTO target);

    public object ConvertExtension(object entity)
    {
        return ConvertExtension(entity as Domain.Transients._TR_CLASSNAME);
    }
    public object ConvertExtension(Domain.Transients._TR_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }
    public Contracts.DTO._TR_CLASSNAMEDTO Convert(Domain.Transients._TR_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }

    public Contracts.DTO._TR_CLASSNAMEDTO Convert(Domain.Transients._TR_CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        // Get Calculated values
        #region foreach PROPERTY[S][CA]
        _ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][CA]
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        Contracts.DTO._TR_CLASSNAMEDTO dto = new();

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        dto._PROPERTYNAME_Id = entity._PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][PUO][EN][AG]
        dto._REF__PROPERTYNAME_Id = entity._REF__PROPERTYNAME_Id.ToString();
        #endregion foreach PROPERTY

        AdditionalConversionForDTO(entity, dto);

        Initialize(dto);

        return dto;
    }

    public Domain.Transients._TR_CLASSNAME Convert(Contracts.DTO._TR_CLASSNAMEDTO dataTransferObject, IServiceProvider serviceProvider)
    {
        if (dataTransferObject == null)
            return null;

        Domain.Transients._TR_CLASSNAME entity = new();

        #region foreach PROPERTY[S][NO][NP][PUO]
        entity._SIMPLE__PROPERTYNAME_ = dataTransferObject._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][PUO]
        entity._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(dataTransferObject._ENUM__PROPERTYNAME_);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG][NAR]
        // EN NAR
        entity._PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(dataTransferObject._PROPERTYNAME_Id);
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NP][PUO][EN][AG][NAR]
        // EN NP NAR
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][N][NO][NP][PUO][EN][AG][AR]
        // EN AR
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][N][NO][NP][PUO][EN][AG][NAR]
        // EN
        entity._PROPERTYNAME_Id = dataTransferObject._PROPERTYNAME_Id != null ? IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(dataTransferObject._PROPERTYNAME_Id) : default;
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[R][NO][NP][PUO][EN][AG]
        // EN
        entity._REF__PROPERTYNAME_Id = IdentityKeyHelper<_IDENTITY_KEY_TYPE_>.ReadString(dataTransferObject._REF__PROPERTYNAME_Id);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][PUO][VO]
        // VO
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][PUO][TR]
        // TR
        entity._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(dataTransferObject._PROPERTYNAME_, serviceProvider);
        #endregion foreach PROPERTY

        AdditionalConversionForEntity(dataTransferObject, entity);

        return entity;
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(List<Domain.Transients._TR_CLASSNAME> list)
    {
        return Convert(list, null, "", null);
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(List<Domain.Transients._TR_CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (list == null)
            return null;
        List<Contracts.DTO._TR_CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.Transients._TR_CLASSNAME> Convert(List<Contracts.DTO._TR_CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Transients._TR_CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(IEnumerable<Domain.Transients._TR_CLASSNAME> collection)
    {
        return Convert(collection, null, "", null);
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(IEnumerable<Domain.Transients._TR_CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references)
    {
        if (collection == null)
            return null;

        List<Contracts.DTO._TR_CLASSNAMEDTO> dtos = new();
        foreach (var entity in collection)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }

        return dtos;
    }

    public List<Domain.Transients._TR_CLASSNAME> Convert(IEnumerable<Contracts.DTO._TR_CLASSNAMEDTO> dataTransferObjects, IServiceProvider serviceProvider)
    {
        if (dataTransferObjects == null)
            return null;

        List<Domain.Transients._TR_CLASSNAME> collection = new();
        foreach (var dto in dataTransferObjects)
        {
            var entity = Convert(dto, serviceProvider);
            if (entity != null)
                collection.Add(entity);
        }

        return collection;
    }

}
#endregion foreach MODEL