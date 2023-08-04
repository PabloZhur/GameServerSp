using GameServerSP.Domain.Entities.Base;

namespace GameServerSP.Domain.Entities;

public class Device : EntityBase<Guid>
{
    public int PlayerId { get; set; }
    public Player Player { get; set; }

}
