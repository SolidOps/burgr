//using Microsoft.EntityFrameworkCore;

//namespace SolidOps.UM.Infrastructure.Repositories;

//public partial class ApplicationRepository
//{
//    public async Task<IEnumerable<string>> GetConfigurations(string applicationName, string environmentName)
//    {
//        var q = this.GetQueryable();
        
//        var application = await q.Include(a => a.ApplicationEnvironments)
//            .ThenInclude(ae => ae.Environment)
//            .SingleOrDefaultAsync(a => a.Name == applicationName);

//        List<string> result = new List<string>();
//        if (application != null)
//        {
//            foreach (var ae in application.ApplicationEnvironments)
//            {
//                if (ae.Environment.Name == environmentName)
//                {
//                    result.Add(ae.Environment.ConfigurationContent);
//                    result.Add(application.ConfigurationContent);
//                    result.Add(ae.ConfigurationContent);
//                    break;
//                }
//            }
//        }
//        return result;
//    }
//}
