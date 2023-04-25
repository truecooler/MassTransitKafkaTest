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
                //rider.AddConsumer<MessageConsumer>();
                rider.AddConsumer<RequestResponseConsumer>();
                rider.UsingKafka((context, k) =>
                {
                    k.Host("localhost:29092");
                    //k.TopicEndpoint<Message>("test-topic", "test-consumer-group", e =>
                    //{
                    //    e.CreateIfMissing(t =>
                    //    {
                    //        t.NumPartitions = 1;
                    //    });
                    //    e.AutoOffsetReset = AutoOffsetReset.Error;
                    //    e.ConfigureConsumer<MessageConsumer>(context);
                    //    e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
                    //    e.EnableAutoOffsetStore = true;
                    //    e.CheckpointInterval = TimeSpan.FromSeconds(1);
                    //    e.MessageLimit = 1;
                    //});

                    k.TopicEndpoint<MyRequest>("request-topic", "test-consumer-group", e =>
                    {
                        e.CreateIfMissing(t =>
                        {
                            t.NumPartitions = 1;
                        });
                        e.ConfigureConsumer<RequestResponseConsumer>(context);
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
                        e.EnableAutoOffsetStore = true;
                        e.CheckpointInterval = TimeSpan.FromSeconds(1);
                        e.MessageLimit = 1;
                    });
                });
            });
        });
    })
    .Build();

host.Run();
