namespace VideoStore.Messages.Events
{
    public class OrderCancelled 
    {
        public int OrderNumber { get; set; }
        public string ClientId { get; set; }
    }
}