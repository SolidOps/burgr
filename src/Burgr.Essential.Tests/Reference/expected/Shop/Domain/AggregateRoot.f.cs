namespace Acme.Shop.Domain;

// framework file of the Shop module: same content for every model
public abstract class AggregateRoot
{
    public Guid Id { get; set; }
}
