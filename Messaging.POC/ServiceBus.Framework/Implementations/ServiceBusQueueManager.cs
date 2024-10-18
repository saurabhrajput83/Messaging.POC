using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusQueueManager
    {
        ServiceBusQueueSender _sender;
        ServiceBusQueueReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _queue_name = string.Empty;

        public ServiceBusQueueManager(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

            _sender = new ServiceBusQueueSender(_namespace_connection_string, _queue_name);
            _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _queue_name);

        }

        public async Task StartListening(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
        {
            //await _receiver.Start(messageHandler, errorHandler);
        }

        public async Task StopListening()
        {
            await _receiver.Stop();
        }

        public async Task SendMessage(string message)
        {
            await _sender.Send(message);
        }

    }
}
