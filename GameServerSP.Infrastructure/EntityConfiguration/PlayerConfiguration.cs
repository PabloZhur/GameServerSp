using GameServerSP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameServerSP.Infrastructure.EntityConfiguration
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Coins).IsRequired();
            builder.Property(p => p.Rolls).IsRequired();

            builder.HasMany(p => p.Devices)
                .WithOne(d => d.Player)
                .HasForeignKey(d => d.PlayerId)
                .IsRequired();



            builder.HasData(GetPlayers());
        }

        public static List<Player> GetPlayers()
        {
            var players = new List<Player>
            {
                new Player
                {
                    Id = 1,
                    Name = "Pavel",
                    Coins = 10,
                    Rolls = 10
                },
                new Player
                {
                    Id = 2,
                    Name = "Ivan",
                    Coins = 15,
                    Rolls = 15
                },
                new Player
                {
                    Id = 3,
                    Name = "Maria",
                    Coins = 20,
                    Rolls = 20
                },
                new Player
                {
                    Id = 4,
                    Name = "Nadina",
                    Coins = 25,
                    Rolls = 25
                }
            };

            return players;
        }
    }
}
