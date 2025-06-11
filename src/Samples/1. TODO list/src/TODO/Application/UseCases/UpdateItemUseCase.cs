using SolidOps.TODO.Contracts.Enums;
using SolidOps.TODO.Shared.Domain.Results;

namespace SolidOps.TODO.Application.UseCases;

public partial class UpdateItemUseCase
{
    public override async Task<IOpsResult> InternalExecute(string id, string name, string status, DateTime dueDate)
    {
        var dbItem = await _dependencyItemRepository.GetSingleById(Guid.Parse(id));
        dbItem.DueDate = dueDate;
        dbItem.Status = Enum.Parse<ItemStatusEnum>(status);
        dbItem.Name = name;
        await _dependencyItemRepository.Update(dbItem);

        return IOpsResult.Ok();
    }
}
