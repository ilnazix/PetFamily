using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Framework.Auth;
using PetFamily.Framework.Processors;
using PetFamily.Framework.Response;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPetPhoto;
using PetFamily.Volunteers.Application.Volunteers.Commands.ChangePetPosition;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePermanently;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePet;
using PetFamily.Volunteers.Application.Volunteers.Commands.DeletePetPermanently;
using PetFamily.Volunteers.Application.Volunteers.Commands.Restore;
using PetFamily.Volunteers.Application.Volunteers.Commands.SetPetMainPhoto;
using PetFamily.Volunteers.Application.Volunteers.Commands.SoftDelete;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetStatus;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.Requests;
using PetFamily.Volunteers.Presentation.Volunteers.Extensions;


namespace PetFamily.Volunteers.Presentation.Volunteers
{
    [Route("[controller]")]
    public class VolunteersController : ApplicationController
    {
        [HttpGet]
        [HasPermission(Permissions.Volunteers.Read)]
        public async Task<ActionResult> GetVolunteerWithPagination(
            [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
            [FromServices] GetFilteredVolunteersWithPaginationQueryHandler handler,
            CancellationToken cancellationToken)
        {
            var query = request.ToQuery();

            var response = await handler.Handle(query, cancellationToken);

            return Ok(response);
        }

        [HttpPost]
        [HasPermission(Permissions.Volunteers.Create)]
        public async Task<ActionResult<Envelope>> Create(
            [FromServices] CreateVolunteerCommandHandler handler,
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
        [HasPermission(Permissions.Volunteers.Update)]
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

        [HttpDelete("{id:guid}")]
        [HasPermission(Permissions.Volunteers.Delete)]
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
        [HasPermission(Permissions.Volunteers.Delete)]
        public async Task<ActionResult<Envelope>> HardDelete(
            [FromRoute] Guid id,
            [FromServices] DeleteVolunteerPermanentlyCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeleteVolunteerPermanentlyCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.ToResponse();
            }

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}")]
        [HasPermission(Permissions.Volunteers.Restore)]
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
        [HasPermission(Permissions.Pets.Create)]
        public async Task<ActionResult<Envelope>> AddPet(
            [FromRoute] Guid id,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
        [HasPermission(Permissions.Pets.Update)]
        public async Task<ActionResult<Envelope>> UpdatePetInfo(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetInfoRequest request,
            [FromServices] UpdatePetInfoCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(volunteerId, petId);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}")]
        [HasPermission(Permissions.Pets.Delete)]
        public async Task<ActionResult<Envelope>> DeletePet(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] DeletePetCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetCommand(volunteerId, petId);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return NoContent();
        }

        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/main-photo")]
        [HasPermission(Permissions.Pets.Update)]
        public async Task<ActionResult> SetMainPhoto(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] SetPetMainPhotoRequest request,
            [FromServices] SetPetMainPhotoCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(volunteerId, petId);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }



        [HttpDelete("{volunteerId:guid}/pets/{petId:guid}/hard")]
        [HasPermission(Permissions.Pets.Delete)]
        public async Task<ActionResult<Envelope>> DeletePetPermanently(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromServices] DeletePetPermanentlyCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeletePetPermanentlyCommand(volunteerId, petId);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return NoContent();
        }

        [HttpPut("{volunteerId:guid}/pets/{petId:guid}/status")]
        [HasPermission(Permissions.Pets.Update)]
        public async Task<ActionResult<Envelope>> UpdatePetStatus(
            [FromRoute] Guid volunteerId,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetStatusRequest request,
            [FromServices] UpdatePetStatusCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(volunteerId, petId);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{volunteerId:guid}/pets/{petId:guid}/photos")]
        [HasPermission(Permissions.Pets.Update)]
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
        [HasPermission(Permissions.Pets.Update)]
        public async Task<ActionResult> ChangePetPosition(
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
