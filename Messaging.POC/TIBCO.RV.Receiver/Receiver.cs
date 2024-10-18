//using Newtonsoft.Json;
using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
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

namespace TIBCO.RV.Receiver
{
    public class Receiver
    {
        private string _sendMessageSubject = "MS.Send.TEST";
        private string _sendRequestMessageSubject = "MS.SendRequest.TEST";
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

                var dispacher = new Dispatcher(_queue);
                dispacher.Join();

                Console.WriteLine("Receiver started running..");
                Console.ReadLine();

                TIBCO.Rendezvous.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadKey();

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


            Console.WriteLine("SendListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

        }

        private void SendRequestListener_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            Message msg = args.Message;
            CustomMessage customMsg = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine("SendRequestListener_Message Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");


            if (!string.IsNullOrEmpty(msg.ReplySubject))
            {
                Message replyMsg = GetReplyMessage("Sajid", 45, "Exec", "Marathalli");
                _channel.SendReplyMessage(replyMsg, msg);
            }
        }

        private Message GetReplyMessage(string name, int age, string department, string address)
        {
            Message msg = new Message();

            msg.AddField(new MessageField("Name", name));
            msg.AddField(new MessageField("Age", age));
            msg.AddField(new MessageField("Department", department));
            msg.AddField(new MessageField("Address", address));

            return msg;
        }
    }
}
