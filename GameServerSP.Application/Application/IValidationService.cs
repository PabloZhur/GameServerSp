using MediatR;

namespace GameServerSP.Application.Application;

public interface IValidationService<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<bool> ValidateAsync(TRequest request);
}

