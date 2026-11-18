namespace Acme.Shop.Billing.Domain;

// framework file of the Billing module: same content for every model
public abstract class AggregateRoot
{
    public Guid Id { get; set; }
}
