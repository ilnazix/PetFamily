using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Infrastructure.Options;
using PetFamily.Volunteers.Infrastructure.Services;


namespace PetFamily.Volunteers.Infrastructure.BackgroundServices
{
    public class DeleteExpiredVolunteersBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DeleteExpiredVolunteersBackgroundService> _logger;
        private readonly VolunteerEntityOptions _options;

        public DeleteExpiredVolunteersBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<DeleteExpiredVolunteersBackgroundService> logger,
            IOptions<VolunteerEntityOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DeleteExpiredVolunteersService is started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var service = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteersService>();

                _logger.LogInformation("DeleteExpiredVolunteersService is working");

                await service.Process(stoppingToken);

                await Task.Delay(TimeSpan.FromHours(_options.DeleteExpiredVolunteersServiceReductionDays),
                    stoppingToken);
            }
        }
    }
}
