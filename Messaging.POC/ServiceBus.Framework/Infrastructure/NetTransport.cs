namespace ServiceBus.Framework.Infrastructure
{
    public class NetTransport : Transport
    {

        public NetTransport(string namespace_connection_string, string queue_name, string subscription_name) :
            base(namespace_connection_string, queue_name, subscription_name)
        {


        }
    }
}
