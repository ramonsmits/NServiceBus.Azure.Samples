
namespace VideoStore.ECommerce
{
	using NServiceBus;
	using NServiceBus.Installation.Environments;
	using NServiceBus.Features;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class MvcApplication : HttpApplication
	{
		private static IBus _bus;

		private IStartableBus _startableBus;

		public static IBus Bus
		{
			get { return _bus; }
		}

		protected void Application_Start()
		{
			Feature.Disable<TimeoutManager>();

			_startableBus = Configure.With()
				.DefaultBuilder()
				.TraceLogger()
				.UseTransport<AzureStorageQueue>()
				.PurgeOnStartup(true)
				.UnicastBus()
				.RunHandlersUnderIncomingPrincipal(false)
				.RijndaelEncryptionService()
				.CreateBus();

			Configure.Instance.ForInstallationOn<Windows>().Install();

			_bus = _startableBus.Start();

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		protected void Application_End()
		{
			_startableBus.Dispose();
		}
	}
}