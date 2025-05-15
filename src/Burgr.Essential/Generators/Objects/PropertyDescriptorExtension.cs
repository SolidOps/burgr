using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Descriptors;
using SolidOps.Burgr.Essential.Generators.ConversionServices;

namespace SolidOps.Burgr.Essential.Generators.Objects;

public static class PropertyDescriptorExtension
{
    public static string GetPropertyType(this ModelDescriptor descriptor, IConversionService conversionService, string prefix, string suffix, bool preventList)
    {
        if (descriptor.Get("PropertyType") == "Simple")
        {
            return conversionService.SimplePropertyType(descriptor, preventList);
        }
        else if (descriptor.Get("PropertyType") == "Enum")
        {
            return conversionService.EnumPropertyType(descriptor, suffix, preventList);
        }
        else if (descriptor.Get("PropertyType") == "Model")
        {
            return conversionService.ModelPropertyType(descriptor, prefix, suffix, preventList);
        }
        else if (descriptor.Get("PropertyType") == "ReferencedModel")
        {
            return conversionService.ReferencedModelPropertyType(descriptor, prefix, suffix, preventList);
        }
        return "";
    }
}
