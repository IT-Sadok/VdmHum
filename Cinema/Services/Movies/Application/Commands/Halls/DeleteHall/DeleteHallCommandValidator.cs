namespace Application.Commands.Halls.DeleteHall;

using FluentValidation;

public sealed class DeleteHallCommandValidator : AbstractValidator<DeleteHallCommand>
{
    public DeleteHallCommandValidator()
    {
        this.RuleFor(c => c.Id)
            .NotEmpty();
    }
}