using Festify.Sales.States;
using MassTransit;

namespace Festify.Sales
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var salesStateMachine = new SalesStateMachine();
            var repository = new InMemorySagaRepository<SalesState>();

            var bus = Bus.Factory.CreateUsingRabbitMq(busConfig =>
            {
                busConfig.Host("rabbitmq://localhost");
                busConfig.ReceiveEndpoint("Festify.Sales", e =>
                {
                    e.StateMachineSaga(salesStateMachine, repository);
                });
            });

            await bus.StartAsync();

            Console.WriteLine("Receiving messages. Press a key to stop.");
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();
        }
    }
}
