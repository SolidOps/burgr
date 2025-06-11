using SolidOps.UM.Shared.Contracts.Events;

namespace MetaCorp.Template.Contracts.Events;

#region foreach MODEL[EVT]
public partial class CLASSNAMEEvent : SerializableEventOf<DTO.DEPENDENCYEVENTDATATYPEDTO>
{
    // For deserialization only
    public CLASSNAMEEvent() : base(string.Empty, string.Empty, false)
    {

    }

    public CLASSNAMEEvent(string eventContext, DTO.DEPENDENCYEVENTDATATYPEDTO data) : base(eventContext, data, false)
    {

    }

    public CLASSNAMEEvent(string eventContext, string content) : base(eventContext, content, false)
    {

    }
}
#endregion foreach MODEL