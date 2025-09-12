using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Commands;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    RefreshSession GenerateRefreshToken(User user, LoginMetadata metadata);
}
