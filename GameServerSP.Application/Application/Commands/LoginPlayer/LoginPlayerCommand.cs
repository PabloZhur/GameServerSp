using GameServerSP.Application.Models.Requests;
using MediatR;

namespace GameServerSP.Application.Application.Commands.LoginPlayer;

public class LoginPlayerCommand : WebSocketRequest, IRequest<Unit>
{
    public Guid DeviceId { get; set; }
}

