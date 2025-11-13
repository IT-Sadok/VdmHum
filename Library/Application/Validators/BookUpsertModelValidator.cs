using Domain.Enums;
using FluentValidation;
using Shared.Contracts;

namespace Application.Validators;

public sealed class BookUpsertModelValidator : AbstractValidator<BookUpsertModel>
{
    public BookUpsertModelValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage("Title must not exceed 255 characters.");

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
                    .MaximumLength(100)
                    .When(x => x.Type is AuthorType.Known or AuthorType.Pseudonym)
                    .WithMessage("Name must not exceed 100 characters.");

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