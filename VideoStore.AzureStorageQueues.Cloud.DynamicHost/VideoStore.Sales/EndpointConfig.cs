using System.Diagnostics;
using NServiceBus.Logging;

namespace VideoStore.Sales
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue>, IWantCustomInitialization
    {
        public void Init()
        {
            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);

            Configure.With()
                .DefaultBuilder()
                .RijndaelEncryptionService();
        }
    }

    public class DefineEndpointName : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .DefineEndpointName(() => "VideoStore.Sales");
        }
    }

    public class ShowTheBusStarted : IWantToRunWhenBusStartsAndStops
    {
        static ILog Log = LogManager.GetLogger(typeof(ShowTheBusStarted));

        public void Start()
        {
            Log.DebugFormat("The VideoStore.Sales endpoint is now started and subscribed to events from VideoStore.Sales");
        }

        public void Stop()
        {

        }
    }
}
