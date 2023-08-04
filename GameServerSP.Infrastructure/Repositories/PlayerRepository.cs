using GameServerSP.Domain.Entities;
using GameServerSP.Domain.Repositories;
using GameServerSP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerSP.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly GameServerContext _dbContext;

    public PlayerRepository(GameServerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Player> GetByIdAsync(int id)
    {
        var player = await _dbContext.Players.FindAsync(id);

        return player;
    }
}
