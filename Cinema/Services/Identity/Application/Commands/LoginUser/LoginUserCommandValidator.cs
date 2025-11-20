namespace Application.Commands.LoginUser;

using FluentValidation;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        this.RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}