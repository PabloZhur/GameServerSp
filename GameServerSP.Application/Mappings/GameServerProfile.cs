using AutoMapper;
using GameClientServerSP.Shared.Requests;
using GameServerSP.Application.Application.Commands.LoginPlayer;
using GameServerSP.Application.Application.Commands.SendGift;
using GameServerSP.Application.Application.Commands.UpdateResource;
using GameServerSP.Application.Models.Requests;

namespace GameServerSP.Application.Mappings;

public class GameServerProfile : Profile
{

    public GameServerProfile()
    {
        CreateMap<BaseRequest, WebSocketRequest>()
            .Include<LoginRequest, LoginPlayerCommand>()
            .Include<UpdateResourceRequest, UpdateResourceCommand>()
            .Include<GiftRequest, SendGiftCommand>();

        CreateMap<LoginRequest, LoginPlayerCommand>();
        CreateMap<UpdateResourceRequest, UpdateResourceCommand>();
        CreateMap<GiftRequest, SendGiftCommand>();
    }
}
