using GameServerSP.Domain.Entities.Base;

namespace GameServerSP.Domain.Entities;

public class Player : EntityBase<int>
{
    public string Name { get; set; }
    public int Coins { get; set; }
    public int Rolls { get; set; }
    public ICollection<Device> Devices { get; set; }
}
