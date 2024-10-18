using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Frwk = ServiceBus.Framework.Infrastructure;

namespace Messaging.POC.BLL.Implementations.Service_Bus
{
    public class Publisher : IPublisher
    {
        private string _sendMessageSubject = ConfigurationManager.AppSettings["sendMessageSubject"];
        private string _sendRequestMessageSubject = ConfigurationManager.AppSettings["sendRequestMessageSubject"];
        private string _sendReplyMessageSubject = ConfigurationManager.AppSettings["sendReplyMessageSubject"];
        private string _namespace_connection_string = ConfigurationManager.AppSettings["namespace_connection_string"];
        private string _queue_name = ConfigurationManager.AppSettings["queue_name"];
        private Frwk.Transport _transport;
        private Channel _channel;

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();


                _transport = new Frwk.NetTransport(_namespace_connection_string, _queue_name);
                _channel = new Channel(_transport);
                Console.WriteLine("\nService Bus Publisher started running..");
                bool flag = true;

                while (flag)
                {
                    Console.WriteLine("Press 1 to test Send, 2 to test SendRequest, 3 to test SendReply, and x to exit");
                    var line = Console.ReadLine().ToUpper();


                    switch (line)
                    {

                        case "1":
                            SendMessage();
                            break;
                        case "2":
                            SendRequestMessage();
                            break;
                        case "3":
                            SendReplyMessage();
                            break;
                        case "X":
                            flag = false;
                            break;
                        default:
                            break;
                    }

                }

                Frwk.Environment.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendMessage()
        {
            CustomMessage customMsg = GetCustomMessage(_sendMessageSubject, 1);


            _channel.SendMessage(customMsg);
            Console.WriteLine("\nService Bus SendMessage Completed..");
        }

        private void SendRequestMessage()
        {
            CustomMessage customMsg = GetCustomMessage(_sendRequestMessageSubject, 2);

            CustomMessage responseMsg = _channel.SendRequestMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);
            string responseMsgStr = JsonConvert.SerializeObject(responseMsg);


            Console.WriteLine("\nService Bus SendRequestMessage Completed..");
            Console.WriteLine($"customMsg: {customMsgStr}");
            Console.WriteLine($"responseMsg: {responseMsgStr}");
        }

        private void SendReplyMessage()
        {

            CustomMessage customMsg = GetCustomMessage(_sendMessageSubject, 3);
            customMsg.ReplySubject = _sendReplyMessageSubject;

            _channel.SendMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine("\nService Bus SendReplyMessage Completed..");
            Console.WriteLine($"customMsg: {customMsgStr}");
        }

        private CustomMessage GetCustomMessage(string sendSubject, int counter)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.SendSubject = sendSubject;
            customMsg.Name = $"Saurabh {counter}";
            customMsg.Age = 39;
            customMsg.Department = $"I.T. {counter}";
            customMsg.Address = $"Whitefield {counter}";

            return customMsg;
        }

    }
}
