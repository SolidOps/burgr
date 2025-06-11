using System.Reflection;

namespace SolidOps.UM.Shared.Core.Events;

public class SignalREvent
{
    public SignalREvent()
    {

    }

    public string EventContext { get; set; }
    public string Content { get; set; }
    public string AssemblyName { get; set; }
    public string FullType { get; set; }
    private Type _eventType { get; set; }

    public Type GetEventType()
    {
        if (_eventType == null)
        {
            var assembly = Assembly.Load(AssemblyName);
            _eventType = assembly.GetType(FullType);
        }
        return _eventType;

    }
}
