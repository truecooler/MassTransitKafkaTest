using Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Publisher
{
    internal class RpcWorker : BackgroundService
    {
        private readonly ILogger<RpcWorker> _logger;
        private readonly IClientFactory _clientFactory;

        public RpcWorker(ILogger<RpcWorker> logger, IClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            var client = _clientFactory.CreateRequestClient<MyRequest>(new Uri("exchange:request-topic"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(i + " sending request...");
                var response = await client.GetResponse<MyResponse>(new() { Data = i.ToString() }) ;
                _logger.LogInformation(i + " rpc result: " + response.Message.Result);

                await Task.Delay(4000, stoppingToken);
                i++;
            }
        }
    }
}
