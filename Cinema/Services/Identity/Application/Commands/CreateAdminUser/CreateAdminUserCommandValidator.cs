namespace Application.Commands.CreateAdminUser;

using FluentValidation;

public class CreateAdminUserCommandValidator : AbstractValidator<CreateAdminUserCommand>
{
    public CreateAdminUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        this.RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);

        this.RuleFor(u => u.PhoneNumber)
            .MaximumLength(15);

        this.RuleFor(u => u.FirstName)
            .MaximumLength(100);

        this.RuleFor(u => u.LastName)
            .MaximumLength(100);
    }
}