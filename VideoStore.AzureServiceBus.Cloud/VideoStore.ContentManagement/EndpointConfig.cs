using System.Diagnostics;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;
using NServiceBus.Config;
using NServiceBus.Features;
using NServiceBus.Hosting.Helpers;

namespace VideoStore.ContentManagement
{
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
            Trace.WriteLine("The VideoStore.ContentManagement endpoint is now started and subscribed to OrderAccepted events from VideoStore.Sales");
        }

        public void Stop()
        {

        }
    }

    ///// <summary>
    ///// This is just here so that topics are created irrespective of boot order of the processes
    ///// </summary>
    //public class AutoCreateDependantTopics : IWantToRunWhenConfigurationIsComplete
    //{
    //    public void Run()
    //    {
    //        var topicCreator = new AzureServicebusTopicCreator();

    //        topicCreator.Create(AzureServiceBusPublisherAddressConventionForSubscriptions.Apply(Address.Parse("VideoStore.Sales")));
    //    }
    //}

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
