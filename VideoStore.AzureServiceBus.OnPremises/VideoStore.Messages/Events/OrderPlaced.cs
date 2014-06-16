namespace VideoStore.Messages.Events
{
    public class OrderPlaced
    {
        public int OrderNumber { get; set; }
        public string[] VideoIds { get; set; }
        public string ClientId { get; set; }
    }
}