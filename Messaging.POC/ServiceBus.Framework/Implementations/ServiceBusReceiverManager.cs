using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Helpers;
using ServiceBus.Framework.Infrastructure;
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
        private ServiceBusQueueSender _sender;
        private ServiceBusQueueReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _topic_or_queue_name = string.Empty;
        private string _subscription_name = string.Empty;
        private Dictionary<string, Listener> _listeners;

        public ServiceBusReceiverManager(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _topic_or_queue_name = topic_or_queue_name;
            _subscription_name = subscription_name;

            _sender = new ServiceBusQueueSender(_namespace_connection_string, _topic_or_queue_name);
            _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _topic_or_queue_name);

        }

        public async Task StartListening(Dictionary<string, Listener> listeners)
        {
            await _receiver.Start(MessageHandler, ErrorHandler);
            _listeners = listeners;
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


            Console.WriteLine($"Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);
            SubjectHelper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            if (_listeners.ContainsKey(sendSubject))
            {
                Listener listener = _listeners[sendSubject];
                MessageReceivedEventArgs mrArgs = new MessageReceivedEventArgs(msg, listener.Closure);

                listener.MessageReceivedEventHandler(listener, mrArgs);
            }

            //MessageReceivedEventArgs mrea = new MessageReceivedEventArgs(msg, _closure);

            //if (_messageReceived != null)
            //    _messageReceived(null, mrea);

            //await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Exception: {args.Exception.ToString()}");
            //return Task.CompletedTask;
        }
    }
}
