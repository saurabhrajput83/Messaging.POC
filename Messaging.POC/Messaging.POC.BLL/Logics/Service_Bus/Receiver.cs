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
        private string _sendMessageSubject = ConfigurationManager.AppSettings["sendMessageSubject"];
        private string _sendRequestMessageSubject = ConfigurationManager.AppSettings["sendRequestMessageSubject"];
        private string _sendReplyMessageSubject = ConfigurationManager.AppSettings["sendReplyMessageSubject"];
        private string _service = ConfigurationManager.AppSettings["service"];
        private string _network = ConfigurationManager.AppSettings["network"];
        private string _daemon = ConfigurationManager.AppSettings["daemon"];
        private string _namespace_connection_string = ConfigurationManager.AppSettings["namespace_connection_string"];
        private string _queue_name = ConfigurationManager.AppSettings["queue_name"];
        private Frwk.Transport _transport;
        private Channel _channel;
        private Frwk.Queue _queue;
        private string _messagingType = MessagingType.Service_Bus.ToString();

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();

                _transport = new Frwk.NetTransport(_namespace_connection_string, _queue_name);
                _channel = new Channel(_transport);
                _queue = Frwk.Queue.Default;


                Frwk.Listener sendListener = new Frwk.Listener(
                       _queue,
                        _transport,
                        SendListener_MessageReceived,
                        _sendMessageSubject,
                        new object()
                        );


                Frwk.Listener sendRequestListener = new Frwk.Listener(
                       _queue,
                        _transport,
                        SendRequestListener_MessageReceived,
                        _sendRequestMessageSubject,
                        new object()
                        );


                Frwk.Listener sendReplyListener = new Frwk.Listener(
                      _queue,
                       _transport,
                       SendReplyListener_MessageReceived,
                       _sendReplyMessageSubject,
                       new object()
                       );

                Console.WriteLine($"\n{_messagingType} Receiver started running..");


                var dispacher = new Frwk.Dispatcher(_queue);
                dispacher.Join();


                Frwk.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadLine();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendListener_MessageReceived(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\n{_messagingType} SendListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                CustomMessage replyCustomMessage = GetCustomMessage("Sajid 1", 45, "Exec 1", "Marathalli 1");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }

        }

        private void SendRequestListener_MessageReceived(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine($"\n{_messagingType} SendRequestListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");


            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                CustomMessage replyCustomMessage = GetCustomMessage("Sajid 2", 46, "Exec 2", "Marathalli 2");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }
        }

        private void SendReplyListener_MessageReceived(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\n{_messagingType} SendReplyListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                CustomMessage replyCustomMessage = GetCustomMessage("Sajid 3", 47, "Exec 3", "Marathalli 3");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }

        }

        private CustomMessage GetCustomMessage(string name, int age, string department, string address)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.Name = name;
            customMsg.Age = age;
            customMsg.Department = department;
            customMsg.Address = address;

            return customMsg;
        }
    }
}
