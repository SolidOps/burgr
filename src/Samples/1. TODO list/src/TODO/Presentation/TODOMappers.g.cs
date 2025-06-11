using SolidOps.TODO.Shared;
namespace SolidOps.TODO.Presentation.Mappers;
// Object [AG][EXP]
public partial class ItemDTOMapper : IDTOMapper<Contracts.DTO.ItemDTO, Domain.AggregateRoots.Item>
{
    private readonly IServiceProvider serviceProvider;
    public ItemDTOMapper(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    partial void PartialInitialize(Contracts.DTO.ItemDTO dto);
    public void Initialize(Contracts.DTO.ItemDTO dto)
    {
        PartialInitialize(dto);
    }
    partial void AdditionalConversionForDTO(Domain.AggregateRoots.Item source, Contracts.DTO.ItemDTO target);
    public Contracts.DTO.ItemDTO Convert(Domain.AggregateRoots.Item entity, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(entity, null, "", references);
    }
    public Contracts.DTO.ItemDTO Convert(Domain.AggregateRoots.Item entity, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (entity == null)
            return null;
        if (preventLazyLoading == null)
            preventLazyLoading = new List<string>();
        if (references == null)
            references = new Dictionary<string, Dictionary<string, object>>();
        preventLazyLoading.Add("Item|" + entity.Id);
        Contracts.DTO.ItemDTO dto = new();

        dto.Id = entity.Id.ToString();

        if (!references.ContainsKey("Item"))
        {
            references.Add("Item", new Dictionary<string, object>());
        }
        if (references["Item"].ContainsKey(dto.Id))
        {
            return references["Item"][dto.Id] as Contracts.DTO.ItemDTO;
        }
        references["Item"].Add(dto.Id, dto);
        // Get Calculated values
        // Property [S][CA]
        _ = entity.RemainingDays;

        // Property [S][NO][NP][CA][PUO]
        dto.Name = entity.Name;

        dto.DueDate = entity.DueDate;

        dto.RemainingDays = entity.RemainingDays;

        // Property [E][NO][NP][CA][PUO]
        dto.Status = DisplayConverter.GetEnumData<SolidOps.TODO.Contracts.Enums.ItemStatusEnum, SolidOps.TODO.Contracts.Enums.ItemStatusEnum, SolidOps.TODO.Contracts.Enums.ItemStatusEnum>(entity.Status).Data;

        AdditionalConversionForDTO(entity, dto);
        Initialize(dto);
        return dto;
    }
    public List<Contracts.DTO.ItemDTO> Convert(List<Domain.AggregateRoots.Item> list, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(list, null, "", references);
    }
    public List<Contracts.DTO.ItemDTO> Convert(List<Domain.AggregateRoots.Item> list, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (list == null)
            return null;
        List<Contracts.DTO.ItemDTO> dtos = new();
        foreach (var entity in list)
        {
            if (!preventLazyLoading.Contains("Item|" + entity.Id))
            {
                var dto = Convert(entity, preventLazyLoading, parentPath, references);
                if (dto != null)
                    dtos.Add(dto);
            }
        }
        return dtos;
    }
    public List<Contracts.DTO.ItemDTO> Convert(IEnumerable<Domain.AggregateRoots.Item> collection, Dictionary<string, Dictionary<string, object>> references = null)
    {
        return Convert(collection, null, "", references);
    }
    public List<Contracts.DTO.ItemDTO> Convert(IEnumerable<Domain.AggregateRoots.Item> collection, List<string> preventLazyLoading, string parentPath, Dictionary<string, Dictionary<string, object>> references = null)
    {
        if (collection == null)
            return null;
        List<Contracts.DTO.ItemDTO> dtos = new();
        foreach (var entity in collection)
        {
            var dto = Convert(entity, preventLazyLoading, parentPath, references);
            if (dto != null)
                dtos.Add(dto);
        }
        return dtos;
    }
}