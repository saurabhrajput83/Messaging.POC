using Azure.Core;
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
    public class ServiceBusSenderManager
    {
        private IServiceBusSender _sender;
        private IServiceBusReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _topic_or_queue_name = string.Empty;
        private string _subscription_name = string.Empty;
        private List<string> _sendRequests;
        private Dictionary<string, Message> _sendRequestsResponses;


        public ServiceBusSenderManager(ServiceBusType serviceBusType, string namespace_connection_string, string topic_or_queue_name, string subscription_name)
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

            _sendRequests = new List<string>();
            _sendRequestsResponses = new Dictionary<string, Message>();

        }

        public async Task StartListening()
        {
            await _receiver.Start(MessageHandler, ErrorHandler);
        }

        public async Task StopListening()
        {
            await _receiver.Stop();
        }

        public async Task SendMessage(Message message)
        {
            string subject = SubjectHelper.CreateSubject(ActionTypes.Send, message.SendSubject);
            string body = JsonConvert.SerializeObject(message);

            await _sender.Send(subject, body);
        }

        public async Task<Message> SendRequestMessage(Message requestMessage, double timeout)
        {

            string replySubject = Guid.NewGuid().ToString();

            requestMessage.SendSubject = requestMessage.SendSubject ?? "";
            requestMessage.ReplySubject = replySubject;

            _sendRequests.Add(replySubject);


            await SendMessage(requestMessage);

            Message responseMessage = null;
            DateTime maxTime = DateTime.Now.AddMilliseconds(timeout);
            DateTime currentTime = DateTime.Now;

            //maxTime.

            //while (currentTime.Millisecond < maxTime.Millisecond)
            //{
            //    if (_messages.ContainsKey(subject))
            //    {
            //        responseMessage = _messages[subject];
            //        break;
            //    }
            //    currentTime = DateTime.Now;
            //}

            return responseMessage;

        }

        public async Task SendReplyMessage(Message reply, Message request)
        {

            reply.SendSubject = reply.SendSubject ?? request.ReplySubject;
            reply.ReplySubject = string.Empty;

            await SendMessage(reply);
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs pmArgs)
        {
            ActionTypes actionType;
            string sendSubject;

            string messageId = pmArgs.Message.MessageId;
            string subject = pmArgs.Message.Subject.ToString();
            string body = pmArgs.Message.Body.ToString();


            Console.WriteLine($"\nServiceBusSenderManager Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);

            SubjectHelper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            switch (actionType)
            {
                case ActionTypes.SendReply:
                    if (_sendRequests.Contains(sendSubject))
                    {
                        _sendRequestsResponses.Add(sendSubject, msg);
                    }
                    break;
            }



        }

        // handle any errors when receiving messages
        async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"\nServiceBusSenderManager Exception: {args.Exception.ToString()}");
            //return Task.CompletedTask;
        }
    }
}
