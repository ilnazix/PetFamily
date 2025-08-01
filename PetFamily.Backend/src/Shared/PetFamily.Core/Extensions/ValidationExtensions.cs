using FluentValidation.Results;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static ErrorList ToErrorList(this ValidationResult validationResult)
        {
            if (validationResult.IsValid) throw new InvalidOperationException("Cannot convert valid validation result to Error List");

            var validationErrors = validationResult.Errors;
            var errors = validationErrors.Select(ve => Error.Deserialize(ve.ErrorMessage));
            return new ErrorList(errors);
        }
    }
}
