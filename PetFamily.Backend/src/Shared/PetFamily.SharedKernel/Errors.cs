using static PetFamily.SharedKernel.Errors;
using static System.Net.Mime.MediaTypeNames;

namespace PetFamily.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid", name);
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for id={id}";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? " " : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length", name);
        }
    }

    public static class Species
    {
        public static Error CannotDeleteWhenAnimalsExist()
        {
            return Error.Conflict(
                "species.delete.not.allowed",
                "cannot delete species because animals of this species exist"
            );
        }
    }

    public static class Breeds
    {
        public static Error CannotDeleteWhenAnimalsExist()
        {
            return Error.Conflict(
                "breed.delete.not.allowed",
                "cannot delete breed because animals of this species exist"
            );
        }
    }

    public static class Pets
    {
        public static Error InvalidSpeciesOrBreed()
        {
            return Error.Validation(
                "pet.species-or-breed.invalid",
                "cannot create pet because specified species or breed does not exist",
                null
            );
        }
    }

    public static class User 
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation(
                "invalid.credentials", 
                "Your credentials are invalid", 
                null);
        }

        public static Error InvalidRole()
        {
            return Error.Failure(
                "invalid.role",
                "Invalid role");
        }

        public static Error TokenExpired()
        {
            return Error.Validation(
                "token.expired",
                "Token has expired",
                null);
        }
    }

    public static class Message 
    {
        public static Error MessageAuthorRequired()
        {
            return Error.Validation(
                code: "message.userId.required",
                message: "UserId is required to create a message.",
                "userId");
        }

        public static Error MessageTextRequired()
        {
            return Error.Validation(
            code: "message.newText.required",
            message: "Message newText cannot be empty.",
            "text");
        }

        public static Error EditNotAllowed()
        {
            return Error.Validation(
                code: "message.edit.notAllowed",
                message: "User is not allowed to edit this message.",
                null);
        }
    }

    public static class VolunteerRequest 
    {
        public static Error InvalidUser()
        {
            return Error.Validation(
                code: "volunteerRequest.invalidUser",
                message: "Only the user who created the request can submit it.",
                null
            );
        }
        public static Error InvalidAdmin()
        {
            return Error.Validation(
                code: "volunteerRequest.invalidAdmin",
                message: "Only the admin who took the request on review can perform this action.",
                null
            );
        }

        public static Error ActiveRequestExists()
        {
            return Error.Validation(
                code: "volunteerRequest.active.exists",
                message: "The user already has an active volunteer request.",
                null
            );
        }

        public static Error UserBannedAfterRejection(DateTime until)
        {
            return Error.Validation(
                code: "volunteerRequest.rejected.waitPeriod",
                message: $"You can create a new volunteer request after {until:dd.MM.yyyy HH:mm}.",
                null
            );
        }
    }

    public static class Discussion
    {
        public static Error RelationIdRequired()
        {
            return Error.Validation(
                code: "discussion.relationId.required",
                message: "RelationId is required to create a discussion.",
                "relationId"
            );
        }

        public static Error InvalidParticipantCount(int maxCount)
        {
            return Error.Validation(
                code: "discussion.participants.invalidCount",
                message: $"Discussion must contain exactly {maxCount} participants.",
                "ParticipantIds"
            );
        }

        public static Error MessageAuthorNotParticipant()
        {
            return Error.Validation(
                code: "discussion.message.author.notParticipant",
                message: "Only participants of the discussion can add messages.",
                null
            );
        }

        public static Error MessageDeleteNotAuthor()
        {
            return Error.Validation(
                code: "discussion.message.delete.notAuthor",
                message: "Only the author of the message can delete it.",
                null
            );
        }

        public static Error DiscussionClosed()
        {
            return Error.Validation(
                code: "discussion.closed",
                message: "Cannot perform this action because the discussion is already closed.",
                null
            );
        }
    }

}
