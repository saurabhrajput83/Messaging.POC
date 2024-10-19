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
        private string _namespace_connection_string = ConfigurationManager.AppSettings["namespace_connection_string"];
        private string _queue_name = ConfigurationManager.AppSettings["queue_name"];
        private ServiceBusQueueManager _serviceBusQueueManager = null;
        private Queue _queue = null;


        public Dispatcher(Queue queue)
        {
            _queue = queue;
            _serviceBusQueueManager = new ServiceBusQueueManager(_namespace_connection_string, _queue_name);
        }

        public void Join(Dictionary<string, Listener> listeners)
        {
            Task.Run(async () => await _serviceBusQueueManager.StartListening(listeners)).GetAwaiter().GetResult();
        }
    }
}
