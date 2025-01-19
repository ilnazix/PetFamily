using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Restore;
using PetFamily.Application.Volunteers.Shared;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialMedias;

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
                SocialMedias: request.SocialMedias.Select(sm => new SocialMediaDto(sm.Link, sm.Title)),
                Requisites: request.Requisites.Select(r => new RequisitesDto(r.Title, r.Description))
            );

            var result = await handler.Handle(command, validator, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult<Envelope>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoDto dto,
            [FromServices] UpdateMainInfoHandler handler,
            [FromServices] IValidator<UpdateMainInfoCommand> validator,
            CancellationToken cancellationToken)
        {
            var command = new UpdateMainInfoCommand(id, dto);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToResponse();
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/social-medias")]
        public async Task<ActionResult<Envelope>> UpdateSocialMediasList(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialMediaDto dto,
            [FromServices] UpdateSocialMediasCommandHandler handler,
            [FromServices] IValidator<UpdateSocialMediasCommand> validator,
            CancellationToken cancellationToken)
        {
            var command = new UpdateSocialMediasCommand(id, dto);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToResponse();
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/requisites")]
        public async Task<ActionResult<Envelope>> UpdateRequisitesList(
            [FromRoute] Guid id,
            [FromBody] UpdateRequisitesDto dto,
            [FromServices] UpdateRequisitesCommandHandler handler,
            [FromServices] IValidator<UpdateRequisitesCommand> validator,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRequisitesCommand(id, dto);
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToResponse();
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Envelope>> SoftDelete(
            [FromRoute] Guid id,
            [FromServices] SoftDeleteCommandHandler handler,
            [FromServices] IValidator<SoftDeleteCommand> validator,
            CancellationToken cancellationToken)
        {
            var command = new SoftDeleteCommand(id);
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                return validationResult.ToResponse();
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<Envelope>> Restore(
            [FromRoute] Guid id,
            [FromServices] RestoreVolunteerCommandHandler handler,
            [FromServices] IValidator<RestoreVolunteerCommand> validator, 
            CancellationToken cancellationToken)
        {
            var command = new RestoreVolunteerCommand(id);
            var validationResult = await validator.ValidateAsync(command);
            
            if(validationResult.IsValid == false)
            {
                return validationResult.ToResponse();
            }

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }
    }
}
