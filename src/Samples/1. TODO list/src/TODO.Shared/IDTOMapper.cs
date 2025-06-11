namespace SolidOps.TODO.Shared;

public interface IDTOMapper
{    
}

public interface IDTOMapper<TDTO, TEntity> : IDTOMapper
{
    void Initialize(TDTO dto);
}
