using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Consumer
{
    internal class RequestResponseConsumer : IConsumer<Common.MyRequest>
    {
        readonly ILogger<RequestResponseConsumer> _logger;

        public RequestResponseConsumer()
        {
        }

        public RequestResponseConsumer(ILogger<RequestResponseConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MyRequest> context)
        {
            _logger.LogInformation("Consume message with data: " + context.Message.Data);
            await context.RespondAsync<MyResponse>(new() { Result = "Data length is: " + context.Message.Data.Length });
        }
    }
}
