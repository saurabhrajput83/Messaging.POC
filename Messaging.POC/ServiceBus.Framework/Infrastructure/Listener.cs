using Microsoft.Azure.Amqp.Framing;
using ServiceBus.Framework.Implementations;
using ServiceBus.Framework.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Listener
    {
        private string _namespace_connection_string = ConfigurationManager.AppSettings["namespace_connection_string"];
        private string _queue_name = ConfigurationManager.AppSettings["queue_name"];
        private ServiceBusQueueManager _serviceBusQueueManager = null;

        public Listener(Queue queue, Transport transport, MessageReceivedEventHandler onMessageReceived, string subject, object closure)
        {
            _serviceBusQueueManager = new ServiceBusQueueManager(_namespace_connection_string, _queue_name);

            Init(onMessageReceived, closure);
        }

        private void Init(MessageReceivedEventHandler onMessageReceived, object closure)
        {
            Task.Run(async () => await _serviceBusQueueManager.StartListening(onMessageReceived, closure)).GetAwaiter().GetResult();
        }

    }
}
