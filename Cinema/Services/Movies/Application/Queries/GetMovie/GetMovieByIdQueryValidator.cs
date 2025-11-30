namespace Application.Queries.GetMovie;

using FluentValidation;

public sealed class GetMovieByIdQueryValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdQueryValidator()
    {
        this.RuleFor(q => q.Id)
            .NotEmpty();
    }
}