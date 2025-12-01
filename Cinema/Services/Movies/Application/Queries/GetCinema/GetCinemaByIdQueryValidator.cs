namespace Application.Queries.GetCinema;

using FluentValidation;

public sealed class GetCinemaByIdQueryValidator : AbstractValidator<GetCinemaByIdQuery>
{
    public GetCinemaByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}