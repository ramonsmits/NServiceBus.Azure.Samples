namespace VideoStore.CustomerRelations
{
    using System.Diagnostics;
    using Messages.Events;
    using NServiceBus;
    using Common;

    class SendLimitedTimeOffer : IHandleMessages<ClientBecamePreferred>
    {
        public void Handle(ClientBecamePreferred message)
        {
            if (DebugFlagMutator.Debug)
            {
                Debugger.Break();
            }
            Trace.WriteLine(string.Format("Handler WhenCustomerIsPreferredSendLimitedTimeOffer invoked for CustomerId: {0}", message.ClientId));
        }
    }
}
