using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusQueueManager
    {
        private ServiceBusQueueSender _sender;
        private ServiceBusQueueReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _queue_name = string.Empty;
        private event MessageReceivedEventHandler _messageReceived;
        private object _closure;

        public ServiceBusQueueManager(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

            _sender = new ServiceBusQueueSender(_namespace_connection_string, _queue_name);
            _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _queue_name);

        }

        public async Task StartListening(MessageReceivedEventHandler messageReceived, object closure)
        {
            _messageReceived = messageReceived;
            _closure = closure;

            await _receiver.Start(MessageHandler, ErrorHandler);
        }

        public async Task StopListening()
        {
            await _receiver.Stop();
        }

        public async Task SendMessage(string message)
        {
            await _sender.Send(message);
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 

            Message msg = JsonConvert.DeserializeObject<Message>(body);
            MessageReceivedEventArgs mrea = new MessageReceivedEventArgs(msg, _closure);

            if (_messageReceived != null)
                _messageReceived(null, mrea);

            //await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
