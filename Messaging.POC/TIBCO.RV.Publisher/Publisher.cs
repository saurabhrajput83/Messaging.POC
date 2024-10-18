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
using TIBCO.Rendezvous;

namespace TIBCO.RV.Publisher
{
    public class Publisher : IPublisher
    {
        private string _sendMessageSubject = "MS.Send.TEST";
        private string _sendRequestMessageSubject = "MS.SendRequest.TEST";
        private string _sendReplyMessageSubject = "MS.SendReply.TEST";
        private string _service = ConfigurationManager.AppSettings["service"];
        private string _network = ConfigurationManager.AppSettings["network"];
        private string _daemon = ConfigurationManager.AppSettings["daemon"];
        private Transport _transport;
        private Channel _channel;

        public void Run()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();


                _transport = new NetTransport(_service, _network, _daemon);
                _channel = new Channel(_transport);
                Console.WriteLine("Publisher started running..");
                bool flag = true;

                while (flag)
                {
                    Console.WriteLine("Press 1 to test Send, 2 to test SendRequest, 3 to test SendRequest2, 4 to test SendReply, and x to exit");
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
                            SendRequestMessage2();
                            break;
                        case "4":
                            SendReplyMessage();
                            break;
                        case "X":
                            flag = false;
                            break;
                        default:
                            break;
                    }

                }

                TIBCO.Rendezvous.Environment.Close();

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
            Console.WriteLine("SendMessage Completed..");
        }

        private void SendRequestMessage()
        {
            CustomMessage customMsg = GetCustomMessage(_sendRequestMessageSubject, 2);

            Message msg = _channel.SendRequestMessage(customMsg);
            CustomMessage customMsg2 = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsg2Str = JsonConvert.SerializeObject(customMsg2);


            Console.WriteLine("SendRequestMessage Completed..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage2: {customMsg2Str}");
        }

        private void SendRequestMessage2()
        {
            CustomMessage customMsg = GetCustomMessage(_sendRequestMessageSubject, 3);
            customMsg.ReplySubject = Guid.NewGuid().ToString();

            Message msg = _channel.SendRequestMessage(customMsg);
            CustomMessage customMsg2 = _channel.ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsg2Str = JsonConvert.SerializeObject(customMsg2);


            Console.WriteLine("SendRequestMessage2 Completed..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage2: {customMsg2Str}");
        }

        private void SendReplyMessage()
        {

            CustomMessage customMsg = GetCustomMessage(_sendMessageSubject, 4);
            customMsg.ReplySubject = _sendReplyMessageSubject;

            _channel.SendMessage(customMsg);


            Console.WriteLine("SendReplyMessage Completed..");
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
