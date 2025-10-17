namespace PetFamily.Framework.Auth;

public interface IUserContext
{
    UserScopedData Current { get; }
}