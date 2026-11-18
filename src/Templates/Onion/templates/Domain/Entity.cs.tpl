namespace _MODULE_NAMESPACE_.Domain;

/// <summary>
/// An object of the _MODULE_ module with an identity: two entities are the same when their
/// Id is. Entities of the model derive from it; they are persisted in their own table and
/// reached through the navigations of their aggregate.
/// </summary>
public abstract class Entity
{
    public _IDENTITY_KEY_TYPE_ Id { get; set; }
}

/// <summary>
/// The root of an aggregate: the only kind of entity loaded and saved through a repository
/// (<see cref="IRepository{T}"/>) and exposed by the API. Deleting it deletes the entities
/// referencing it; an aggregate root referenced by another one cannot be deleted.
/// </summary>
public abstract class AggregateRoot : Entity
{
}
