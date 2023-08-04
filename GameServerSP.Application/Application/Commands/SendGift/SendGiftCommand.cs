using GameClientServerSP.Shared.Requests;
using GameServerSP.Application.Models.Requests;
using MediatR;

namespace GameServerSP.Application.Application.Commands.SendGift;

public class SendGiftCommand : WebSocketRequest, IRequest<Unit>
{
    public int FriendPlayerId { get; set; }
    public ResourceType Type{ get; set; }
    public int Value { get; set; }
}
