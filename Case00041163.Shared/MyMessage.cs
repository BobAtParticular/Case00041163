using NServiceBus;

[TimeToBeReceived("00:00:20")]
public class MyEvent :
    IEvent
{
}