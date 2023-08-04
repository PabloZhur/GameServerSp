using GameClientServerSP.Shared.Responses;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using GameServerSP.Domain.Repositories;
using MediatR;

namespace GameServerSP.Application.Application.Commands.LoginPlayer;

public class LoginPlayerCommandHandler : IRequestHandler<LoginPlayerCommand, Unit>
{
    private readonly IWebSocketHandler _webSocketHander;
    private readonly IValidationService<LoginPlayerCommand, Unit> _validationService;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ILogger<LoginPlayerCommandHandler> _logger;

    public LoginPlayerCommandHandler(
        IWebSocketHandler webSocketHander,
        IValidationService<LoginPlayerCommand, Unit> validationService,
        IDeviceRepository deviceRepository,
        ILogger<LoginPlayerCommandHandler> logger)
    {
        _webSocketHander = webSocketHander;
        _validationService = validationService;
        _deviceRepository = deviceRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(LoginPlayerCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Try login Player with deviceId={command.DeviceId}");

        if (await _validationService.ValidateAsync(command))
        {
            var device = await _deviceRepository.GetByIdAsync(command.DeviceId);

            if (device != null)
            {
                if (_webSocketHander.ConnectionManager.IsSocketAuthenticated(command.WebSocketId))
                {
                    var message = $"Login: You've already login";

                    _logger.LogWarning(message);
                    await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { message } });
                }
                else if (_webSocketHander.ConnectionManager.IsPlayerAuthenticated(device.PlayerId))
                {
                    var message = $"Login: You've already login from another device {device.Id}";

                    await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { message } });
                }
                else
                {
                    _webSocketHander.ConnectionManager.AuthenticateSocket(command.WebSocketId, device.PlayerId);
                    _logger.LogInformation($"The Player with PlayerId={device.PlayerId} logged in with websocket connectionId = {command.WebSocketId}");

                    var loginResponse = new LoginResponse
                    {
                        PlayerId = device.PlayerId
                    };
                    await _webSocketHander.SendMessageAsync(command.WebSocketId, loginResponse);
                }
            }
            else
            {
                var message = $"DeviceId {command.DeviceId} was not found";

                _logger.LogWarning(message);
                await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { message } });
            }
        }

        return Unit.Value;
    }
}
