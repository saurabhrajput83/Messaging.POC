using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Frwk = ServiceBus.Framework.Infrastructure;

namespace Messaging.POC.BLL.Logics.Service_Bus
{
    public class Receiver : IReceiver
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private Frwk.Queue _queue;
        private string _messagingType = MessagingType.Service_Bus.ToString();

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();

                _transport = new Frwk.NetTransport(Configs.NAMESPACE_CONNECTION_STRING, Configs.TOPIC_OR_QUEUE_NAME, Configs.SUBSCRIPTION_NAME);
                _channel = new Channel(_transport);

                _channel.Subscribe(Configs.SENDMESSAGESUBJECT, SendListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREQUESTMESSAGESUBJECT, SendRequestListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREPLYMESSAGESUBJECT, SendReplyListener_MessageReceived);

                Console.WriteLine($"\n{_messagingType} Receiver started running..");

                _channel.Dispatch();

                Frwk.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadLine();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {

            CustomMessage customMsg = args.Message;


            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\n{_messagingType} SendListener_Message Received..");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            if (!string.IsNullOrEmpty(customMsg.ReplySubject))
            {
                CustomMessage replyCustomMessage = CustomMessageHelper.GetCustomMessage("Sajid 1", 45, "Exec 1", "Marathalli 1");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }

        }

        private void SendRequestListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {

            CustomMessage customMsg = args.Message;

            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine($"\n{_messagingType} SendRequestListener_Message Received..");
            Console.WriteLine($"CustomMessage: {customMsgStr}");


            if (!string.IsNullOrEmpty(customMsg.ReplySubject))
            {
                CustomMessage replyCustomMessage = CustomMessageHelper.GetCustomMessage("Sajid 2", 46, "Exec 2", "Marathalli 2");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }
        }

        private void SendReplyListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {
            CustomMessage customMsg = args.Message;

            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\n{_messagingType} SendReplyListener_Message Received..");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            if (!string.IsNullOrEmpty(customMsg.ReplySubject))
            {
                CustomMessage replyCustomMessage = CustomMessageHelper.GetCustomMessage("Sajid 3", 47, "Exec 3", "Marathalli 3");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }

        }
    }
}
