namespace VideoStore.Messages.Events
{
    using System.Collections.Generic;

    public class DownloadIsReady 
    {
        public int OrderNumber { get; set; }
        public Dictionary<string, string> VideoUrls { get; set; }
        public string ClientId { get; set; }
    }
}