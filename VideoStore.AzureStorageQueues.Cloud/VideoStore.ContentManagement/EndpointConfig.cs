using System.Diagnostics;
using NServiceBus.Features;

namespace VideoStore.ContentManagement
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue> { }
    
    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Trace.WriteLine(string.Format("The VideoStore.ContentManagement endpoint is now started and subscribed to OrderAccepted events from VideoStore.Sales"));
        }

        public void Stop()
        {

        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableFeatures : INeedInitialization
    {
        public void Init()
        {
            Feature.Disable<SecondLevelRetries>();
            Feature.Disable<TimeoutManager>();
        }
    }
}
