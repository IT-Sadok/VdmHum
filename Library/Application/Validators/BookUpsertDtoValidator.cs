using Domain.Enums;
using Domain.ValueObjects;
using FluentValidation;
using Shared.Contracts;

namespace Application.Validators;

public class BookUpsertDtoValidator : AbstractValidator<BookUpsertDto>
{
    public BookUpsertDtoValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty()
            .MaximumLength(BookTitle.MaxTitleLength);

        RuleFor(b => b.Year)
            .Must(year => year >= 1)
            .WithMessage("Year must not be below 1.");

        RuleFor(b => b.Authors)
            .NotEmpty()
            .WithMessage("A book must have at least one author.");

        RuleForEach(b => b.Authors)
            .ChildRules(a =>
            {
                a.RuleFor(x => x.Type)
                    .IsInEnum();

                a.RuleFor(x => x.Name)
                    .NotEmpty()
                    .When(x => x.Type is AuthorType.Known or AuthorType.Pseudonym)
                    .WithMessage("Author name required for Known or Pseudonym.");

                a.RuleFor(x => x.Name)
                    .Empty()
                    .When(x => x.Type is AuthorType.Anonymous or AuthorType.Folk or AuthorType.Unknown)
                    .WithMessage("Author name must be empty for this author type.");
            });
    }
}