namespace GameServerSP.Application.Services.AuthValidation;

public interface IAuthenticationValidationService
{
    Task<bool> IsAuthenticatedAsync(Guid webSocketId);
}