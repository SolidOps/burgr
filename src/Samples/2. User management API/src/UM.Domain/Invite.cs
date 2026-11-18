namespace SolidOps.UM.Domain;

public partial class Invite
{
    private partial bool CalculateIsOpen() => Status == InviteStatus.Sent;
}
