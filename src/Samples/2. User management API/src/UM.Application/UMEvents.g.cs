#nullable enable
namespace SolidOps.UM.Application;
// Object [EVT]
public sealed partial record UserEvent(string Context, UserDto Data) : IEvent;
public sealed partial record InviteUsedEvent(string Context, InviteResultDto Data) : IEvent;