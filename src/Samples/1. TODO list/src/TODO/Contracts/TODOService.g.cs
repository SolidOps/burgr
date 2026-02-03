using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Shared;
namespace SolidOps.TODO.Contracts.Services;
// Service 
public partial interface IAddItemService
{

    Task<IOpsResult> Execute(SolidOps.TODO.Contracts.DTO.ItemDTO item);

}
public partial interface IUpdateItemService
{

    Task<IOpsResult> Execute(System.String id, System.String name, System.String status, System.DateTime dueDate);

}
public partial interface IGetItemsService
{

    Task<IOpsResult<IEnumerable<SolidOps.TODO.Contracts.DTO.ItemDTO>>> Execute();

}