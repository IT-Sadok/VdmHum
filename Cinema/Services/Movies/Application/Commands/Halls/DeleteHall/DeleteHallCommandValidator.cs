namespace Application.Commands.Halls.DeleteHall;

using FluentValidation;

public sealed class DeleteHallCommandValidator : AbstractValidator<DeleteHallCommand>
{
    public DeleteHallCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}