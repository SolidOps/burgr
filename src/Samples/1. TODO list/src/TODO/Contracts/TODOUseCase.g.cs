using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;
namespace SolidOps.TODO.Contracts.UseCases;
// UseCase [I]
public partial interface IAddItemUseCase
{

    Task<IOpsResult> Execute(SolidOps.TODO.Contracts.DTO.ItemDTO item);

}
public partial interface IUpdateItemUseCase
{

    Task<IOpsResult> Execute(System.String id, System.String name, System.String status, System.DateTime dueDate);

}
public partial interface IGetItemsUseCase
{

    Task<IOpsResult<IEnumerable<SolidOps.TODO.Contracts.DTO.ItemDTO>>> Execute();

}