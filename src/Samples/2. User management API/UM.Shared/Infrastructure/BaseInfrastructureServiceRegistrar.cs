using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.SubZero;

namespace SolidOps.UM.Shared.Infrastructure;

public abstract class BaseInfrastructureServiceRegistrar
{
    //protected virtual IDataAccessFactory InstantiateDataAccessFactory(IExtendedConfiguration configuration, string moduleName, string suffix, bool forceBase)
    //{
    //    var fullKey = moduleName + suffix;

    //    if (configuration.BurgrConfiguration.DataAccessFactories.ContainsKey(fullKey))
    //    {
    //        var entry = configuration.BurgrConfiguration.DataAccessFactories[fullKey];

    //        if (entry != null && !string.IsNullOrEmpty(entry.DataAccessFactory))
    //        {
    //            Type t = Type.GetType(entry.DataAccessFactory);
    //            IDataAccessFactory factory = (IDataAccessFactory)Activator.CreateInstance(t, configuration);
    //            factory.Name = fullKey;
    //            if (!string.IsNullOrEmpty(entry.Database))
    //            {
    //                factory.DatabaseInfo = configuration.BurgrConfiguration.Databases[entry.Database];
    //            }
    //            else
    //            {
    //                factory.DatabaseInfo = configuration.BurgrConfiguration.Databases.First().Value;
    //            }

    //            factory.DatabaseInfo.ConnectionString = factory.DatabaseInfo.ConnectionString.ReplaceLine();

    //            entry.Instance = factory;

    //            return factory;
    //        }
    //    }

    //    return null;
    //}
}
