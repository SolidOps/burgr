namespace SolidOps.TODO.Contracts.DTO;

public partial class ItemDTO
{
    public override string ToString()
    {
        return $"{Id}: {Name} ({Status.ToString()}), remaining: {RemainingDays} days";
    }
}
