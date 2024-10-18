//using Newtonsoft.Json;
using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Interfaces;
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
using TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Implementations.TIBCO_RV
{
    public class Receiver : IReceiver
    {
        private string _sendMessageSubject = "MS.Send.TEST";
        private string _sendRequestMessageSubject = "MS.SendRequest.TEST";
        private string _sendReplyMessageSubject = "MS.SendReply.TEST";
        private string _service = ConfigurationManager.AppSettings["service"];
        private string _network = ConfigurationManager.AppSettings["network"];
        private string _daemon = ConfigurationManager.AppSettings["daemon"];
        private Transport _transport;
        private Channel _channel;
        private TIBCO.Rendezvous.Queue _queue;

        public void Run()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();

                _transport = new NetTransport(_service, _network, _daemon);
                _channel = new Channel(_transport);
                _queue = TIBCO.Rendezvous.Queue.Default;


                Listener sendListener = new Listener(
                       _queue,
                        _transport,
                        _sendMessageSubject,
                        new object()
                        );
                sendListener.MessageReceived += new MessageReceivedEventHandler(SendListener_MessageReceived);


                Listener sendRequestListener = new Listener(
                       _queue,
                        _transport,
                        _sendRequestMessageSubject,
                        new object()
                        );
                sendRequestListener.MessageReceived += new MessageReceivedEventHandler(SendRequestListener_MessageReceived);

                Listener sendReplyListener = new Listener(
                      _queue,
                       _transport,
                       _sendReplyMessageSubject,
                       new object()
                       );
                sendReplyListener.MessageReceived += new MessageReceivedEventHandler(SendReplyListener_MessageReceived);

                Console.WriteLine("\nTIBCO RV Receiver started running..");


                var dispacher = new Dispatcher(_queue);
                dispacher.Join();


                TIBCO.Rendezvous.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadLine();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendListener_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine("\nTIBCO RV SendListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                CustomMessage replyCustomMessage = GetCustomMessage("Sajid 1", 45, "Exec 1", "Marathalli 1");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }

        }

        private void SendRequestListener_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine("\nTIBCO RV SendRequestListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");


            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                CustomMessage replyCustomMessage = GetCustomMessage("Sajid 2", 46, "Exec 2", "Marathalli 2");
                _channel.SendReplyMessage(replyCustomMessage, customMsg);
            }
        }

        private void SendReplyListener_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine("\nTIBCO RV SendReplyListener_Message Received..");
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
