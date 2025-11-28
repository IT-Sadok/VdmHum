namespace Application.Commands.Cinemas.CreateCinema;

using FluentValidation;

public sealed class CreateCinemaCommandValidator : AbstractValidator<CreateCinemaCommand>
{
    public CreateCinemaCommandValidator()
    {
        this.RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(c => c.City)
            .NotEmpty()
            .MaximumLength(200);

        this.RuleFor(c => c.Address)
            .NotEmpty()
            .MaximumLength(500);

        this.RuleFor(c => c)
            .Must(c =>
                (!c.Latitude.HasValue && !c.Longitude.HasValue) ||
                (c.Latitude.HasValue && c.Longitude.HasValue))
            .WithMessage("You must specify both latitude and longitude, or specify neither.");
    }
}