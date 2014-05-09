using NServiceBus.Logging;

namespace VideoStore.CustomerRelations
{
    using System.Diagnostics;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class SendLimitedTimeOffer : IHandleMessages<ClientBecamePreferred>
    {
        static ILog Log = LogManager.GetLogger(typeof(SendLimitedTimeOffer));

        public void Handle(ClientBecamePreferred message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }
            Log.DebugFormat("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {0}", message.ClientId);
        }
    }
}
