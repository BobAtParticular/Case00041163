using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Performance.TimeToBeReceived;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

namespace Case00041163.Publisher
{
    public class DispatchPublishedMessagesWithTtbrBehavior : Behavior<IDispatchContext>
    {
        public override async Task Invoke(IDispatchContext context, Func<Task> next)
        {
            foreach (var operation in context.Operations.Where(o =>
                o.AddressTag is UnicastAddressTag unicastAddress &&
                unicastAddress.Destination.StartsWith("Case00041163.OccasionalSubscriber")))
            {
                operation.DeliveryConstraints.Add(new DiscardIfNotReceivedBefore(TimeSpan.FromSeconds(20)));
            }
            await next().ConfigureAwait(false);
        }
    }
}
