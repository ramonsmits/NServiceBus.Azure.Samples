using System.Diagnostics;
using NServiceBus.Features;

namespace VideoStore.ContentManagement
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration builder)
        {
            builder.UseTransport<AzureStorageQueue>();
            builder.UsePersistence<AzureStorage>();
            
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
            Trace.WriteLine(string.Format("The VideoStore.ContentManagement endpoint is now started and subscribed to OrderAccepted events from VideoStore.Sales"));
        }

        public void Stop()
        {

        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableFeatures : INeedInitialization
    {
        public void Customize(BusConfiguration builder)
        {
            builder.DisableFeature<SecondLevelRetries>();
            builder.DisableFeature<TimeoutManager>();
        }
    }
}
