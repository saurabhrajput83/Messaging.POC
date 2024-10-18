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
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Implementations.Service_Bus
{
    public class Publisher : IPublisher
    {
        private string _sendMessageSubject = "MS.Send.TEST";
        private string _sendRequestMessageSubject = "MS.SendRequest.TEST";
        private string _sendReplyMessageSubject = "MS.SendReply.TEST";
        private string _service = ConfigurationManager.AppSettings["service"];
        private string _network = ConfigurationManager.AppSettings["network"];
        private string _daemon = ConfigurationManager.AppSettings["daemon"];
        private Frwk.Transport _transport;
        private Channel _channel;

        public void Run()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();


                _transport = new Frwk.NetTransport(_service, _network, _daemon);
                _channel = new Channel(_transport);
                Console.WriteLine("\nTIBCO RV Publisher started running..");
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
            Console.WriteLine("\nTIBCO RV SendMessage Completed..");
        }

        private void SendRequestMessage()
        {
            CustomMessage customMsg = GetCustomMessage(_sendRequestMessageSubject, 2);

            CustomMessage responseMsg = _channel.SendRequestMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);
            string responseMsgStr = JsonConvert.SerializeObject(responseMsg);


            Console.WriteLine("\nTIBCO RV SendRequestMessage Completed..");
            Console.WriteLine($"customMsg: {customMsgStr}");
            Console.WriteLine($"responseMsg: {responseMsgStr}");
        }

        private void SendReplyMessage()
        {

            CustomMessage customMsg = GetCustomMessage(_sendMessageSubject, 3);
            customMsg.ReplySubject = _sendReplyMessageSubject;

            _channel.SendMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine("\nTIBCO RV SendReplyMessage Completed..");
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
