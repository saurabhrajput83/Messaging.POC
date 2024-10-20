using Azure.Core;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Interfaces;
using ServiceBus.Framework.Logics;
using ServiceBus.Framework.Utilities;
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
        private string _appType = ServiceBusAppTypes.Publisher.ToString();


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
            ServiceBusActionTypes serviceBusActionType = ServiceBusActionTypes.Send;
            string subject = message.SendSubject;
            string body = JsonConvert.SerializeObject(message);

            await _sender.Send(serviceBusActionType, subject, body);
        }

        public async Task<Message> SendRequestMessage(Message requestMessage, double timeout)
        {
            ServiceBusActionTypes serviceBusActionType = ServiceBusActionTypes.SendRequest;
            string subject = requestMessage.SendSubject;
            string replySubject = _sendRequestLogic.CreateNewRequest();

            requestMessage.ReplySubject = replySubject;

            string body = JsonConvert.SerializeObject(requestMessage);

            await _sender.Send(serviceBusActionType, subject, body);

            Message responseMessage = await _sendRequestLogic.GetResponse(replySubject, timeout);

            return responseMessage;

        }

        public async Task SendReplyMessage(Message reply, Message request)
        {
            ServiceBusActionTypes serviceBusActionType = ServiceBusActionTypes.SendReply;
            string subject = request.ReplySubject;

            reply.SendSubject = subject;
            reply.ReplySubject = string.Empty;

            string body = JsonConvert.SerializeObject(reply);

            await _sender.Send(serviceBusActionType, subject, body);
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs pmArgs)
        {
            string messageId = pmArgs.Message.MessageId;
            string subject = pmArgs.Message.Subject.ToString();
            string body = pmArgs.Message.Body.ToString();

            string sendSubject = subject;
            string actionTypeStr = string.Empty;
            if (pmArgs.Message.ApplicationProperties["ActionType"] != null)
                actionTypeStr = pmArgs.Message.ApplicationProperties["ActionType"].ToString();


            ConsoleHelper.StartProcessMessageHandler(_appType, actionTypeStr, subject, body);

            Message msg = JsonConvert.DeserializeObject<Message>(body);

            // complete the message. message is deleted from the queue. 
            ServiceBusActionTypes actionType;
            Enum.TryParse<ServiceBusActionTypes>(actionTypeStr, out actionType);

            switch (actionType)
            {
                case ServiceBusActionTypes.SendReply:
                    this._sendRequestLogic.AddResponseMessages(sendSubject, msg);
                    break;
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
