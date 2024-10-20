using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using Newtonsoft.Json;
using ServiceBus.Framework;
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

namespace Messaging.POC.BLL.Logics.Service_Bus
{
    public class Publisher : IPublisher
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private MessagingTypes _messagingType = Helper.GetMessagingType();
        private AppTypes _appType = AppTypes.Publisher;
        private ServiceBusTypes _serviceBusType = Helper.GetDefaultServiceBusType();

        public void Preprocessing()
        {
            _transport.StartListening();
        }

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();


                _transport = new Frwk.NetTransport(_serviceBusType, Configs.NAMESPACE_CONNECTION_STRING, Configs.TOPIC_OR_QUEUE_NAME, Configs.SUBSCRIPTION_NAME);
                _channel = new Channel(_transport);
                ConsoleHelper.StartApp(_messagingType, _appType);
                bool flag = true;

                Preprocessing();

                while (flag)
                {
                    ConsoleHelper.DisplayActions();

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

                Postprocessing();

                Frwk.Environment.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Postprocessing()
        {
        }

        private void SendMessage()
        {
            ActionTypes actionType = ActionTypes.Send;
            string sendSubject = Configs.SENDMESSAGESUBJECT;

            ConsoleHelper.StartAction(_messagingType, actionType);

            CustomMessage requestMsg = CustomMessageHelper.GetCustomMessage(sendSubject, 1);

            ConsoleHelper.DisplayCustomRequestMessage(requestMsg);

            _channel.SendMessage(requestMsg);

            ConsoleHelper.CompleteAction(_messagingType, actionType);
        }

        private void SendRequestMessage()
        {
            ActionTypes actionType = ActionTypes.SendRequest;
            string sendSubject = Configs.SENDREQUESTMESSAGESUBJECT;

            ConsoleHelper.StartAction(_messagingType, actionType);

            CustomMessage requestMsg = CustomMessageHelper.GetCustomMessage(sendSubject, 2);

            ConsoleHelper.DisplayCustomRequestMessage(requestMsg);

            DateTime dtStartTime = DateTime.Now;

            CustomMessage responseMsg = _channel.SendRequestMessage(requestMsg);

            ConsoleHelper.DisplayCustomResponseMessage(responseMsg);

            ConsoleHelper.DisplayTimeDifference(dtStartTime);

            ConsoleHelper.CompleteAction(_messagingType, actionType);
        }

        private void SendReplyMessage()
        {
            ActionTypes actionType = ActionTypes.SendReply;
            string sendSubject = Configs.SENDMESSAGESUBJECT;
            string replySubject = Configs.SENDREPLYMESSAGESUBJECT;

            ConsoleHelper.StartAction(_messagingType, actionType);

            CustomMessage requestMsg = CustomMessageHelper.GetCustomMessage(sendSubject, 3);
            requestMsg.ReplySubject = replySubject;

            ConsoleHelper.DisplayCustomRequestMessage(requestMsg);

            _channel.SendMessage(requestMsg);

            ConsoleHelper.CompleteAction(_messagingType, actionType);
        }

    }
}
