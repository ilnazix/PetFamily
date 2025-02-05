using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.AddPet;
using PetFamily.API.Controllers.Volunteers.ChangePetPosition;
using PetFamily.API.Controllers.Volunteers.UpdateMainInfo;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhoto;
using PetFamily.Application.Volunteers.ChangePetPosition;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.HardDelete;
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
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand();
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult<Envelope>> UpdateMainInfo(
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoRequest request,
            [FromServices] UpdateMainInfoHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);
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
            CancellationToken cancellationToken)
        {
            var command = new UpdateSocialMediasCommand(id, dto);
            
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
            CancellationToken cancellationToken)
        {
            var command = new UpdateRequisitesCommand(id, dto);

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
            CancellationToken cancellationToken)
        {
            var command = new SoftDeleteCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}/hard")]
        public async Task<ActionResult<Envelope>> HardDelete(
            [FromRoute] Guid id,
            [FromServices] HardDeleteCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new HardDeleteCommand(id);
           
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
            CancellationToken cancellationToken)
        {
            var command = new RestoreVolunteerCommand(id);
            
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pets")]
        public async Task<ActionResult<Envelope>> AddPet(
            [FromRoute] Guid id,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure) {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
        public async Task<ActionResult> AddPetPhoto(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] AddPetPhotoCommandHandler handler,
            CancellationToken cancellationToken)
        {
            await using var formFileProcessor = new FormFileProcessor();
            var fileCommands = formFileProcessor.Process(files);

            var addPetPhotoCommand = new AddPetPhotoCommand(volunteerId, petId, fileCommands);

            var result = await handler.Handle(addPetPhotoCommand, cancellationToken);

            if (result.IsFailure)  
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{volunteerId:guid}/pets/{petId:guid}")]
        public async Task<ActionResult> ChangepetPosition(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] ChangePetPositionRequest request,
            [FromServices] ChangePetPositionCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(volunteerId, petId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
