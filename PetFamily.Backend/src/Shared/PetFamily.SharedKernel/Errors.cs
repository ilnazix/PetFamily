﻿using static PetFamily.SharedKernel.Errors;
using static System.Net.Mime.MediaTypeNames;

namespace PetFamily.SharedKernel
{
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

    }
}
