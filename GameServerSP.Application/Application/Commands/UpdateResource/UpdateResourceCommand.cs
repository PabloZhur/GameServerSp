using GameServerSP.Application.Models.Requests;
using MediatR;

namespace GameServerSP.Application.Application.Commands.UpdateResource;

public class UpdateResourceCommand : WebSocketRequest, IRequest<Unit>
{
    public int Coins { get; set; }
    public int Rolls { get; set; }
}
