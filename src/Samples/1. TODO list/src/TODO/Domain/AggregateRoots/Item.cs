namespace SolidOps.TODO.Domain.AggregateRoots;

public partial class Item
{
    protected override int CalculateRemainingDays()
    {
        return Convert.ToInt32(Math.Ceiling((this.DueDate - DateTime.Now).TotalDays));
    }
}
