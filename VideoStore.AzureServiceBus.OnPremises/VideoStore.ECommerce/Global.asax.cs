using NServiceBus.Azure.Transports.WindowsAzureServiceBus;
using NServiceBus.Config;
using NServiceBus.Hosting.Helpers;

namespace VideoStore.ECommerce
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using log4net.Appender;
    using log4net.Core;
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    public class MvcApplication : HttpApplication
    {
        private static IBus bus;

        private IStartableBus startableBus;

        public static IBus Bus
        {
            get { return bus; }
        }

        protected void Application_Start()
        {
            startableBus = Configure.With(o =>
            {
                //o.EndpointName(endpointName);
                // o.EndpointVersion(() => endpointVersionToUse);
            
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
            })
                .UseTransport<AzureServiceBus>()
                .PurgeOnStartup(true)
                .ScaleOut(s => s.UseSingleBrokerQueue())
                .RijndaelEncryptionService()
                .EnableInstallers()
                .CreateBus();
                

            //Configure.Instance.ForInstallationOn<Windows>().Install();

            bus = startableBus
              //  .RunInstallers()
                .Start();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            startableBus.Dispose();
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
    //        topicCreator.Create(AzureServiceBusPublisherAddressConventionForSubscriptions.Apply(Address.Parse("VideoStore.ContentManagement")));
    //    }
    //}
}