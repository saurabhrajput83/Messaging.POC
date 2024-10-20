using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class NetTransport : Transport
    {

        public NetTransport(ServiceBusTypes serviceBusType, string namespace_connection_string, string queue_name, string subscription_name) : base(serviceBusType, namespace_connection_string, queue_name, subscription_name)
        {


        }
    }
}
