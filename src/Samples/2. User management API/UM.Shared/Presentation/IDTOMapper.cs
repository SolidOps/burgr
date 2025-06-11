namespace SolidOps.UM.Shared.Presentation;

public interface IDTOMapper
{
    object ConvertExtension(object entity);
}

public interface IDTOMapper<TDTO, TEntity> : IDTOMapper
{
    void Initialize(TDTO dto);
    object ConvertExtension(TEntity entity);
}
