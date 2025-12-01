namespace Application.Commands.Movies.DeleteMovie;

using FluentValidation;

public sealed class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}