using NServiceBus.Features;

namespace Headquarter
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
            
            //Feature.Enable<NServiceBus.Gateway.V2.Features.Gateway>(c => c.);
            
            //Configure.Features.Gateway(c => c);

            Configure.With()
                .DefaultBuilder()
                .AzureDataBus();
            // .FileShareDataBus(".\\databus");
        }
    }

}
