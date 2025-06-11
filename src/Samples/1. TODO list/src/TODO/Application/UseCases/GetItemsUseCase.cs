using SolidOps.TODO.Contracts.DTO;
using SolidOps.TODO.Presentation.Mappers;
using SolidOps.TODO.Shared.Domain.Results;

namespace SolidOps.TODO.Application.UseCases;

public partial class GetItemsUseCase
{
    public override async Task<IOpsResult<IEnumerable<ItemDTO>>> InternalExecute()
    {
        var result = await _dependencyItemRepository.GetListWhere(_ => true);
        
        var converter = new ItemDTOMapper(serviceProvider);
        var list = converter.Convert(result);
        return IOpsResult.Ok(list.AsEnumerable());
    }
}
