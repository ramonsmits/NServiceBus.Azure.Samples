using NServiceBus.Hosting.Helpers;

namespace VideoStore.Operations
{
    using System;
    using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, UsingTransport<AzureServiceBus>
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

    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.Out.WriteLine("The VideoStore.Operations endpoint is now started and ready to accept messages");
        }

        public void Stop()
        {

        }
    }
}
