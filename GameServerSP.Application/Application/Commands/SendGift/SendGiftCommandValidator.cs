using FluentValidation;

namespace GameServerSP.Application.Application.Commands.SendGift;

public class SendGiftCommandValidator : AbstractValidator<SendGiftCommand>
{
    public SendGiftCommandValidator()
    {
        RuleFor(c => c.FriendPlayerId)
            .NotEmpty()
            .WithErrorCode("Friend is required to send a gift");

        RuleFor(c => c.Value)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Gift value must be non negative numer");
        RuleFor(c => c.Type)
            .NotNull()
            .WithErrorCode("Gift Type must be specified");
    }
}
