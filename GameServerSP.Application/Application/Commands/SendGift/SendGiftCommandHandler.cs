using GameServerSP.Application.Services.AuthValidation;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using GameServerSP.Domain.Repositories.Base;
using GameServerSP.Domain.Repositories;
using MediatR;
using GameClientServerSP.Shared.Requests;
using GameClientServerSP.Shared.Responses;

namespace GameServerSP.Application.Application.Commands.SendGift;

public class SendGiftCommandHandler : IRequestHandler<SendGiftCommand, Unit>
{
    private readonly IWebSocketHandler _webSocketHander;
    private readonly IValidationService<SendGiftCommand, Unit> _validationService;
    private readonly IAuthenticationValidationService _authenticationValidationService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SendGiftCommandHandler> _logger;

    public SendGiftCommandHandler(
        IWebSocketHandler webSocketHander,
        IValidationService<SendGiftCommand, Unit> validationService,
        IAuthenticationValidationService authenticationValidationService,
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork,
        ILogger<SendGiftCommandHandler> logger)
    {
        _webSocketHander = webSocketHander;
        _validationService = validationService;
        _authenticationValidationService = authenticationValidationService;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendGiftCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Send gift: send Gift to Player {command.FriendPlayerId}");

        if (await _validationService.ValidateAsync(command) && await _authenticationValidationService.IsAuthenticatedAsync(command.WebSocketId))
        {
            var friend = await _playerRepository.GetByIdAsync(command.FriendPlayerId);

            if (friend == null) 
            {
                var message = $"Send gift: cannot send a gift, friend {command.FriendPlayerId} was not found";
                _logger.LogWarning(message);
                await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { message } });
                return Unit.Value;
            }

            var connectionManger = _webSocketHander.ConnectionManager;

            var playerId = connectionManger.GetSocketById(command.WebSocketId).PlayerId;

            var player = await _playerRepository.GetByIdAsync(playerId);

            if(player.Id == friend.Id)
            {
                var message = $"Send gift: cannot send a gift to yourself PlayerId = {command.FriendPlayerId}";
                _logger.LogWarning(message);
                await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { message } });
                return Unit.Value;
            }

            var isValid = true;
            var errorMessage = string.Empty;
            switch (command.Type)
            {
                case ResourceType.Coins:
                    var newPlayerCoins = player.Coins - command.Value;
                    if (newPlayerCoins >= 0)
                    {
                        player.Coins = newPlayerCoins;
                        friend.Coins += command.Value;
                    }
                    else
                    {
                        errorMessage = $"Send gift: cannot send a gift, player {player.Id} does not have enought coins";
                        isValid = false;
                    }
                    break;
                case ResourceType.Rolls:
                    var newPlayerRolls = player.Rolls - command.Value;

                    if (newPlayerRolls >= 0)
                    {
                        player.Rolls = newPlayerRolls;
                        friend.Rolls += command.Value;
                    }
                    else
                    {
                        isValid = false;
                        errorMessage = $"Send gift: cannot send a gift, player {player.Id} does not have enought rolls";
                    }
                    break;
                default:
                    isValid = false;
                    errorMessage = $"Send gift: Gift Type is not specified";
                    break;
            }

            if (isValid)
            {
                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation($"SendGift: Gift was send from PlayerId {player.Id} to Player {command.FriendPlayerId}");
                await SendGift(playerId, command);
            }
            else
            {
                _logger.LogWarning(errorMessage);
                await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { errorMessage } });
            }
        }

        return Unit.Value;
    }

    private async Task SendGift(int playerId, SendGiftCommand command)
    {
        var isSent = false;
        if (_webSocketHander.ConnectionManager.TryGetWebSocketByPlayerId(command.FriendPlayerId, out var friendWebSocket))
        {
            if (friendWebSocket.IsAuthenticated)
            {
                isSent = true;
                await _webSocketHander.SendMessageAsync(friendWebSocket.WebSocket, new SendGiftEvent { PlayerId = playerId, Type = command.Type, Value = command.Value });
                _logger.LogInformation($"Send gift: event was sent from player {playerId} to player {command.FriendPlayerId}, Player is not login");

            }
        }
        if (!isSent)
        {
            _logger.LogWarning($"Send gift: event was not sent from player {playerId} to player {command.FriendPlayerId}, Player is not login");
        }
    }
}
