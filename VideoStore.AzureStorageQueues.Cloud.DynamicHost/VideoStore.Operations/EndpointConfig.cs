using System.Diagnostics;
using NServiceBus.Features;
using NServiceBus.Hosting.Helpers;
using NServiceBus.Logging;

namespace VideoStore.Operations
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue>, UsingPersistence<AzureStorage>
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
            Log.DebugFormat("The VideoStore.Operations endpoint is now started");
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
            config.DisableFeature<Sagas>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<TimeoutManager>();
        }
    }
}
