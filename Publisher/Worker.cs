using Common;
using Confluent.Kafka;
using MassTransit;

namespace Publisher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var producer = scope.ServiceProvider.GetService<ITopicProducer<Null, SettingsMessage>>();
            int i = 666;
            while (!stoppingToken.IsCancellationRequested)
            {
                await producer.Produce(null, new("host " + i, "host " + i+1, i + 2));
                _logger.LogInformation(i + " published");

                await Task.Delay(4000, stoppingToken);
                i++;
            }
        }
    }
}