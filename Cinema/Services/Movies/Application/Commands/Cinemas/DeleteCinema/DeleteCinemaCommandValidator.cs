namespace Application.Commands.Cinemas.DeleteCinema;

using FluentValidation;

public sealed class DeleteCinemaCommandValidator : AbstractValidator<DeleteCinemaCommand>
{
    public DeleteCinemaCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}