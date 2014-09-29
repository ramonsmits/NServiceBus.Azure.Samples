using System.Diagnostics;
using NServiceBus.Features;

namespace VideoStore.CustomerRelations
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration builder)
        {
            builder.UseTransport<AzureStorageQueueTransport>();
            builder.UsePersistence<AzureStoragePersistence>();

            builder.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));

            builder.RijndaelEncryptionService();
        }
    }
    
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
        public void Customize(BusConfiguration builder)
        {
            builder.DisableFeature<SecondLevelRetries>();
            builder.DisableFeature<TimeoutManager>();
        }
    }
}
