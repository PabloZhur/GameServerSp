using GameClientServerSP.Shared.Responses;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using MediatR;

namespace GameServerSP.Application.Application.Commands.UnsupportedCommand;

public class UnsupportedCommandHandler : IRequestHandler<UnsupportedCommand, Unit>
{
    private readonly IWebSocketHandler _webSocketHandler;
    private readonly ILogger<UnsupportedCommand> _logger;

    public UnsupportedCommandHandler(IWebSocketHandler webSocketHandler, ILogger<UnsupportedCommand> logger)
    {
        _webSocketHandler = webSocketHandler;
        _logger = logger;
    }

    public async Task<Unit> Handle(UnsupportedCommand request, CancellationToken cancellationToken)
    {
        await _webSocketHandler.SendMessageAsync(request.WebSocketId, new BaseResponse { Errors = new List<string> { request.Message } });

        _logger.LogWarning($"Unsupported command was sent: {request.Message}");
        return Unit.Value;
    }
}
