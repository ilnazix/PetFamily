using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string UserName,
    string Password) : ICommand;
