using Microsoft.EntityFrameworkCore.Design;

namespace GameServerSP.Infrastructure.Data;

public class GameServerContextFactory : IDesignTimeDbContextFactory<GameServerContext>
{
    public GameServerContext CreateDbContext(string[] args)
    {
        //Specify here the connection string
        return new GameServerContext("Data Source=LocalDatabase.db");
    }
}
