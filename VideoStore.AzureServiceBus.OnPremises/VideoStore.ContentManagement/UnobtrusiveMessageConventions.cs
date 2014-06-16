//namespace VideoStore.Common
//{
//    using NServiceBus;

//    class UnobtrusiveMessageConventions : INeedInitialization
//    {
//        public void Init(Configure config)
//        {
//            config.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Commands"))
//                     .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("Events"))
//                     .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
//                     .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
//        }
//    }
//}


