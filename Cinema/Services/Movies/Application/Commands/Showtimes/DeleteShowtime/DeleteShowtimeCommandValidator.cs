namespace Application.Commands.Showtimes.DeleteShowtime;

using FluentValidation;

public sealed class DeleteShowtimeCommandValidator : AbstractValidator<DeleteShowtimeCommand>
{
    public DeleteShowtimeCommandValidator()
    {
        this.RuleFor(c => c.Id)
            .NotEmpty();
    }
}