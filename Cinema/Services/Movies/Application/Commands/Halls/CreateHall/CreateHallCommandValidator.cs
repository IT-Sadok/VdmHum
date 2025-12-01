namespace Application.Commands.Halls.CreateHall;

using FluentValidation;

public sealed class CreateHallCommandValidator : AbstractValidator<CreateHallCommand>
{
    public CreateHallCommandValidator()
    {
        RuleFor(c => c.CinemaId)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.NumberOfSeats)
            .GreaterThan(0);
    }
}