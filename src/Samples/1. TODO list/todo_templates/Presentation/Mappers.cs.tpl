using SolidOps.TODO.Shared;

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

    partial void AdditionalConversionForDTO(Domain.Entities.CLASSNAME source, Contracts.DTO.CLASSNAMEDTO target);

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.Entities.CLASSNAME entity, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(entity, null, "", references);
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.Entities.CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        if (references == null)
            references = new Dictionary<string, Dictionary<string, object>>();

        preventLazyLoading.Add("CLASSNAME|" + entity.Id);

        Contracts.DTO.CLASSNAMEDTO dto = new();
        #region to remove if PRIVATE_ID
        dto.Id = entity.Id.ToString();
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
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_") && !preventLazyLoading.Contains("_PROPERTYTYPE_|" + entity._PROPERTYNAME_.Id))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        dto._PROPERTYNAME_Id = entity._PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.Entities.CLASSNAME> list, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(list, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.Entities.CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (list == null)
            return null;
        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            if (!preventLazyLoading.Contains("CLASSNAME|" + entity.Id))
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
        }

        return dtos;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.Entities.CLASSNAME> collection, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(collection, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.Entities.CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

    partial void AdditionalConversionForDTO(Domain.AggregateRoots.CLASSNAME source, Contracts.DTO.CLASSNAMEDTO target);

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.AggregateRoots.CLASSNAME entity, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(entity, null, "", references);
    }

    public Contracts.DTO.CLASSNAMEDTO Convert(Domain.AggregateRoots.CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (entity == null)
            return null;

        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();

        if (references == null)
            references = new Dictionary<string, Dictionary<string, object>>();

        preventLazyLoading.Add("CLASSNAME|" + entity.Id);

        Contracts.DTO.CLASSNAMEDTO dto = new();
        #region to remove if PRIVATE_ID
        dto.Id = entity.Id.ToString();
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
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][CA][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_") && !preventLazyLoading.Contains("_PROPERTYTYPE_|" + entity._PROPERTYNAME_.Id))
        {
            _ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        }
        #endregion foreach PROPERTY

        #region foreach PROPERTY[S][NO][NP][CA][PUO]
        dto._SIMPLE__PROPERTYNAME_ = entity._SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO][NP][CA][PUO]
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][PUO][EN][AG]
        dto._PROPERTYNAME_Id = entity._PROPERTYNAME_Id/*_ISNULL_*/.ToString();
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][EN][AG][TR][NAR]
        if (entity._PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
        {
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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
            if (references.ContainsKey("_PROPERTYTYPE_") && references["_PROPERTYTYPE_"].ContainsKey(entity._PROPERTYNAME_.Id))
            {
                dto._PROPERTYNAME_ = references["_PROPERTYTYPE_"][entity._PROPERTYNAME_.Id] as DEPENDENCYNAMESPACE.Contracts.DTO._PROPERTYTYPE_DTO;
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

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.AggregateRoots.CLASSNAME> list, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(list, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(List<Domain.AggregateRoots.CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (list == null)
            return null;
        List<Contracts.DTO.CLASSNAMEDTO> dtos = new();
        foreach (var entity in list)
        {
            if (!preventLazyLoading.Contains("CLASSNAME|" + entity.Id))
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
        }

        return dtos;
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.AggregateRoots.CLASSNAME> collection, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(collection, null, "", references);
    }

    public List<Contracts.DTO.CLASSNAMEDTO> Convert(IEnumerable<Domain.AggregateRoots.CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

    partial void AdditionalConversionForDTO(Domain.ValueObjects._VO_CLASSNAME source, Contracts.DTO._VO_CLASSNAMEDTO target);

    public Contracts.DTO._VO_CLASSNAMEDTO Convert(Domain.ValueObjects._VO_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }

    public Contracts.DTO._VO_CLASSNAMEDTO Convert(Domain.ValueObjects._VO_CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, Contracts.Enums._ENUMTYPE_Enum, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
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
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][NO][NP][CA][PUO][VO]
        if (entity._NONPERSISTED__PROPERTYNAME_ != null && !preventLazyLoading.Contains(parentPath + "_PROPERTYNAME_"))
            dto._PROPERTYNAME_ = new _PROPERTYTYPE_DTOMapper(serviceProvider).Convert(entity._NONPERSISTED__PROPERTYNAME_, preventLazyLoading, parentPath + "_PROPERTYNAME_" + ".", references);
        #endregion foreach PROPERTY

        AdditionalConversionForDTO(entity, dto);

        Initialize(dto);

        return dto;
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(List<Domain.ValueObjects._VO_CLASSNAME> list)
    {
        return Convert(list, null, "", null);
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(List<Domain.ValueObjects._VO_CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(IEnumerable<Domain.ValueObjects._VO_CLASSNAME> collection)
    {
        return Convert(collection, null, "", null);
    }

    public List<Contracts.DTO._VO_CLASSNAMEDTO> Convert(IEnumerable<Domain.ValueObjects._VO_CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

    partial void AdditionalConversionForDTO(Domain.Transients._TR_CLASSNAME source, Contracts.DTO._TR_CLASSNAMEDTO target);

    public Contracts.DTO._TR_CLASSNAMEDTO Convert(Domain.Transients._TR_CLASSNAME entity)
    {
        return Convert(entity, null, "", null);
    }

    public Contracts.DTO._TR_CLASSNAMEDTO Convert(Domain.Transients._TR_CLASSNAME entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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
        _ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
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
        dto._ENUM__PROPERTYNAME_ = DisplayConverter.GetEnumData/*_ISLIST_*/<DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_, DEPENDENCYNAMESPACE.Contracts.Enums._ENUMNULLABLETYPE_>(entity._ENUM__PROPERTYNAME_).Data;
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

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(List<Domain.Transients._TR_CLASSNAME> list)
    {
        return Convert(list, null, "", null);
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(List<Domain.Transients._TR_CLASSNAME> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(IEnumerable<Domain.Transients._TR_CLASSNAME> collection)
    {
        return Convert(collection, null, "", null);
    }

    public List<Contracts.DTO._TR_CLASSNAMEDTO> Convert(IEnumerable<Domain.Transients._TR_CLASSNAME> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
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

}
#endregion foreach MODEL