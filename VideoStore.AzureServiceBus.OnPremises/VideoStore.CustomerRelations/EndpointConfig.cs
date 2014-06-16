using NServiceBus.Azure.Transports.WindowsAzureServiceBus;
using NServiceBus.Config;
using NServiceBus.Hosting.Helpers;

namespace VideoStore.CustomerRelations
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<AzureServiceBus>,
        IWantCustomInitialization
    {
        public Configure Init()
        {
            var assemblyScanner = new AssemblyScanner();
            assemblyScanner.MustReferenceAtLeastOneAssembly.Add(typeof(IHandleMessages<>).Assembly);
            var assembliesToScan = assemblyScanner
                .GetScannableAssemblies()
                .Assemblies;

            var endpointName = this.GetType().Namespace ?? this.GetType().Assembly.GetName().Name;
            //endpointVersionToUse = FileVersionRetriever.GetFileVersion(specifier.GetType());

            var config = Configure.With(o =>
            {
                o.EndpointName(endpointName);
                // o.EndpointVersion(() => endpointVersionToUse);
                o.AssembliesToScan(assembliesToScan);
                o.Conventions(c =>
                        c.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Commands"))
                         .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Events"))
                         .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
                         .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted")));
            });

            return config;
        }
    }
    
    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.Out.WriteLine("The VideoStore.CustomerRelations endpoint is now started and subscribed to events from VideoStore.Sales");
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
    //    public void Run(Configure config)
    //    {
    //        var topicCreator = new AzureServicebusTopicCreator();

    //        topicCreator.Create(AzureServiceBusPublisherAddressConventionForSubscriptions.Apply(Address.Parse("VideoStore.Sales")));
    //    }

    //}
}
