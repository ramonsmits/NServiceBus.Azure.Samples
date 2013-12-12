using System.Diagnostics;
using NServiceBus.Features;
using NServiceBus.Logging;

namespace VideoStore.Operations
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue>
    ,
        IWantCustomInitialization
    {
        public void Init()
        {
            //SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
        }
    }

    public class DefineEndpointName : INeedInitialization
    {
        public void Init()
        {
            Configure.Instance
                .DefineEndpointName(() => "VideoStore.Operations");
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
    public class DisableTimeoutManager : INeedInitialization
    {
        public void Init()
        {
            Feature.Disable<SecondLevelRetries>();
            Feature.Disable<TimeoutManager>();
        }
    }
}
