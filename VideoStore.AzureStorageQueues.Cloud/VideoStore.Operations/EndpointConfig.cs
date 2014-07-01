using NServiceBus.Features;

namespace VideoStore.Operations
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue>
    {
        public void Customize(ConfigurationBuilder builder)
        {
            builder.Conventions(c =>
                c.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Commands"))
                 .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Events"))
                 .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
                 .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted")));
        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableTimeoutManager : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<TimeoutManager>();
        }
    }
}
