using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.TODO.Shared.Domain.Results;
using SolidOps.TODO.Contracts.Services;
using SolidOps.TODO.Shared;
namespace SolidOps.TODO.Application.Services;
// Service 
public partial class AddItemService : BaseAddItemService, IAddItemService
{
    private readonly IUserContext userContext;
    // Dependency [EN][AG]
    private readonly SolidOps.TODO.Domain.Repositories.IItemRepository _dependencyItemRepository;

    public AddItemService(IUserContext userContext, IServiceProvider serviceProvider
    // Dependency [EN][AG]
        , SolidOps.TODO.Domain.Repositories.IItemRepository dependencyItemRepository

        ) : base(serviceProvider)
    {
        this.userContext = userContext;
        // Dependency [EN][AG]
        _dependencyItemRepository = dependencyItemRepository ?? throw new ArgumentNullException(nameof(dependencyItemRepository));

    }

    public virtual async Task<IOpsResult> Execute(SolidOps.TODO.Contracts.DTO.ItemDTO item)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var result = await InternalExecute(item);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }

}
public partial class UpdateItemService : BaseUpdateItemService, IUpdateItemService
{
    private readonly IUserContext userContext;
    // Dependency [EN][AG]
    private readonly SolidOps.TODO.Domain.Repositories.IItemRepository _dependencyItemRepository;

    public UpdateItemService(IUserContext userContext, IServiceProvider serviceProvider
    // Dependency [EN][AG]
        , SolidOps.TODO.Domain.Repositories.IItemRepository dependencyItemRepository

        ) : base(serviceProvider)
    {
        this.userContext = userContext;
        // Dependency [EN][AG]
        _dependencyItemRepository = dependencyItemRepository ?? throw new ArgumentNullException(nameof(dependencyItemRepository));

    }

    public virtual async Task<IOpsResult> Execute(System.String id, System.String name, System.String status, System.DateTime dueDate)
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Write);
        var result = await InternalExecute(id, name, status, dueDate);
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }

}
public partial class GetItemsService : BaseGetItemsService, IGetItemsService
{
    private readonly IUserContext userContext;
    // Dependency [EN][AG]
    private readonly SolidOps.TODO.Domain.Repositories.IItemRepository _dependencyItemRepository;

    public GetItemsService(IUserContext userContext, IServiceProvider serviceProvider
    // Dependency [EN][AG]
        , SolidOps.TODO.Domain.Repositories.IItemRepository dependencyItemRepository

        ) : base(serviceProvider)
    {
        this.userContext = userContext;
        // Dependency [EN][AG]
        _dependencyItemRepository = dependencyItemRepository ?? throw new ArgumentNullException(nameof(dependencyItemRepository));

    }

    public virtual async Task<IOpsResult<IEnumerable<SolidOps.TODO.Contracts.DTO.ItemDTO>>> Execute()
    {
        using var unitOfWork = userContext.StartUnitOfWork(UnitOfWorkType.Read);
        var result = await InternalExecute();
        if(result.HasError)
        {
            return result;
        }
        await unitOfWork.Complete();
        return result;
    }

}
// Service 
public abstract class BaseAddItemService
{
    protected readonly IServiceProvider serviceProvider;
    public BaseAddItemService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult> InternalExecute(SolidOps.TODO.Contracts.DTO.ItemDTO item)
    {
        throw new NotImplementedException("Execute");
    }

}
public abstract class BaseUpdateItemService
{
    protected readonly IServiceProvider serviceProvider;
    public BaseUpdateItemService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult> InternalExecute(System.String id, System.String name, System.String status, System.DateTime dueDate)
    {
        throw new NotImplementedException("Execute");
    }

}
public abstract class BaseGetItemsService
{
    protected readonly IServiceProvider serviceProvider;
    public BaseGetItemsService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected T GetService<T>()
    {
        return this.serviceProvider.GetRequiredService<T>();
    }

    public virtual Task<IOpsResult<IEnumerable<SolidOps.TODO.Contracts.DTO.ItemDTO>>> InternalExecute()
    {
        throw new NotImplementedException("Execute");
    }

}