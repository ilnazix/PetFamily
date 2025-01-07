using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var status = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            var responeError = new ResponseError(error.Code, error.Message, null);
            var envelope = Envelope.Error([responeError]);
        
            return new ObjectResult(envelope)
            {
                StatusCode = status,
            };
        }

        public static ActionResult ToResponse(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                throw new InvalidOperationException("Result cannot be succeed");
            }

            var validationErrors = validationResult.Errors;
            var responseErrors = new List<ResponseError>();

            foreach (var validationError in validationErrors)
            {
                var error = Error.Deserialize(validationError.ErrorMessage);
                responseErrors.Add(new ResponseError(error.Code, error.Message, validationError.PropertyName));
            }

            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}
