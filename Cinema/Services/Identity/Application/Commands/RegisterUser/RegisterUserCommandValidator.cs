namespace Application.Commands.RegisterUser;

using FluentValidation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        this.RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}