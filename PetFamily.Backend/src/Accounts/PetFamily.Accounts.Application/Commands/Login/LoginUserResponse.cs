using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.Login;

public record LoginUserResponse(
    string AccessToken,
    string RefreshToken): ICommand;
