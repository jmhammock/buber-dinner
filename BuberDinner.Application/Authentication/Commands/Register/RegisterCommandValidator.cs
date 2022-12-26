namespace BuberDinner.Application.Authentication.Commands.Register;

using FluentValidation;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(200).WithMessage("First name must not exceed 200 characters.");
        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(200).WithMessage("Last name must not exceed 200 characters.");
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");
        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
    }
}