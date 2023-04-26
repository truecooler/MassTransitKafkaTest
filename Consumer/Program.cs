using Common;
using Confluent.Kafka;
using Consumer;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, config) => config.ConfigureEndpoints(context));
            x.AddRider(rider =>
            {
                rider.AddConsumer<SettingsMessageConsumer>();
                rider.UsingKafka((context, k) =>
                {
                    k.Host("localhost:29092");
                    k.TopicEndpoint<SettingsMessage>("settings-topic", "test-consumer-group", e =>
                    {
                        e.CreateIfMissing(t =>
                        {
                            t.NumPartitions = 1;
                        });
                        e.AutoOffsetReset = AutoOffsetReset.Earliest;
                        e.ConfigureConsumer<SettingsMessageConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
                        e.EnableAutoOffsetStore = false;
                        e.CheckpointInterval = TimeSpan.MaxValue;
                        e.MessageLimit = ushort.MaxValue;
                    });
                });
            });
        });
    })
    .Build();

host.Run();
