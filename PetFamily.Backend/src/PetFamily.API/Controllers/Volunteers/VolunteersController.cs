using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers.Volunteers
{
    [Route("[controller]")]
    public class VolunteersController : ApplicationController
    {

        [HttpPost]
        public async Task<ActionResult<Envelope>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            [FromServices] IValidator<CreateVolunteerCommand> validator,
            CancellationToken cancellationToken)
        {
            var command = new CreateVolunteerCommand(
                FirstName: request.FirstName,
                LastName: request.LastName,
                MiddleName: request.MiddleName,
                PhoneNumber: request.PhoneNumber,
                Email: request.Email,
                SocialMedias: request.SocialMedias.Select(sm => new CreateSocialMediaCommand(sm.Link, sm.Title)),
                Requisites: request.Requisites.Select(r => new CreateRequisiteCommand(r.Title, r.Description))
            ); 

            var result = await handler.Handle(command, validator, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
