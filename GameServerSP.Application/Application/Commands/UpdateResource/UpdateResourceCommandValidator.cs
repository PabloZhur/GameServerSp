using FluentValidation;

namespace GameServerSP.Application.Application.Commands.UpdateResource;

public class UpdateResourceCommandValidator : AbstractValidator<UpdateResourceCommand>
{
    public UpdateResourceCommandValidator()
    {
        RuleFor(c => c.Rolls)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Rolls must be non negative number");

        RuleFor(c => c.Coins)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Rolls must be non negative number");
    }
}
