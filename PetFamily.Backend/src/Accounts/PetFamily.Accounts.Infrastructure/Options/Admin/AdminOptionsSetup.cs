using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetFamily.Accounts.Infrastructure.Options.Admin;

internal class AdminOptionsSetup : IConfigureOptions<AdminOptions>
{
    private const string SECTION_NAME = "ADMIN";

    private readonly IConfiguration _configuration;

    public AdminOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AdminOptions options)
    {
        _configuration
            .GetSection(SECTION_NAME)
            .Bind(options);
    }
}
