using Common;
using Confluent.Kafka;
using MassTransit;
using Publisher;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddHostedService<Worker>();
        services.AddHostedService<RpcWorker>();
        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, config) => config.ConfigureEndpoints(context));

            x.AddRider(rider =>
            {
                rider.AddProducer<Null, Message>("test-topic", (riderContext, producerConfig) =>
                {
                });
                rider.AddProducer<Null, MyRequest>("request-topic", (riderContext, producerConfig) =>
                {
                });
                rider.UsingKafka((context, k) =>
                {
                    k.Host("localhost:29092");
                });
            });
            x.AddRequestClient<MyRequest>(new Uri("topic:request-topic"));

        });
    })
    .Build();
var a = host.Services.GetRequiredService<IBusControl>();
a.Start();
host.Run();
