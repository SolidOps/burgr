using SolidOps.SubZero;

namespace SolidOps.UM.Shared.Presentation;

public class JsonOutputSerializer : IOutputSerializer
{
    public string Serialize(object output)
    {
        return Serializer.Serialize(output, false, true, true);
    }
}
