using GameClientServerSP.Shared;
using GameServerSP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameServerSP.Infrastructure.EntityConfiguration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            var players = PlayerConfiguration.GetPlayers();
            var guids = PredifiendGuids.GetGuids();
            builder.HasData(
                new Device
                {
                    Id = guids[0],
                    PlayerId = players[0].Id
                },
                new Device
                {
                    Id = guids[1],
                    PlayerId = players[1].Id
                },
                new Device
                {
                    Id = guids[2],
                    PlayerId = players[2].Id
                },
                new Device
                {
                    Id = guids[3],
                    PlayerId = players[3].Id
                }
            );

        }
    }
}
