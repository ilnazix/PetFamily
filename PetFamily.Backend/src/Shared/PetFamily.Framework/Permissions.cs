namespace PetFamily.Framework;

public static class Permissions
{
    public static class Species
    {
        private const string EntityName = nameof(Species);

        public const string Create = $"{EntityName}.Create";
        public const string Update = $"{EntityName}.Update";
        public const string Delete = $"{EntityName}.Delete";
        public const string Read = $"{EntityName}.Read";
    }

    public static class Volunteers
    {
        private const string EntityName = nameof(Volunteers);

        public const string Create = $"{EntityName}.Create";
        public const string Update = $"{EntityName}.Update";
        public const string Delete = $"{EntityName}.Delete";
        public const string Read = $"{EntityName}.Read";
        public const string Restore = $"{EntityName}.Restore";
    }

    public static class Pets
    {
        private const string EntityName = nameof(Pets);

        public const string Create = $"{EntityName}.Create";
        public const string Update = $"{EntityName}.Update";
        public const string Delete = $"{EntityName}.Delete";
        public const string Read = $"{EntityName}.Read";
    }

    public static class Accounts 
    {
        private const string EntityName = nameof(Accounts);

        public const string Read = $"{EntityName}.Read";
    }

    public static class VolunteerRequests
    {
        private const string Base = "Volunteer.Request";

        public const string Create = $"{Base}.Create";
        public const string TakeOnReview = $"{Base}.TakeOnReview";
        public const string RequireRevision = $"{Base}.RequireRevision";
        public const string Approve = $"{Base}.Approve";
        public const string Submit = $"{Base}.Submit";
        public const string Update = $"{Base}.Update";
        public const string ReadUnassigned = $"{Base}.ReadUnassigned";
        public const string ReadAdmin = $"{Base}.ReadAdmin";
        public const string ReadOwn = $"{Base}.ReadOwn";
    }

    public static class Discussions 
    {
        private const string EntityName = nameof(Discussions);

        public const string AddMessage = $"{EntityName}.AddMessage";
    }
}

