namespace Application.Commands.UpdateProfile;

using FluentValidation;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        this.RuleFor(u => u.PhoneNumber)
            .MaximumLength(15);

        this.RuleFor(u => u.FirstName)
            .MaximumLength(100);

        this.RuleFor(u => u.LastName)
            .MaximumLength(100);
    }
}