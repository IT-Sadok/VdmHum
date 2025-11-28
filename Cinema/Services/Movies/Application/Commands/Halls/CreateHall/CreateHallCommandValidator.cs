namespace Application.Commands.Halls.CreateHall;

using FluentValidation;

public sealed class CreateHallCommandValidator : AbstractValidator<CreateHallCommand>
{
    public CreateHallCommandValidator()
    {
        this.RuleFor(c => c.CinemaId)
            .NotEmpty();

        this.RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(c => c.NumberOfSeats)
            .GreaterThan(0);
    }
}