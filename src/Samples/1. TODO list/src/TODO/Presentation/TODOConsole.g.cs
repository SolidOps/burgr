using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Contracts.UseCases;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain.Results;
namespace SolidOps.TODO.Presentation;
public partial class TODOConsoleSession
{
    #region static
    public static bool IsCommand(string command)
    {
        // UseCase 

        if(command == "AddItem_Execute")
        {
            return true;
        }

        if(command == "UpdateItem_Execute")
        {
            return true;
        }

        if(command == "GetItems_Execute")
        {
            return true;
        }

        return false;
    }
    public static void DisplayCommands()
    {
        // UseCase 

        Console.WriteLine("AddItem_Execute: Execute for AddItem use case");

        Console.WriteLine("UpdateItem_Execute: Execute for UpdateItem use case");

        Console.WriteLine("GetItems_Execute: Execute for GetItems use case");

    }
    #endregion
    private readonly IServiceProvider serviceProvider;
    public TODOConsoleSession(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task Clear()
    {
        await Task.CompletedTask;
    }
    public async Task Call(string command)
    {
        try
        {
            // UseCase 

            if(command == "AddItem_Execute")
            {
                Console.WriteLine("Execute for AddItem use case called");
                var useCase = serviceProvider.GetRequiredService<IAddItemUseCase>();

                // UseCaseStepParameter [M]
                Console.WriteLine("item:");
                var itemResult = ItemConsoleReader.ReadFromInput();
                if(itemResult.HasError)
                {
                    Console.WriteLine(itemResult.Error);
                    return;
                }
                var item = itemResult.Data;

                var result = await useCase.Execute(item);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }
                return;
            }

            if(command == "UpdateItem_Execute")
            {
                Console.WriteLine("Execute for UpdateItem use case called");
                var useCase = serviceProvider.GetRequiredService<IUpdateItemUseCase>();
                // UseCaseStepParameter [S]
                Console.WriteLine("id:");
                var idResult = DisplayConverter.ConvertToString(Console.ReadLine());
                if(idResult.HasError)
                {
                    Console.WriteLine(idResult.Error);
                    return;
                }
                var id = idResult.Data;

                Console.WriteLine("name:");
                var nameResult = DisplayConverter.ConvertToString(Console.ReadLine());
                if(nameResult.HasError)
                {
                    Console.WriteLine(nameResult.Error);
                    return;
                }
                var name = nameResult.Data;

                Console.WriteLine("status:");
                var statusResult = DisplayConverter.ConvertToString(Console.ReadLine());
                if(statusResult.HasError)
                {
                    Console.WriteLine(statusResult.Error);
                    return;
                }
                var status = statusResult.Data;

                Console.WriteLine("dueDate:");
                var dueDateResult = DisplayConverter.ConvertToDateTime(Console.ReadLine());
                if(dueDateResult.HasError)
                {
                    Console.WriteLine(dueDateResult.Error);
                    return;
                }
                var dueDate = dueDateResult.Data;

                var result = await useCase.Execute(id, name, status, dueDate);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }
                return;
            }

            if(command == "GetItems_Execute")
            {
                Console.WriteLine("Execute for GetItems use case called");
                var useCase = serviceProvider.GetRequiredService<IGetItemsUseCase>();

                var result = await useCase.Execute();
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }
                foreach(var item in result.Data)
                {
                    Console.WriteLine(item.ToString());
                }
                return;
            }

        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
// Object [AG][EN]
public static class ItemConsoleReader {
    public static IOpsResult<Contracts.DTO.ItemDTO> ReadFromInput() {
        // Property [S][NO]
        Console.WriteLine("Name:");
        var NameResult = DisplayConverter.ConvertToString(Console.ReadLine());
        if(NameResult.HasError)
        {
            Console.WriteLine(NameResult.Error);
            return NameResult.ToResult<Contracts.DTO.ItemDTO>();
        }
        var Name = NameResult.Data;

        Console.WriteLine("DueDate:");
        var DueDateResult = DisplayConverter.ConvertToDateTime(Console.ReadLine());
        if(DueDateResult.HasError)
        {
            Console.WriteLine(DueDateResult.Error);
            return DueDateResult.ToResult<Contracts.DTO.ItemDTO>();
        }
        var DueDate = DueDateResult.Data;

        // Property [E][NO]
        Console.WriteLine("Status:");
        var StatusResult = DisplayConverter.ConvertToInt32(Console.ReadLine());
        if(StatusResult.HasError)
        {
            Console.WriteLine(StatusResult.Error);
            return StatusResult.ToResult<Contracts.DTO.ItemDTO>();
        }
        var Status = (Contracts.Enums.ItemStatusEnum)StatusResult.Data;

        var dto = new Contracts.DTO.ItemDTO();
        // Property [S][NO]
        dto.Name = Name;

        dto.DueDate = DueDate;

        // Property [E][NO]
        dto.Status = Status;

        return IOpsResult.Ok(dto);
    }
}