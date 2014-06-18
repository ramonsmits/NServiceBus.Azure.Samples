using System.Diagnostics;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;
using NServiceBus.Config;
using NServiceBus.Features;
using NServiceBus.Hosting.Helpers;

namespace VideoStore.CustomerRelations
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureServiceBus>
        , IWantCustomInitialization
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
            });

            return config;
        }
    }
    
    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Trace.WriteLine(string.Format("The VideoStore.CustomerRelations endpoint is now started and subscribed to events from VideoStore.Sales"));
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
            config.Features(f =>
            {
                f.Disable<TimeoutManager>();
                f.Disable<SecondLevelRetries>();
            });
           
        }
    }
}
