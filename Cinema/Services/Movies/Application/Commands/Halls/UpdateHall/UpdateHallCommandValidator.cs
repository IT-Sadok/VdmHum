namespace Application.Commands.Halls.UpdateHall;

using FluentValidation;

public sealed class UpdateHallCommandValidator : AbstractValidator<UpdateHallCommand>
{
    public UpdateHallCommandValidator()
    {
        this.RuleFor(c => c.Id)
            .NotEmpty();

        this.RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(c => c.NumberOfSeats)
            .NotEmpty()
            .GreaterThan(0);
    }
}