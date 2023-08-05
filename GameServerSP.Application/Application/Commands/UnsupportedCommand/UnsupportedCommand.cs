using GameServerSP.Application.Models.Requests;
using MediatR;

namespace GameServerSP.Application.Application.Commands.UnsupportedCommand;

public class UnsupportedCommand : WebSocketRequest, IRequest<Unit>
{
    public string Message { get; set; }
}
