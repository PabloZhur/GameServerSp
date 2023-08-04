using GameServerSP.Domain.Entities;
using GameServerSP.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace GameServerSP.Infrastructure.Data;

public class GameServerContext : DbContext
{
    private readonly string _connectionString;

    public GameServerContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    public GameServerContext(DbContextOptions options) : base(options) { }

    public DbSet<Device> Devices { get; set; }

    public DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DeviceConfiguration());
        builder.ApplyConfiguration(new PlayerConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
