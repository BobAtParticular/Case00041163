using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Raw;
using NServiceBus.Routing;
using NServiceBus.Transport;
using NServiceBus.Unicast.Transport;

class Program
{
    static async Task Main()
    {
        Console.Title = "Case00041163.OccasionalSubscriber";

        var endpointConfiguration = RawEndpointConfiguration.Create(Console.Title, (context, messages) => Task.CompletedTask, "error");

        endpointConfiguration.UseTransport<MsmqTransport>();

        var endpointInstance = await RawEndpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var subscriptionMessage = ControlMessageFactory.Create(MessageIntentEnum.Subscribe);

        subscriptionMessage.Headers[Headers.SubscriptionMessageType] = typeof(MyEvent).AssemblyQualifiedName;
        subscriptionMessage.Headers[Headers.ReplyToAddress] = "Case00041163.OccasionalSubscriber@FAKEPC";
        subscriptionMessage.Headers[Headers.SubscriberTransportAddress] = "Case00041163.OccasionalSubscriber@FAKEPC";
        subscriptionMessage.Headers[Headers.SubscriberEndpoint] = "Case00041163.OccasionalSubscriber";
        subscriptionMessage.Headers[Headers.TimeSent] = DateTimeExtensions.ToWireFormattedString(DateTime.UtcNow);
        subscriptionMessage.Headers[Headers.NServiceBusVersion] = "5.6.4";

        var operation = new TransportOperation(subscriptionMessage, new UnicastAddressTag("Case00041163.Publisher"));

        await endpointInstance.Dispatch(new TransportOperations(operation), new TransportTransaction(), new ContextBag()).ConfigureAwait(false);

        await endpointInstance.Stop().ConfigureAwait(false);
    }
}