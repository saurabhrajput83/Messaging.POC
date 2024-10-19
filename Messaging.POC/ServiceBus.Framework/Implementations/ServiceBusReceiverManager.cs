using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Helpers;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusReceiverManager
    {
        private IServiceBusSender _sender;
        private IServiceBusReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _topic_or_queue_name = string.Empty;
        private string _subscription_name = string.Empty;
        private Dictionary<string, Listener> _listeners;

        public ServiceBusReceiverManager(ServiceBusType serviceBusType, string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _topic_or_queue_name = topic_or_queue_name;
            _subscription_name = subscription_name;

            if (serviceBusType == ServiceBusType.Topic)
            {
                _sender = new ServiceBusTopicSender(_namespace_connection_string, _topic_or_queue_name, subscription_name);
                _receiver = new ServiceBusTopicReceiver(_namespace_connection_string, _topic_or_queue_name, subscription_name);
            }
            else if (serviceBusType == ServiceBusType.Queue)
            {
                _sender = new ServiceBusQueueSender(_namespace_connection_string, _topic_or_queue_name);
                _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _topic_or_queue_name);

            }


        }

        public async Task StartListening(Dictionary<string, Listener> listeners)
        {
            _listeners = listeners;
            await _receiver.Start(MessageHandler, ErrorHandler);
        }

        public async Task StopListening()
        {
            await _receiver.Stop();
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs pmArgs)
        {
            ActionTypes actionType;
            string sendSubject;

            string messageId = pmArgs.Message.MessageId;
            string subject = pmArgs.Message.Subject.ToString();
            string body = pmArgs.Message.Body.ToString();


            Console.WriteLine($"\nServiceBusReceiverManager Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);
            SubjectHelper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            if (_listeners.ContainsKey(sendSubject))
            {
                Listener listener = _listeners[sendSubject];
                MessageReceivedEventArgs mrArgs = new MessageReceivedEventArgs(msg, listener.Closure);

                listener.MessageReceivedEventHandler(listener, mrArgs);
            }

            await pmArgs.CompleteMessageAsync(pmArgs.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"\nServiceBusReceiverManager Exception: {args.Exception.ToString()}");
            return Task.CompletedTask;
        }
    }
}
