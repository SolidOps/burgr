
using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Contracts.UseCases;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain.Results;

namespace MetaCorp.Template.Presentation;

public partial class TemplateConsoleSession
{
    #region static
    public static bool IsCommand(string command)
    {
        #region foreach DOMAIN_USECASE
        #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
        if(command == "USECASENAME__DOVOIDACTION_")
        {
            return true;
        }
        #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN

        #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
        if(command == "USECASENAME__DOIDENTITYACTION_")
        {
            return true;
        }
        #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN

        #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
        if(command == "USECASENAME__DOSIMPLEACTION_")
        {
            return true;
        }
        #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN

        #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
        if(command == "USECASENAME__DOMODELACTION_")
        {
            return true;
        }
        #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

        #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
        if(command == "USECASENAME__DOMODELLISTACTION_")
        {
            return true;
        }
        #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
        
        #endregion foreach DOMAIN_USECASE
        return false;
    }
    public static void DisplayCommands()
    {
        #region foreach DOMAIN_USECASE
        #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
        Console.WriteLine("USECASENAME__DOVOIDACTION_: _DOVOIDACTION_ for USECASENAME use case");
        #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN

        #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
        Console.WriteLine("USECASENAME__DOIDENTITYACTION_: _DOIDENTITYACTION_ for USECASENAME use case");
        #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN

        #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
        Console.WriteLine("USECASENAME__DOSIMPLEACTION_: _DOSIMPLEACTION_ for USECASENAME use case");
        #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN

        #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
        Console.WriteLine("USECASENAME__DOMODELACTION_: _DOMODELACTION_ for USECASENAME use case");
        #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

        #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
        Console.WriteLine("USECASENAME__DOMODELLISTACTION_: _DOMODELLISTACTION_ for USECASENAME use case");
        #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
        
        #endregion foreach DOMAIN_USECASE
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
            #region foreach DOMAIN_USECASE
            #region foreach STEP_IN_USECASE_WITH_VOID_RETURN
            if(command == "USECASENAME__DOVOIDACTION_")
            {
                Console.WriteLine("_DOVOIDACTION_ for USECASENAME use case called");
                var useCase = serviceProvider.GetRequiredService<IUSECASENAMEUseCase>();

                #region foreach USECASE_STEP_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                #region foreach USECASE_STEP_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                var result = await useCase._DOVOIDACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach STEP_IN_USECASE_WITH_VOID_RETURN

            #region foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN
            if(command == "USECASENAME__DOIDENTITYACTION_")
            {
                Console.WriteLine("_DOIDENTITYACTION_ for USECASENAME use case called");
                var useCase = serviceProvider.GetRequiredService<IUSECASENAMEUseCase>();
            
                #region foreach USECASE_STEP_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                #region foreach USECASE_STEP_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                var result = await useCase._DOIDENTITYACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach STEP_IN_USECASE_WITH_IDENTITY_RETURN

            #region foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN
            if(command == "USECASENAME__DOSIMPLEACTION_")
            {
                Console.WriteLine("_DOSIMPLEACTION_ for USECASENAME use case called");
                var useCase = serviceProvider.GetRequiredService<IUSECASENAMEUseCase>();
            
                #region foreach USECASE_STEP_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                #region foreach USECASE_STEP_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                var result = await useCase._DOSIMPLEACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach STEP_IN_USECASE_WITH_SIMPLE_RETURN

            #region foreach STEP_IN_USECASE_WITH_MODEL_RETURN
            if(command == "USECASENAME__DOMODELACTION_")
            {
                Console.WriteLine("_DOMODELACTION_ for USECASENAME use case called");
                var useCase = serviceProvider.GetRequiredService<IUSECASENAMEUseCase>();
            
                #region foreach USECASE_STEP_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                #region foreach USECASE_STEP_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                var result = await useCase._DOMODELACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }
                Console.WriteLine(result.Data.ToString());
                return;
            }
            #endregion foreach STEP_IN_USECASE_WITH_MODEL_RETURN

            #region foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
            if(command == "USECASENAME__DOMODELLISTACTION_")
            {
                Console.WriteLine("_DOMODELLISTACTION_ for USECASENAME use case called");
                var useCase = serviceProvider.GetRequiredService<IUSECASENAMEUseCase>();
            
                #region foreach USECASE_STEP_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                #region foreach USECASE_STEP_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach USECASE_STEP_PARAMETER
            
                var result = await useCase._DOMODELLISTACTION_(/*PARAMETERS*/);
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
            #endregion foreach STEP_IN_USECASE_WITH_MODEL_LIST_RETURN
        
            #endregion foreach DOMAIN_USECASE
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

#region foreach MODEL[AG][EN]
public static class CLASSNAMEConsoleReader {
    public static IOpsResult<Contracts.DTO.CLASSNAMEDTO> ReadFromInput() {
        #region foreach PROPERTY[S][NO]
        Console.WriteLine("_SIMPLE__PROPERTYNAME_:");
        var _SIMPLE__PROPERTYNAME_Result = DisplayConverter.ConvertTo_PROPERTYTYPE_(Console.ReadLine());
        if(_SIMPLE__PROPERTYNAME_Result.HasError)
        {
            Console.WriteLine(_SIMPLE__PROPERTYNAME_Result.Error);
            return _SIMPLE__PROPERTYNAME_Result.ToResult<Contracts.DTO.CLASSNAMEDTO>();
        }
        var _SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_Result.Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[E][NO]
        Console.WriteLine("_ENUM__PROPERTYNAME_:");
        var _ENUM__PROPERTYNAME_Result = DisplayConverter.ConvertToInt32(Console.ReadLine());
        if(_ENUM__PROPERTYNAME_Result.HasError)
        {
            Console.WriteLine(_ENUM__PROPERTYNAME_Result.Error);
            return _SIMPLE__PROPERTYNAME_Result.ToResult<Contracts.DTO.CLASSNAMEDTO>();
        }
        var _ENUM__PROPERTYNAME_ = (Contracts.Enums._ENUMTYPE_Enum)_ENUM__PROPERTYNAME_Result.Data;
        #endregion foreach PROPERTY

        #region foreach PROPERTY[M][R][NO][EN][AG]
        var _PROPERTYNAME_IdResult = DisplayConverter.ConvertTo_IDENTITY_KEY_TYPE_(Console.ReadLine());
        if(_PROPERTYNAME_IdResult.HasError)
        {
            Console.WriteLine(_PROPERTYNAME_IdResult.Error);
            return _SIMPLE__PROPERTYNAME_Result.ToResult<Contracts.DTO.CLASSNAMEDTO>();
        }
        var _PROPERTYNAME_Id = _PROPERTYNAME_IdResult.Data;
        #endregion foreach PROPERTY

        var dto = new Contracts.DTO.CLASSNAMEDTO();
        #region foreach PROPERTY[S][NO]
        dto._SIMPLE__PROPERTYNAME_ = _SIMPLE__PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[E][NO]
        dto._ENUM__PROPERTYNAME_ = _ENUM__PROPERTYNAME_;
        #endregion foreach PROPERTY
        #region foreach PROPERTY[M][R][NO][EN][AG]
        dto._PROPERTYNAME_Id = _PROPERTYNAME_Id.ToString();
        #endregion foreach PROPERTY

        return IOpsResult.Ok(dto);
    }
}
#endregion foreach MODEL