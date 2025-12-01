namespace Application.Queries.GetMovie;

using FluentValidation;

public sealed class GetMovieByIdQueryValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}