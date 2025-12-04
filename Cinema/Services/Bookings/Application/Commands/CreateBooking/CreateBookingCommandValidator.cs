namespace Application.Commands.CreateBooking;

using FluentValidation;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(c => c.ShowtimeId)
            .NotEmpty();

        RuleFor(c => c.Seats)
            .NotEmpty()
            .WithMessage("At least one seat must be selected.");

        RuleForEach(c => c.Seats)
            .GreaterThan(0);

        RuleFor(c => c.TotalPrice)
            .GreaterThan(0);

        RuleFor(c => c.Currency)
            .NotEmpty();
    }
}