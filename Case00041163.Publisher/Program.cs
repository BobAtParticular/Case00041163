using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static async Task Main()
    {
        Console.Title = "Case00041163.Publisher";

        var endpointConfiguration = new EndpointConfiguration(Console.Title);
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Start(endpointInstance).ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press '1' to publish MyEvent");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D1)
            {
                var orderReceived = new MyEvent();

                await endpointInstance.Publish(orderReceived)
                    .ConfigureAwait(false);
                Console.WriteLine($"Published MyEvent");
            }
            else
            {
                return;
            }
        }
    }
}