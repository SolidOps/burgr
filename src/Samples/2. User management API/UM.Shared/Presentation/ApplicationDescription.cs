using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace SolidOps.UM.Shared.Presentation;

public class ApplicationDescription : IApplicationModelConvention
{
    private readonly List<Assembly> dependencies;

    public ApplicationDescription(List<Assembly> dependencies)
    {
        this.dependencies = dependencies;
    }

    public void Apply(ApplicationModel application)
    {
        var ctr = application.Controllers.Where((model) =>
        {
            return !dependencies.Contains(model.ControllerType.Assembly);
        });
        if (ctr.Count() > 0)
        {
            foreach (var controller in ctr.ToList())
            {
                application.Controllers.Remove(controller);
            }
        }
    }
}
