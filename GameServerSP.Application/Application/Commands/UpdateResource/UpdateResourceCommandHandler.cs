using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using MediatR;
using GameClientServerSP.Shared.Responses;
using GameServerSP.Application.Services.AuthValidation;
using GameServerSP.Domain.Repositories;
using GameServerSP.Domain.Repositories.Base;

namespace GameServerSP.Application.Application.Commands.UpdateResource;

public class UpdateResourceCommandHandler : IRequestHandler<UpdateResourceCommand, Unit>
{
    private readonly IWebSocketHandler _webSocketHander;
    private readonly IValidationService<UpdateResourceCommand, Unit> _validationService;
    private readonly IAuthenticationValidationService _authenticationValidationService;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateResourceCommandHandler> _logger;
    public UpdateResourceCommandHandler(
        IWebSocketHandler webSocketHander,
        IValidationService<UpdateResourceCommand, Unit> validationService,
        IAuthenticationValidationService authenticationValidationService,
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateResourceCommandHandler> logger)
    {
        _webSocketHander = webSocketHander;
        _validationService = validationService;
        _authenticationValidationService = authenticationValidationService;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateResourceCommand command, CancellationToken cancellationToken)
    {
        var playerId = _webSocketHander.ConnectionManager.GetSocketById(command.WebSocketId).PlayerId;

        _logger.LogInformation($"UpdateResource: Update resource for player {playerId}");

        if(await _validationService.ValidateAsync(command) && await _authenticationValidationService.IsAuthenticatedAsync(command.WebSocketId))
        {
            await UpdateResources(command, playerId);
        }

        return Unit.Value;
    }

    public async Task UpdateResources(UpdateResourceCommand command, int playerId)
    {
        var player = await _playerRepository.GetByIdAsync(playerId);

        if (player != null)
        {
            player.Coins = command.Coins;
            player.Rolls = command.Rolls;

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            await _webSocketHander.SendMessageAsync(command.WebSocketId, new UpdateResourceResponse { PlayerId = playerId, Coins = command.Coins, Rolls = command.Rolls });
            _logger.LogInformation($"UpdateResource: Updated resource for player {playerId}");

        }
        else
        {
            await _webSocketHander.SendMessageAsync(command.WebSocketId, new BaseResponse { Errors = new List<string> { $"Player with {playerId} was not found" } });
            _logger.LogInformation($"UpdateResource: Player {playerId} was not found");
        }
    }
}
