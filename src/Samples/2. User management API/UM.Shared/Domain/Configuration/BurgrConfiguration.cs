using Microsoft.Extensions.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;

namespace SolidOps.UM.Shared.Domain.Configuration;

public class BurgrConfiguration
{
    private readonly IConfiguration configuration;

    private const string SERVICECONFIGKEY = "Burgr";
    private const string SERVICECONFIGPATH = $"{SERVICECONFIGKEY}:";

    public BurgrConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string SmtpServer { get { return configuration[$"{SERVICECONFIGPATH}SmtpServer"]; } }
    public int SmtpPort
    {
        get
        {
            return ParseInt(configuration[$"{SERVICECONFIGPATH}SmtpPort"]);
        }
    }

    public string SmtpUserName { get { return configuration[$"{SERVICECONFIGPATH}SmtpUserName"]; } }

    public string SmtpPassword { get { return configuration[$"{SERVICECONFIGPATH}SmtpPassword"]; } }

    public bool SendErrorToClient { get { return configuration[$"{SERVICECONFIGPATH}SendErrorToClient"] == "True"; } }

    public int TokenDurationInMinutes
    {
        get
        {
            return ParseInt(configuration[$"{SERVICECONFIGPATH}TokenDurationInMinutes"], 30);
        }
    }

    public int TechnicalTokenDurationInMinutes
    {
        get
        {
            return ParseInt(configuration[$"{SERVICECONFIGPATH}TechnicalTokenDurationInMinutes"], 300000);
        }
    }

    public bool EnableSelfUserCreation { get { return configuration[$"{SERVICECONFIGPATH}EnableSelfUserCreation"] == "True"; } }

    public bool EnableSwagger { get { return configuration[$"{SERVICECONFIGPATH}EnableSwagger"] == "True"; } }

    public string Origins { get { return configuration[$"{SERVICECONFIGPATH}Origins"]; } }

    public Dictionary<string, DataAccessFactoryInfo> DataAccessFactories
    {
        get
        {
            var children = configuration
                .GetSection(SERVICECONFIGKEY)
                .GetChildren()
                .Single(c => c.Key == "DataAccessFactories")
                .GetChildren();

            var dic = new Dictionary<string, DataAccessFactoryInfo>();
            foreach (var child in children)
            {
                var key = child.Key;
                var items = child.GetChildren().ToDictionary(x => x.Key, x => x.Value);
                dic.Add(key, new DataAccessFactoryInfo()
                {
                    DataAccessFactory = items.ContainsKey("DataAccessFactory") ? items["DataAccessFactory"] : null,
                    Database = items.ContainsKey("Database") ? items["Database"] : null
                });
            }

            return dic;
        }
    }

    public Dictionary<string, DatabaseInfo> Databases
    {
        get
        {
            var children = configuration
                .GetSection(SERVICECONFIGKEY)
                .GetChildren()
                .Single(c => c.Key == "Databases")
                .GetChildren();

            var dic = new Dictionary<string, DatabaseInfo>();
            foreach (var child in children)
            {
                var key = child.Key;
                var items = child.GetChildren().ToDictionary(x => x.Key, x => x.Value);
                dic.Add(key, new DatabaseInfo()
                {
                    ConnectionString = items.ContainsKey("ConnectionString") ? items["ConnectionString"] : null,
                    LogRequests = items.ContainsKey("LogRequests") ? bool.Parse(items["LogRequests"]) : default,
                    DataCommandTimeout = items.ContainsKey("DataCommandTimeout") ? int.Parse(items["DataCommandTimeout"]) : default
                });
            }

            return dic;
        }
    }

    public Dictionary<string, string> Endpoints
    {
        get
        {
            return configuration
                .GetSection(SERVICECONFIGKEY)
                .GetChildren()
                .Single(c => c.Key == "Endpoints")
                .GetChildren().ToDictionary(x => x.Key, x => x.Value);
        }
    }

    private int ParseInt(string value, int defaultValue = default)
    {
        int outValue;
        if (int.TryParse(value, out outValue))
            return outValue;
        return defaultValue;
    }
}

public class DataAccessFactoryInfo
{
    public string DataAccessFactory { get; set; }
    public string Database { get; set; }
    public IDataAccessFactory Instance { get; set; }
}

public class DatabaseInfo
{
    public string ConnectionString { get; set; }
    public bool LogRequests { get; internal set; }
    public int DataCommandTimeout { get; internal set; }
}

public enum EventOption
{
    Disabled = 0,
    SignalR = 1,
    Internal = 2
}
