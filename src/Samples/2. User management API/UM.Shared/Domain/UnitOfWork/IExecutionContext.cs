using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidOps.UM.Shared.Contracts.Endpoints;
using SolidOps.UM.Shared.Domain.Entities;
using System.Data.Common;

namespace SolidOps.UM.Shared.Domain.UnitOfWork;

public interface IExecutionContext : IDisposable
{
    public const string ALLRIGHT = "*";
    public const string ROOTUSER = "root";
    public const string WILDCARD = ALLRIGHT;
    public const string TEMPORARY_DATA_TOKEN = "token";

    string Authorization { get; }
    
    string UserId { get; }

    List<string> Rights { get; }

    bool HasRight(string right);

    void SetUserId(Func<(string userId, List<string> rights, string authorization)> callback);

    IUnitOfWork StartUnitOfWork(string moduleName, string unitOfWorfName, UnitOfWorkType unitOfWorkType);

    IUnitOfWork CurrentUnitOfWork { get; set; }
    
    Dictionary<string, object> TemporaryData { get; set; }

    List<string> MandatoryRights { get; set; }

    List<string> OwnershipOverrideRights { get; set; }
}
