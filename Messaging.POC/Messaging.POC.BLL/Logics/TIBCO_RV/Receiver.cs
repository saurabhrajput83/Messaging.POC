﻿using Messaging.POC.BLL;
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
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Receiver : IReceiver
    {
        private Frwk.Transport _transport;
        private Channel _channel;
        private MessagingTypes _messagingType = Helper.GetMessagingType();
        private AppTypes _appType = AppTypes.Receiver;

        public void Run()
        {
            try
            {
                Frwk.Environment.Open();

                _transport = new Frwk.NetTransport(Configs.SERVICE, Configs.NETWORK, Configs.DAEMON);
                _channel = new Channel(_transport);

                ConsoleHelper.StartApp(_messagingType, _appType);

                _channel.Subscribe(Configs.SENDMESSAGESUBJECT, SendListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREQUESTMESSAGESUBJECT, SendRequestListener_MessageReceived);

                _channel.Subscribe(Configs.SENDREPLYMESSAGESUBJECT, SendReplyListener_MessageReceived);

                _channel.Dispatch();

                Frwk.Environment.Close();

                ConsoleHelper.ExitApp();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {
            ListenerTypes listenerType = ListenerTypes.SendListener;

            ConsoleHelper.StartListener(_messagingType, listenerType);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 1);
            }

            ConsoleHelper.CompleteListener(_messagingType, listenerType);

        }

        private void SendRequestListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {

            ListenerTypes listenerType = ListenerTypes.SendRequestListener;

            ConsoleHelper.StartListener(_messagingType, listenerType);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 2);

            }

            ConsoleHelper.CompleteListener(_messagingType, listenerType);
        }

        private void SendReplyListener_MessageReceived(object listener, CustomMessageReceivedEventArgs args)
        {
            ListenerTypes listenerType = ListenerTypes.SendReplyListener;

            ConsoleHelper.StartListener(_messagingType, listenerType);

            CustomMessage requestMsg = args.Message;

            ConsoleHelper.DisplayListenerCustomRequestMessage(requestMsg);

            if (!string.IsNullOrEmpty(requestMsg.ReplySubject))
            {
                SendReply(requestMsg, 3);
            }

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
