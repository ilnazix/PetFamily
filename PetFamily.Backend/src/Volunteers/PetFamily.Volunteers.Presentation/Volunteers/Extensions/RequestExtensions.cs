using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Volunteers.Commands.AddPet;
using PetFamily.Volunteers.Application.Volunteers.Commands.ChangePetPosition;
using PetFamily.Volunteers.Application.Volunteers.Commands.Create;
using PetFamily.Volunteers.Application.Volunteers.Commands.SetPetMainPhoto;
using PetFamily.Volunteers.Application.Volunteers.Commands.Shared;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetInfo;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdatePetStatus;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Volunteers.Application.Volunteers.Commands.UpdateSocialMedias;
using PetFamily.Volunteers.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Volunteers.Contracts.Requests;

namespace PetFamily.Volunteers.Presentation.Volunteers.Extensions
{
    public static class RequestExtensions
    {
        public static AddPetCommand ToCommand(this AddPetRequest request, Guid volunteerId)
        {
            return new AddPetCommand(
                volunteerId,
                request.PetName,
                request.Description,
                request.PhoneNumber,
                request.PetStatus,
                request.SpeciesId,
                request.BreedId
            );
        }

        public static ChangePetPositionCommand ToCommand(this ChangePetPositionRequest request, Guid volunteerId, Guid petId)
        {
            return new ChangePetPositionCommand(volunteerId, petId, request.NewPosition);
        }

        public static CreateVolunteerCommand ToCommand(this CreateVolunteerRequest request)
        {
            var fullNameDto = new FullNameDto(request.FirstName, request.LastName, request.MiddleName);

            return new CreateVolunteerCommand(
                FullName: fullNameDto,
                PhoneNumber: request.PhoneNumber,
                Email: request.Email,
                SocialMedias: request.SocialMedias.Select(sm => new SocialMediaInfo(sm.Link, sm.Title)),
                Requisites: request.Requisites.Select(r => new RequisiteInfo(r.Title, r.Description))
            );
        }

        public static GetFilteredVolunteersWithPaginationQuery ToQuery(this GetFilteredVolunteersWithPaginationRequest request)
        {
            return new GetFilteredVolunteersWithPaginationQuery(
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.Email,
                request.Page,
                request.PageSize
            );
        }

        public static SetPetMainPhotoCommand ToCommand(this SetPetMainPhotoRequest request, Guid volunteerId, Guid petId)
        {
            return new SetPetMainPhotoCommand(volunteerId, petId, request.ImagePath);
        }

        public static UpdateMainInfoCommand ToCommand(this UpdateMainInfoRequest request, Guid id)
        {
            var fullName = new FullNameDto(request.FirstName, request.LastName, request.MiddleName);
            return new UpdateMainInfoCommand(
                id,
                fullName,
                request.Experience,
                request.PhoneNumber,
                request.Email,
                request.Description
            );
        }

        public static UpdatePetInfoCommand ToCommand(this UpdatePetInfoRequest request, Guid volunteerId, Guid petId)
        {
            var requisites = request.Requisites.Select(r => new RequisiteInfo(r.Title, r.Description));

            return new UpdatePetInfoCommand(
                volunteerId,
                petId,
                request.PetName,
                request.SpeciesId,
                request.BreedId,
                request.Description,
                request.OwnerPhoneNumber,
                request.Color,
                request.DateOfBirth,
                requisites,
                request.IsCastrated,
                request.IsVaccinated,
                request.HealthInformation,
                request.Height,
                request.Weight,
                request.Country,
                request.State,
                request.City,
                request.Street,
                request.HouseNumber,
                request.ApartmentNumber
            );
        }

        public static UpdatePetStatusCommand ToCommand(this UpdatePetStatusRequest request, Guid volunteerId, Guid petId)
        {
            return new UpdatePetStatusCommand(volunteerId, petId, request.Status);
        }

        public static UpdateRequisitesCommand ToCommand(this UpdateRequisitesRequest request, Guid id)
        {
            var requisites = request.Requisites.Select(r => new RequisiteInfo(r.Title, r.Description));
            return new UpdateRequisitesCommand(id, requisites);
        }

        public static UpdateSocialMediasCommand ToCommand(this UpdateSocialMediasRequest request, Guid id)
        {
            var socialMedias = request.SocialMedias.Select(sm => new SocialMediaInfo(sm.Link, sm.Title));
            return new UpdateSocialMediasCommand(id, socialMedias);
        }
    }
}
