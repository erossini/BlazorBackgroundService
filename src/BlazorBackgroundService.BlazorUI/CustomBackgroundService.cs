using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorBackgroundService.BlazorUI
{
    public class CustomBackgroundService : BackgroundService
    {
        public bool IsRunning { get; set; }

        private readonly ILogger<CustomBackgroundService> _logger;

        public CustomBackgroundService(ILogger<CustomBackgroundService> logger) =>
            _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(CustomBackgroundService)} starting {nameof(ExecuteAsync)}");
                IsRunning = true;
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"{nameof(CustomBackgroundService)} running {nameof(ExecuteAsync)}");
                    await Task.Delay(5000);
                }
                IsRunning = false;
                _logger.LogInformation($"{nameof(CustomBackgroundService)} ending {nameof(ExecuteAsync)}");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}
