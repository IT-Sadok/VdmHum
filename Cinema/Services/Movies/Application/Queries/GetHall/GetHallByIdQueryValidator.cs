namespace Application.Queries.GetHall;

using FluentValidation;

public sealed class GetHallByIdQueryValidator : AbstractValidator<GetHallByIdQuery>
{
    public GetHallByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}