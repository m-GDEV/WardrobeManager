using WardrobeManager.Shared.DTOs;
using FluentValidation;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.StaticResources;

// Generic validation method for all validators
public static class StaticValidators
{
    // A registry of validators (Map Type -> Validator Instance)
    private static readonly Dictionary<Type, IValidator> _validators = new()
    {
        { typeof(NewClothingItemDTO), new NewClothingItemDTOValidator() },
        { typeof(AuthenticationCredentialsModel), new AuthenticationCredentialsModelValidator()}
    };

    public static Result<T> Validate<T>(T input)
    {
        if (input is null) return new Result<T>(input, false, "Input cannot be null");

        // Look for a registered validator for this type
        if (_validators.TryGetValue(typeof(T), out var validator))
        {
            var context = new ValidationContext<T>(input);
            var validationResult = validator.Validate(context);

            if (!validationResult.IsValid)
            {
                // Join all error messages into one string
                string errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new Result<T>(input, false, errors);
            }
        }

        // If no validator is found, we assume it's valid by default
        return new Result<T>(input, true);
    }
}


// All static validators below (in one file to stay clean)
// Note: put a period at the end of each message so the frontend can split up the error into mulitple notifications
public class NewClothingItemDTOValidator : AbstractValidator<NewClothingItemDTO>
{
    public NewClothingItemDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("New clothing item must have a name.")
            .MaximumLength(50).WithMessage("Name is too long.");
    }
}
public class AuthenticationCredentialsModelValidator: AbstractValidator<AuthenticationCredentialsModel>
{
    public AuthenticationCredentialsModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not be empty.")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters.")
            .MaximumLength(50).WithMessage("Password is too long.")
            .Must(x => x.Any(char.IsDigit)).WithMessage("Password must have at least one digit.")
            .Must(x => x.Any(char.IsUpper)).WithMessage("Password must have at least one uppercase.");
    }
}
