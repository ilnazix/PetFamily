using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands;

public record LoginUserResponse(
    string AccessToken,
    string RefreshToken) : ICommand;
