namespace Application.Queries.GetCinema;

using FluentValidation;

public sealed class GetCinemaByIdQueryValidator : AbstractValidator<GetCinemaByIdQuery>
{
    public GetCinemaByIdQueryValidator()
    {
        this.RuleFor(q => q.Id)
            .NotEmpty();
    }
}