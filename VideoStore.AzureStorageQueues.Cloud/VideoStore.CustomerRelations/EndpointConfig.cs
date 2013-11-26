using System.Diagnostics;
using NServiceBus.Features;

namespace VideoStore.CustomerRelations
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue> { }
    
    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
           Trace.WriteLine("The VideoStore.CustomerRelations endpoint is now started and subscribed to events from VideoStore.Sales");
        }

        public void Stop()
        {

        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableTimeoutManager : INeedInitialization
    {
        public void Init()
        {
            Feature.Disable<SecondLevelRetries>();
            Feature.Disable<TimeoutManager>();
        }
    }
}
