
using Microsoft.Extensions.DependencyInjection;
using SolidOps.TODO.Contracts.Services;
using SolidOps.TODO.Shared;
using SolidOps.TODO.Shared.Domain.Results;

namespace MetaCorp.Template.Presentation;

public partial class TemplateConsoleSession
{
    #region static
    public static bool IsCommand(string command)
    {
        #region foreach DOMAIN_SERVICE
        #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN[EXT]
        if(command == "SERVICENAME__DOVOIDACTION_")
        {
            return true;
        }
        #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN[EXT]
        if(command == "SERVICENAME__DOIDENTITYACTION_")
        {
            return true;
        }
        #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN[EXT]
        if(command == "SERVICENAME__DOSIMPLEACTION_")
        {
            return true;
        }
        #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN[EXT]
        if(command == "SERVICENAME__DOMODELACTION_")
        {
            return true;
        }
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN[EXT]
        if(command == "SERVICENAME__DOMODELLISTACTION_")
        {
            return true;
        }
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
        
        #endregion foreach DOMAIN_SERVICE
        return false;
    }
    public static void DisplayCommands()
    {
        #region foreach DOMAIN_SERVICE
        #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN[EXT]
        Console.WriteLine("SERVICENAME__DOVOIDACTION_: _DOVOIDACTION_ for SERVICENAME service");
        #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN[EXT]
        Console.WriteLine("SERVICENAME__DOIDENTITYACTION_: _DOIDENTITYACTION_ for SERVICENAME service");
        #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN[EXT]
        Console.WriteLine("SERVICENAME__DOSIMPLEACTION_: _DOSIMPLEACTION_ for SERVICENAME service");
        #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN[EXT]
        Console.WriteLine("SERVICENAME__DOMODELACTION_: _DOMODELACTION_ for SERVICENAME service");
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

        #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN[EXT]
        Console.WriteLine("SERVICENAME__DOMODELLISTACTION_: _DOMODELLISTACTION_ for SERVICENAME service");
        #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
        
        #endregion foreach DOMAIN_SERVICE
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
            #region foreach DOMAIN_SERVICE
            #region foreach METHOD_IN_SERVICE_WITH_VOID_RETURN[EXT]
            if(command == "SERVICENAME__DOVOIDACTION_")
            {
                Console.WriteLine("_DOVOIDACTION_ for SERVICENAME service called");
                var service = serviceProvider.GetRequiredService<ISERVICENAMEService>();

                #region foreach SERVICE_METHOD_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                #region foreach SERVICE_METHOD_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                var result = await service._DOVOIDACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach METHOD_IN_SERVICE_WITH_VOID_RETURN

            #region foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN[EXT]
            if(command == "SERVICENAME__DOIDENTITYACTION_")
            {
                Console.WriteLine("_DOIDENTITYACTION_ for SERVICENAME service called");
                var service = serviceProvider.GetRequiredService<ISERVICENAMEService>();
            
                #region foreach SERVICE_METHOD_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                #region foreach SERVICE_METHOD_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                var result = await service._DOIDENTITYACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach METHOD_IN_SERVICE_WITH_IDENTITY_RETURN

            #region foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN[EXT]
            if(command == "SERVICENAME__DOSIMPLEACTION_")
            {
                Console.WriteLine("_DOSIMPLEACTION_ for SERVICENAME service called");
                var service = serviceProvider.GetRequiredService<ISERVICENAMEService>();
            
                #region foreach SERVICE_METHOD_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                #region foreach SERVICE_METHOD_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                var result = await service._DOSIMPLEACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }

                return;
            }
            #endregion foreach METHOD_IN_SERVICE_WITH_SIMPLE_RETURN

            #region foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN[EXT]
            if(command == "SERVICENAME__DOMODELACTION_")
            {
                Console.WriteLine("_DOMODELACTION_ for SERVICENAME service called");
                var service = serviceProvider.GetRequiredService<ISERVICENAMEService>();
            
                #region foreach SERVICE_METHOD_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                #region foreach SERVICE_METHOD_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                var result = await service._DOMODELACTION_(/*PARAMETERS*/);
                if (result.HasError)
                {
                    Console.WriteLine("an error occured");
                    return;
                }
                Console.WriteLine(result.Data.ToString());
                return;
            }
            #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_RETURN

            #region foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN[EXT]
            if(command == "SERVICENAME__DOMODELLISTACTION_")
            {
                Console.WriteLine("_DOMODELLISTACTION_ for SERVICENAME service called");
                var service = serviceProvider.GetRequiredService<ISERVICENAMEService>();
            
                #region foreach SERVICE_METHOD_PARAMETER[S]
                Console.WriteLine("_PARAMETER_:");
                 var _PARAMETER_Result = DisplayConverter.ConvertTo_SHORTPARAMTYPE_(Console.ReadLine());
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                #region foreach SERVICE_METHOD_PARAMETER[M]
                Console.WriteLine("_PARAMETER_:");
                var _PARAMETER_Result = _SHORTPARAMTYPE_ConsoleReader.ReadFromInput();
                if(_PARAMETER_Result.HasError)
                {
                    Console.WriteLine(_PARAMETER_Result.Error);
                    return;
                }
                var _PARAMETER_ = _PARAMETER_Result.Data;
                #endregion foreach SERVICE_METHOD_PARAMETER
            
                var result = await service._DOMODELLISTACTION_(/*PARAMETERS*/);
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
            #endregion foreach METHOD_IN_SERVICE_WITH_MODEL_LIST_RETURN
        
            #endregion foreach DOMAIN_SERVICE
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