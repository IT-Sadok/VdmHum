namespace Application.Queries.GetShowtime;

using FluentValidation;

public sealed class GetShowtimeByIdQueryValidator : AbstractValidator<GetShowtimeByIdQuery>
{
    public GetShowtimeByIdQueryValidator()
    {
        this.RuleFor(q => q.Id)
            .NotEmpty();
    }
}