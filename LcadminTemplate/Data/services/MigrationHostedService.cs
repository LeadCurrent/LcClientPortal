using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Data.DataContexts
{
    public class MigrationHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();

            try
            {
                await Task.Run(() => migrationService.MigrateAll(), stoppingToken);
                Console.WriteLine("Migration completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}");
            }
        }
    }
}
