using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBus.Framework.Infrastructure;
using ServiceBus.Framework.Interfaces;
using ServiceBus.Framework.Logics;
using ServiceBus.Framework.Utilities;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusSenderManager
    {
        private IServiceBusSender _sender;
        private IServiceBusReceiver _receiver;
        private SendRequestLogic _sendRequestLogic;
        private string _appType = ServiceBusAppTypes.Publisher.ToString();


        public ServiceBusSenderManager(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
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

            ServiceBusActionTypes actionType;
            Enum.TryParse<ServiceBusActionTypes>(actionTypeStr, out actionType);


            switch (actionType)
            {
                case ServiceBusActionTypes.SendReply:

                    if (this._sendRequestLogic.Requests.Contains(sendSubject))
                    {

                        ConsoleHelper.StartProcessMessageHandler(_appType, actionTypeStr, subject, body);

                        Message msg = JsonConvert.DeserializeObject<Message>(body);

                        this._sendRequestLogic.AddResponseMessages(sendSubject, msg);

                        ConsoleHelper.CompleteProcessMessageHandler(_appType);
                    }

                    break;
            }



            // complete the message. message is deleted from the queue. 
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
