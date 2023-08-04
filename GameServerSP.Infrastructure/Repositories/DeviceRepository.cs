using GameServerSP.Domain.Entities;
using GameServerSP.Domain.Repositories;
using GameServerSP.Infrastructure.Data;

namespace GameServerSP.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly GameServerContext _dbContext;

    public DeviceRepository(GameServerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Device> GetByIdAsync(Guid id)
    {
        var device = await _dbContext.Devices.FindAsync(id);
        if (device == null)
            return null;

        await _dbContext.Entry(device).Reference(a => a.Player).LoadAsync();

        return device;
    }
}
