namespace Application.Commands.CreateAdminUser;

using RegisterUser;
using FluentValidation;

public class CreateAdminUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public CreateAdminUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        this.RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}