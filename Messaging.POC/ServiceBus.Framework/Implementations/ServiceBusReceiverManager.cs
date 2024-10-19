using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
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
        private Dictionary<string, Listener> _listeners;

        public ServiceBusReceiverManager(ServiceBusType serviceBusType, string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            if (serviceBusType == ServiceBusType.Topic)
            {
                _sender = new ServiceBusTopicSender(namespace_connection_string, topic_or_queue_name, subscription_name);
                _receiver = new ServiceBusTopicReceiver(namespace_connection_string, topic_or_queue_name, subscription_name);
            }
            else if (serviceBusType == ServiceBusType.Queue)
            {
                _sender = new ServiceBusQueueSender(namespace_connection_string, topic_or_queue_name);
                _receiver = new ServiceBusQueueReceiver(namespace_connection_string, topic_or_queue_name);

            }

            _listeners = new Dictionary<string, Listener>();
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
            Dictionary<string, Listener> listeners = this._listeners;


            Console.WriteLine($"\nServiceBusReceiverManager Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);
            Helper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            if (listeners.ContainsKey(sendSubject))
            {
                Listener listener = listeners[sendSubject];
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
