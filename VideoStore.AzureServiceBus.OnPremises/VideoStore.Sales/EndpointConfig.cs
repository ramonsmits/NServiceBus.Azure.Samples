using NServiceBus.Hosting.Helpers;

namespace VideoStore.Sales
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<AzureServiceBus>, IWantCustomInitialization
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

    public class MyClass:IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.Out.WriteLine("The VideoStore.Sales endpoint is now started and ready to accept messages");
        }

        public void Stop()
        {
            
        }
    }

    public class ConfigureEncryption: INeedInitialization
    {
        public void Init(Configure config)
        {
            config.RijndaelEncryptionService();
        }
    }
}
