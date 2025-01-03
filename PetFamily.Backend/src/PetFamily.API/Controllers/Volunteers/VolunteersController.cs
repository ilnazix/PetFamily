using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers.Volunteers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
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

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
