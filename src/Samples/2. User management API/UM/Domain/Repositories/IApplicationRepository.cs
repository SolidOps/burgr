namespace SolidOps.UM.Domain.Repositories;

public partial interface IApplicationRepository
{
    Task<IEnumerable<string>> GetConfigurations(string applicationName, string environmentName);
}
