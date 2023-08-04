using FluentValidation;
using GameClientServerSP.Shared.Responses;
using GameServerSP.Application.Models.Requests;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using MediatR;

namespace GameServerSP.Application.Application;

public class ValidationService<TRequest, TResponse> : IValidationService<TRequest, TResponse>
        where TRequest : WebSocketRequest, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IWebSocketHandler _webSocketHandler;

    public ValidationService(IEnumerable<IValidator<TRequest>> validators, IWebSocketHandler webSocketHandler)
    {
        _validators = validators;
        _webSocketHandler = webSocketHandler;
    }

    public async Task<bool> ValidateAsync(TRequest request)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(f => f.ErrorMessage)
                .ToList();

            if (failures.Any())
            {
                await _webSocketHandler.SendMessageAsync(request.WebSocketId, new BaseResponse { Errors = failures });
                return false;
            }
        }

        return true;
    }
}
