using Autofac;
using Festify.Indexer.Documents;
using Festify.Indexer.Elasticsearch;
using Festify.Indexer.Consumers;
using Festify.Indexer.Updaters;
using GreenPipes;
using MassTransit;
using Nest;
using System;
using System.Threading.Tasks;

namespace Festify.Indexer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.Register(context =>
            {
                var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                    .DefaultMappingFor<ShowDocument>(m => m
                        .IndexName("shows")
                    )
                    .DefaultMappingFor<ActDocument>(m => m
                        .IndexName("acts")
                    )
                    .DefaultMappingFor<VenueDocument>(m => m
                        .IndexName("venues")
                    );
                return new ElasticClient(settings);
            }).SingleInstance();

            builder
                .RegisterType<ElasticsearchRepository>()
                .AsImplementedInterfaces();
            builder.RegisterModule<IndexerModule>();

            builder.AddMassTransit(busConfig =>
            {
                busConfig.UsingRabbitMq((context, queueConfig) =>
                {
                    queueConfig.Host("rabbitmq://localhost");
                    queueConfig.ReceiveEndpoint("Festify.Indexer", endpointConfig =>
                    {
                        endpointConfig.UseMessageRetry(r => r.Exponential(
                            retryLimit: 5,
                            minInterval: TimeSpan.FromSeconds(5),
                            maxInterval: TimeSpan.FromSeconds(30),
                            intervalDelta: TimeSpan.FromSeconds(5)));
                    });
                });

                busConfig.AddConsumer<ShowAddedConsumer>();
                busConfig.AddConsumer<ActDescriptionChangedConsumer>();
                busConfig.AddConsumer<VenueDescriptionChangedConsumer>();
                busConfig.AddConsumer<VenueLocationChangedConsumer>();
            });

            var container = builder.Build();

            var bus = container.Resolve<IBusControl>();

            await bus.StartAsync();

            Console.WriteLine("Receiving messages. Press a key to stop.");
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();
        }
    }
}
