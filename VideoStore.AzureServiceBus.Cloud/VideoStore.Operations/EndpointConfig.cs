using System.Diagnostics;
using NServiceBus.Features;
using NServiceBus.Hosting.Helpers;

namespace VideoStore.Operations
{
    using System;
    using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureServiceBus>, UsingPersistence<AzureStorage>
    
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

    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Trace.WriteLine(string.Format("The VideoStore.Operations endpoint is now started and ready to accept messages"));
        }

        public void Stop()
        {

        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableTimeoutManager : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.DisableFeature<TimeoutManager>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<Sagas>();
        }
    }
}
