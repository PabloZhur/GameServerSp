using FluentValidation;

namespace GameServerSP.Application.Application.Commands.LoginPlayer;

public class LoginPlayerCommandValidator : AbstractValidator<LoginPlayerCommand>
{
    public LoginPlayerCommandValidator()
    {
        RuleFor(c => c.DeviceId)
            .NotEmpty().WithErrorCode("DeviceId must be provided");
    }
}
