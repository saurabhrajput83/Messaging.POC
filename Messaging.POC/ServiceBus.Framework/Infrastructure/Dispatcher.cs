using ServiceBus.Framework.Implementations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Dispatcher
    {
        private ServiceBusReceiverManager _serviceBusReceiverManager = null;
        private Queue _queue = null;


        public Dispatcher(Queue queue, string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            _queue = queue;
            _serviceBusReceiverManager = new ServiceBusReceiverManager(namespace_connection_string, topic_or_queue_name, subscription_name);
        }

        public void Join(Dictionary<string, Listener> listeners)
        {
            Task.Run(async () => await _serviceBusReceiverManager.StartListening(listeners)).GetAwaiter().GetResult();
        }
    }
}
