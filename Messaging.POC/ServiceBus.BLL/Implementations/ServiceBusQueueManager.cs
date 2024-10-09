using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.BLL.Implementations
{
    public class ServiceBusQueueManager
    {
        ServiceBusQueueSender _sender;
        ServiceBusQueueReceiver _receiver;
        private string _namespace_connection_string;
        private string _queue_name;

        public ServiceBusQueueManager(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

            _sender = new ServiceBusQueueSender(_namespace_connection_string, _queue_name);
            _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _queue_name);

            _sender.Init();
            _receiver.Init();

        }

        public async Task StartListening()
        {
            await _receiver.Start();
        }

        public async Task StopListening()
        {
            await _receiver.Stop();
        }

        public async Task StopListening(string message)
        {
            await _sender.Send(message);
        }

    }
}
