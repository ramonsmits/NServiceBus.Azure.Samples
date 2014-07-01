using System.Diagnostics;
using NServiceBus.Features;
using NServiceBus.Hosting.Helpers;
using NServiceBus.Logging;
using NServiceBus.Unicast;

namespace VideoStore.ContentManagement
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue>
    {
        public void Customize(ConfigurationBuilder builder)
        {
            builder.Conventions(c =>
                     c.DefiningCommandsAs(
                         t =>
                             t.Namespace != null && t.Namespace.StartsWith("VideoStore") &&
                             t.Namespace.EndsWith("Commands"))
                         .DefiningEventsAs(
                             t =>
                                 t.Namespace != null && t.Namespace.StartsWith("VideoStore") &&
                                 t.Namespace.EndsWith("Events"))
                         .DefiningMessagesAs(
                             t =>
                                 t.Namespace != null && t.Namespace.StartsWith("VideoStore") &&
                                 t.Namespace.EndsWith("RequestResponse"))
                         .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted")));

        }
    }

    public class ShowTheBusStarted : IWantToRunWhenBusStartsAndStops
    {
        static ILog Log = LogManager.GetLogger(typeof(ShowTheBusStarted));

        public void Start()
        {
            Log.DebugFormat("The VideoStore.ContentManagement endpoint is now started and subscribed to OrderAccepted events from VideoStore.Sales");
        }

        public void Stop()
        {

        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableFeatures : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<TimeoutManager>();
        }
    }
}
