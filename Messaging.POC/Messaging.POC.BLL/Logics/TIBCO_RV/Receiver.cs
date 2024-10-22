using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using System;
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Receiver : IReceiver
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private MessagingTypes _messagingType = Helper.GetMessagingType();
        private AppTypes _appType = AppTypes.Receiver;

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

                Preprocessing();

                ConsoleHelper.StartApp(_messagingType, _appType);

                _channel.Subscribe(Configs.SENDMESSAGESUBJECT, SendListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREQUESTMESSAGESUBJECT, SendRequestListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREPLYMESSAGESUBJECT, SendReplyListener_MessageReceived);

                _channel.Dispatch();

                Postprocessing();

                Frwk.Environment.Close();

                ConsoleHelper.ExitApp();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Postprocessing()
        {
        }

        private void SendListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {
            ListenerTypes listenerType = ListenerTypes.SendListener;
            string subject = Configs.SENDMESSAGESUBJECT;

            ConsoleHelper.StartListener(_messagingType, listenerType, subject);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 4);
            }

            ConsoleHelper.CompleteListener(_messagingType, listenerType, subject);

        }

        private void SendRequestListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {

            ListenerTypes listenerType = ListenerTypes.SendRequestListener;
            string subject = Configs.SENDREQUESTMESSAGESUBJECT;

            ConsoleHelper.StartListener(_messagingType, listenerType, subject);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 5);

            }

            ConsoleHelper.CompleteListener(_messagingType, listenerType, subject);
        }

        private void SendReplyListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {
            ListenerTypes listenerType = ListenerTypes.SendReplyListener;
            string subject = Configs.SENDREPLYMESSAGESUBJECT;

            ConsoleHelper.StartListener(_messagingType, listenerType, subject);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 6);
            }

            ConsoleHelper.CompleteListener(_messagingType, listenerType, subject);
        }

        private void SendReply(CustomMessage requestMsg, int counter)
        {
            ActionTypes actionType = ActionTypes.SendReply;

            ConsoleHelper.StartAction(_messagingType, actionType);

            CustomMessage replyMsg = CustomMessageHelper.GetCustomMessage("Sajid", 45, "Exec", "Marathalli", counter);

            ConsoleHelper.DisplayCustomRequestMessage(replyMsg);

            _channel.SendReplyMessage(replyMsg, requestMsg);

            ConsoleHelper.CompleteAction(_messagingType, actionType);
        }


    }
}
