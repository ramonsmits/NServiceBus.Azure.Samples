using NServiceBus.Logging;

namespace VideoStore.ContentManagement
{
    using System.Diagnostics;
    using Common;
    using Messages.RequestResponse;
    using Messages.Events;
    using NServiceBus;

    public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        static ILog Log = LogManager.GetLogger(typeof(OrderAcceptedHandler));

        public IBus Bus { get; set; }

        public void Handle(OrderAccepted message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }

            Log.DebugFormat("Order # {0} has been accepted, Let's provision the download -- Sending ProvisionDownloadRequest to the VideoStore.Operations endpoint", message.OrderNumber);

            //send out a request (a event will be published when the response comes back)
            Bus.Send<ProvisionDownloadRequest>(r =>
            {
                r.ClientId = message.ClientId;
                r.OrderNumber = message.OrderNumber;
                r.VideoIds = message.VideoIds;
            });
        }
    }
}