using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework.Response;
using PetFamily.SharedKernel;

namespace PetFamily.Framework
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var status = GetStatusCodeForErrorType(error.Type);

            var responeError = new ResponseError(error.Code, error.Message, null);
            var envelope = Envelope.Error(error.ToErrorList());

            return new ObjectResult(envelope)
            {
                StatusCode = status,
            };
        }

        public static ActionResult ToResponse(this ErrorList errors)
        {
            if (!errors.Any())
            {
                return new ObjectResult(null)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            var distinctErrors = errors.Select(x => x.Type).Distinct().ToList();

            var statusCode = distinctErrors.Count() > 1
                ? StatusCodes.Status500InternalServerError
                : GetStatusCodeForErrorType(distinctErrors.First());

            var envelope = Envelope.Error(errors);

            return new ObjectResult(envelope) { StatusCode = statusCode };
        }

        private static int GetStatusCodeForErrorType(ErrorType errorType)
        {
            var statusCode = errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            return statusCode;
        }
    }
}
