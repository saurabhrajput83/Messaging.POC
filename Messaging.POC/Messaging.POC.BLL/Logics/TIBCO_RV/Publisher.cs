using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using System;
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Publisher : IPublisher
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private MessagingTypes _messagingType = Helper.GetMessagingType();
        private AppTypes _appType = AppTypes.Publisher;

        public void Preprocessing()
        {
        }

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();


                _transport = new Frwk.NetTransport(Configs.SERVICE, Configs.NETWORK, Configs.DAEMON);
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
