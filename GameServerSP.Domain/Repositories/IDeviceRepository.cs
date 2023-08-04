using GameServerSP.Domain.Entities;
using GameServerSP.Domain.Repositories.Base;

namespace GameServerSP.Domain.Repositories;

public interface IDeviceRepository : IRepository<Device, Guid>
{
}
