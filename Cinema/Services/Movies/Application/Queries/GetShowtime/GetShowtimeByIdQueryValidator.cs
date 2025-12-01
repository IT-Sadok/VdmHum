namespace Application.Queries.GetShowtime;

using FluentValidation;

public sealed class GetShowtimeByIdQueryValidator : AbstractValidator<GetShowtimeByIdQuery>
{
    public GetShowtimeByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}