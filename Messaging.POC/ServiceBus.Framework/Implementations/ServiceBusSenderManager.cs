using Azure.Core;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Interfaces;
using ServiceBus.Framework.Logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private SendRequestLogic _sendRequestLogic;


        public ServiceBusSenderManager(ServiceBusTypes serviceBusType, string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            if (serviceBusType == ServiceBusTypes.Topic)
            {
                _sender = new ServiceBusTopicSender(namespace_connection_string, topic_or_queue_name, subscription_name);
                _receiver = new ServiceBusTopicReceiver(namespace_connection_string, topic_or_queue_name, subscription_name);
            }
            else if (serviceBusType == ServiceBusTypes.Queue)
            {
                _sender = new ServiceBusQueueSender(namespace_connection_string, topic_or_queue_name);
                _receiver = new ServiceBusQueueReceiver(namespace_connection_string, topic_or_queue_name);
            }

            _sendRequestLogic = new SendRequestLogic();



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
            string subject = Helper.CreateSubject(ServiceBusActionTypes.Send, message.SendSubject);
            string body = JsonConvert.SerializeObject(message);

            await _sender.Send(subject, body);
        }

        public async Task<Message> SendRequestMessage(Message requestMessage, double timeout)
        {

            string replySubject = _sendRequestLogic.CreateNewRequest();

            requestMessage.SendSubject = requestMessage.SendSubject ?? "";
            requestMessage.ReplySubject = replySubject;


            string subject = Helper.CreateSubject(ServiceBusActionTypes.SendRequest, requestMessage.SendSubject);
            string body = JsonConvert.SerializeObject(requestMessage);

            await _sender.Send(subject, body);

            Message responseMessage = await _sendRequestLogic.GetResponse(replySubject, timeout);

            return responseMessage;

        }

        public async Task SendReplyMessage(Message reply, Message request)
        {

            reply.SendSubject = reply.SendSubject ?? request.ReplySubject;
            reply.ReplySubject = string.Empty;

            string subject = Helper.CreateSubject(ServiceBusActionTypes.SendReply, reply.SendSubject);
            string body = JsonConvert.SerializeObject(reply);

            await _sender.Send(subject, body);
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs pmArgs)
        {
            ServiceBusActionTypes actionType;
            string sendSubject;
            string messageId = pmArgs.Message.MessageId;
            string subject = pmArgs.Message.Subject.ToString();
            string body = pmArgs.Message.Body.ToString();


            Console.WriteLine($"\nServiceBusSenderManager Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);

            Helper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            switch (actionType)
            {
                case ServiceBusActionTypes.SendReply:
                    this._sendRequestLogic.AddResponseMessages(sendSubject, msg);
                    break;
            }

            await pmArgs.CompleteMessageAsync(pmArgs.Message);

        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"\nServiceBusSenderManager Exception: {args.Exception.ToString()}");
            return Task.CompletedTask;
        }
    }
}
