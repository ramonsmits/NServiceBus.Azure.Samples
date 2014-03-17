using NServiceBus.Features;

namespace SiteA
{
    using NServiceBus;

    // The endpoint is started with the RunGateway profile which turns it on. The Lite profile is also
    // active which will configure the persistence to be InMemory
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Feature.Disable<NServiceBus.Features.Gateway>();
            Feature.Enable<NServiceBus.Connect.Features.Gateway>();

            Configure.With()
                .DefaultBuilder()
                .AzureDataBus();
               // .FileShareDataBus(".\\databus");
        }
    }
}
