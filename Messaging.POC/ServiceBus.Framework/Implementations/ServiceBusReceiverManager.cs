using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Interfaces;
using ServiceBus.Framework.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusReceiverManager
    {
        private IServiceBusSender _sender;
        private IServiceBusReceiver _receiver;
        private Dictionary<string, Listener> _listeners;
        private string _appType = ServiceBusAppTypes.Receiver.ToString();

        public ServiceBusReceiverManager(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            string serviceBusType = ConfigurationManager.AppSettings["serviceBusType"];

            if (serviceBusType == "Topic")
            {
                _sender = new ServiceBusTopicSender(namespace_connection_string, topic_or_queue_name, subscription_name);
                _receiver = new ServiceBusTopicReceiver(namespace_connection_string, topic_or_queue_name, subscription_name);
            }
            else
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
            string messageId = pmArgs.Message.MessageId;
            string subject = pmArgs.Message.Subject.ToString();
            string body = pmArgs.Message.Body.ToString();

            string sendSubject = subject;
            string actionType = string.Empty;
            if (pmArgs.Message.ApplicationProperties["ActionType"] != null)
                actionType = pmArgs.Message.ApplicationProperties["ActionType"].ToString();


            Dictionary<string, Listener> listeners = this._listeners;


            ConsoleHelper.StartProcessMessageHandler(_appType, actionType, subject, body);
            // complete the message. message is deleted from the queue. 

            Message msg = JsonConvert.DeserializeObject<Message>(body);

            if (listeners.ContainsKey(sendSubject))
            {
                Listener listener = listeners[sendSubject];
                MessageReceivedEventArgs mrArgs = new MessageReceivedEventArgs(msg, listener.Closure);

                listener.OnMessageReceivedEventHandler(listener, mrArgs);
            }

            ConsoleHelper.CompleteProcessMessageHandler(_appType);
            await pmArgs.CompleteMessageAsync(pmArgs.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            ConsoleHelper.ProcessErrorHandler(_appType, args.Exception);
            return Task.CompletedTask;
        }
    }
}
