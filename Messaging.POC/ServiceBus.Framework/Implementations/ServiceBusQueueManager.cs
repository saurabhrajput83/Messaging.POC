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
    public class ServiceBusQueueManager
    {
        private ServiceBusQueueSender _sender;
        private ServiceBusQueueReceiver _receiver;
        private string _namespace_connection_string = string.Empty;
        private string _queue_name = string.Empty;
        private event MessageReceivedEventHandler _messageReceived;
        private object _closure;
        private Dictionary<string, Message> _messages;

        public ServiceBusQueueManager(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

            _sender = new ServiceBusQueueSender(_namespace_connection_string, _queue_name);
            _receiver = new ServiceBusQueueReceiver(_namespace_connection_string, _queue_name);
            _messages = new Dictionary<string, Message>();

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

        public async Task SendMessage(Message message)
        {
            string subject = SubjectHelper.CreateSubject(ActionTypes.Send, message.SendSubject);
            string body = JsonConvert.SerializeObject(message);

            await _sender.Send(subject, body);
        }

        public async Task<Message> SendRequest(Message requestMessage, double timeout)
        {
            string subject = SubjectHelper.CreateSubject(ActionTypes.Send, requestMessage.SendSubject);
            string body = JsonConvert.SerializeObject(requestMessage);

            await _sender.Send(ActionTypes.SendRequest.ToString(), body);

            Message responseMessage = null;
            DateTime maxTime = DateTime.Now.AddMilliseconds(timeout);
            DateTime currentTime = DateTime.Now;

            while (currentTime.Millisecond < maxTime.Millisecond)
            {
                if (_messages.ContainsKey(subject))
                {
                    responseMessage = _messages[subject];
                    break;
                }
                currentTime = DateTime.Now;
            }

            return responseMessage;

        }

        public async Task SendReply(Message reply, Message request)
        {
            string body = JsonConvert.SerializeObject(reply);
            await _sender.Send(ActionTypes.SendReply.ToString(), body);
        }

        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            ActionTypes actionType;
            string sendSubject;

            string messageId = args.Message.MessageId;
            string subject = args.Message.Subject.ToString();
            string body = args.Message.Body.ToString();

            //args.

            Console.WriteLine($"Received: {messageId} {subject} {body}");

            Message msg = JsonConvert.DeserializeObject<Message>(body);
            SubjectHelper.ParseSubject(subject, out actionType, out sendSubject);


            // complete the message. message is deleted from the queue. 


            switch (actionType)
            {
                case ActionTypes.Send:
                    break;
                case ActionTypes.SendRequest:
                    break;
                case ActionTypes.SendReply:
                    break;
            }


            MessageReceivedEventArgs mrea = new MessageReceivedEventArgs(msg, _closure);

            if (_messageReceived != null)
                _messageReceived(null, mrea);

            //await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Exception: {args.Exception.ToString()}");
            return Task.CompletedTask;
        }
    }
}
