namespace PetFamily.Domain.Shared
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

        public static class Breed
        {
            public static Error CannotDeleteWhenAnimalsExist()
            {
                return Error.Conflict(
                    "breed.delete.not.allowed",
                    "cannot delete breed because animals of this species exist"
                );
            }
        }
    }
}
