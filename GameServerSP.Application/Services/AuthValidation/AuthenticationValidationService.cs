using GameClientServerSP.Shared.Responses;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;

namespace GameServerSP.Application.Services.AuthValidation;

public class AuthenticationValidationService : IAuthenticationValidationService
{
    private readonly IWebSocketHandler _webSocketHander;

    public AuthenticationValidationService(IWebSocketHandler webSocketHander)
    {
        _webSocketHander = webSocketHander;
    }

    public async Task<bool> IsAuthenticatedAsync(Guid webSocketId)
    {
        var isAuth = _webSocketHander.ConnectionManager.IsSocketAuthenticated(webSocketId);
        if (!isAuth)
        {
            await _webSocketHander.SendMessageAsync(webSocketId, new BaseResponse { Errors = new List<string> { $"Please login. You are not authenticated" } });
        }

        return isAuth;
    }
}
