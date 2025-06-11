using SolidOps.UM.Shared.Domain.Configuration;
using SolidOps.UM.Shared.Domain.UnitOfWork;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.SubZero;

namespace SolidOps.UM.Shared.Core.CQRS;

public abstract class BaseCQRSServiceRegistrator : BaseInfrastructureServiceRegistrar
{
    //protected override IDataAccessFactory InstantiateDataAccessFactory(IExtendedConfiguration configuration, string moduleName, string suffix, bool forceBase)
    //{
    //    if (forceBase || configuration.BurgrConfiguration.DataAccessFactories.ContainsKey(moduleName + suffix))
    //    {
    //        return base.InstantiateDataAccessFactory(configuration, moduleName, suffix, false);
    //    }
    //    else
    //    {
    //        return InstantiateCQRSDataAccessFactories(configuration, moduleName);
    //    }
    //}

    //protected abstract IDataAccessFactory InstantiateCQRSDataAccessFactories(IExtendedConfiguration configuration, string moduleName);
}
