using Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class MessageConsumer : IConsumer<Message>
    {
        readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer()
        {
        }

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"Received Text: {context.Message.Value}, id: {context.MessageId}");
            //throw new Exception("test");
            return Task.CompletedTask;
        }
    }
}
