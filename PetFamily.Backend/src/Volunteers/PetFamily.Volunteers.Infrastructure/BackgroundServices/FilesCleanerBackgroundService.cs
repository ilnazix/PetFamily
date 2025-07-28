using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Providers;
using PetFamily.Core.Messaging;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices
{
    public class FilesCleanerBackgroundService : BackgroundService
    {
        private readonly IMessageQueue<IEnumerable<FileMetadata>> _messageQueue;
        private readonly ILogger<FilesCleanerBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FilesCleanerBackgroundService(
            IMessageQueue<IEnumerable<FileMetadata>> messageQueue,
            ILogger<FilesCleanerBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _messageQueue = messageQueue;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var fileProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();

            _logger.LogInformation("FilesCleanerBackgroundService is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _messageQueue.ReadAsync(stoppingToken);

                foreach (var fileMetadata in message)
                {
                    await fileProvider.DeleteFile(fileMetadata, stoppingToken);
                }
            }

            await Task.CompletedTask;
        }
    }
}
