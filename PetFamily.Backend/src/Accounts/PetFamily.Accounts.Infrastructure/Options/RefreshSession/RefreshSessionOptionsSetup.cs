using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetFamily.Accounts.Infrastructure.Options.RefreshSession;

internal class RefreshSessionOptionsSetup : IConfigureOptions<RefreshSessionOptions>
{
    private const string SECTION_NAME = "RefreshSession";
    private readonly IConfiguration _configuration;

    public RefreshSessionOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RefreshSessionOptions options)
    {
        _configuration
            .GetSection(SECTION_NAME)
            .Bind(options);
    }
}
