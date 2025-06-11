using SolidOps.TODO.Contracts.DTO;
using SolidOps.TODO.Domain.AggregateRoots;
using SolidOps.TODO.Shared.Domain.Results;

namespace SolidOps.TODO.Application.UseCases;

public partial class AddItemUseCase
{
    public override async Task<IOpsResult> InternalExecute(ItemDTO item)
    {
        var newItem = Item.Create(item.Name, item.DueDate, item.Status);
        return await _dependencyItemRepository.Add(newItem);
    }
}
