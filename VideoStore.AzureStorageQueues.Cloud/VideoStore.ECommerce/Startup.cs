using Microsoft.Owin;
using Owin;
using VideoStore.ECommerce;

[assembly: OwinStartup(typeof(Startup))]

namespace VideoStore.ECommerce
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}