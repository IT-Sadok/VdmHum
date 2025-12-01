namespace Application.Commands.Halls.UpdateHall;

using FluentValidation;

public sealed class UpdateHallCommandValidator : AbstractValidator<UpdateHallCommand>
{
    public UpdateHallCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.NumberOfSeats)
            .NotEmpty()
            .GreaterThan(0);
    }
}