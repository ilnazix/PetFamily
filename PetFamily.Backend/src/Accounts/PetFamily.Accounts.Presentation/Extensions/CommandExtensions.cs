using PetFamily.Accounts.Application.Commands;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Application.Commands.RegisterUser;
using PetFamily.Accounts.Contracts.Requests;

namespace PetFamily.Accounts.Presentation.Extensions;
public static class CommandExtensions 
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest r) 
        => new(r.Email, r.UserName, r.Password);
    
    public static LoginUserCommand ToCommand(
        this LoginUserRequest r,
        LoginMetadata metadata) 
        => new(r.Email, r.Password, metadata);
}
