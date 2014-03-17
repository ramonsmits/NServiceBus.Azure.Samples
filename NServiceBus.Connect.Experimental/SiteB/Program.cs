using NServiceBus.Features;

namespace SiteB
{
    using System;
    using NServiceBus;

    class Program
    {
        static void Main()
        {
            Feature.Disable<NServiceBus.Features.Gateway>();
            Feature.Enable<NServiceBus.Connect.Features.Gateway>();

            Configure.With()
                .DefaultBuilder()
                .UseTransport<Msmq>()
                .UnicastBus()
                .AzureDataBus()
                .CreateBus()
                .Start();

            Console.WriteLine("Waiting for price updates from the headquarter - press any key to exit");

            Console.ReadLine();
        }
    }
}
