using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Case00041163.OccasionalSubscriber";

        var endpointConfiguration = new EndpointConfiguration(Console.Title);
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        transport.Routing().RegisterPublisher(typeof(MyEvent), "Case00041163.Publisher");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Unsubscribe<MyEvent>().ConfigureAwait(false);

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}