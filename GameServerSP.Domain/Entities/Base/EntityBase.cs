namespace GameServerSP.Domain.Entities.Base;

public class EntityBase<TId> : IEntityBase<TId>
{
    public TId Id { get; set; }
}
