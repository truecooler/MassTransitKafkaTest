using Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class SettingsMessageConsumer : IConsumer<SettingsMessage>
    {
        readonly ILogger<SettingsMessageConsumer> _logger;
        public static SettingsMessage CurrentSettings = new("default", "default", 123); 

        public SettingsMessageConsumer()
        {
        }

        public SettingsMessageConsumer(ILogger<SettingsMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<SettingsMessage> context)
        {
            _logger.LogInformation($"Received settings: {context.Message.ToString()}");
            CurrentSettings = context.Message;
            //throw new Exception("test");
            return Task.CompletedTask;
        }
    }
}
