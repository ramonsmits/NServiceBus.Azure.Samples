namespace VideoStore.Sales
{
    using System;
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
        }
    }

    public class ConfigureEncryption : INeedInitialization
    {
        public void Customize(BusConfiguration builder)
        {
            builder.RijndaelEncryptionService();
        }
    }
}
