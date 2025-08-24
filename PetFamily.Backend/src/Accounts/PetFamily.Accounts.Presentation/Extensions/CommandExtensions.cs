using PetFamily.Accounts.Application.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;

namespace PetFamily.Accounts.Presentation.Extensions;
public static class CommandExtensions 
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest r) 
        => new(r.Email, r.UserName, r.Password);
}
