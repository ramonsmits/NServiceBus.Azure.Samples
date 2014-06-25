namespace VideoStore.Messages.Events
{
    //NServiceBus messages can be defined using both classes and interfaces
    public class OrderAccepted 
    {
        public int OrderNumber { get; set; }
        public string[] VideoIds { get; set; }
        public string ClientId { get; set; }
    }
}