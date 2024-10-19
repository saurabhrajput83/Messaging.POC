using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
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

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Publisher : IPublisher
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private string _messagingType = Helper.GetMessagingType().ToString();

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();


                _transport = new Frwk.NetTransport(Configs.SERVICE, Configs.NETWORK, Configs.DAEMON);
                _channel = new Channel(_transport);
                Console.WriteLine($"\n{_messagingType} Publisher started running..");
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
            CustomMessage customMsg = CustomMessageHelper.GetCustomMessage(Configs.SENDMESSAGESUBJECT, 1);


            _channel.SendMessage(customMsg);
            Console.WriteLine($"\n{_messagingType} SendMessage Completed..");
        }

        private void SendRequestMessage()
        {
            CustomMessage customMsg = CustomMessageHelper.GetCustomMessage(Configs.SENDREQUESTMESSAGESUBJECT, 2);

            CustomMessage responseMsg = _channel.SendRequestMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);
            string responseMsgStr = JsonConvert.SerializeObject(responseMsg);


            Console.WriteLine($"\n{_messagingType} SendRequestMessage Completed..");
            Console.WriteLine($"customMsg: {customMsgStr}");
            Console.WriteLine($"responseMsg: {responseMsgStr}");
        }

        private void SendReplyMessage()
        {

            CustomMessage customMsg = CustomMessageHelper.GetCustomMessage(Configs.SENDMESSAGESUBJECT, 3);
            customMsg.ReplySubject = Configs.SENDREPLYMESSAGESUBJECT;

            _channel.SendMessage(customMsg);

            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\n{_messagingType} SendReplyMessage Completed..");
            Console.WriteLine($"customMsg: {customMsgStr}");
        }

    }
}
