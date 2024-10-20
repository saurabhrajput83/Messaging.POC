﻿using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using Messaging.POC.BLL.Logics.Service_Bus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Channel : IChannel
    {
        private Frwk.Transport _transport;
        private Frwk.Queue _queue;
        private double _timeout = 5000;
        private Dictionary<string, Frwk.Listener> _listeners;

        public Channel(Frwk.Transport transport)
        {
            _transport = transport;
            _queue = Frwk.Queue.Default;
            _listeners = new Dictionary<string, Frwk.Listener>();
        }

        public void SendMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = ConvertToFrwkMessage(customMsg);

            _transport.Send(msg);
        }

        public CustomMessage SendRequestMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = ConvertToFrwkMessage(customMsg);

            Frwk.Message replyMsg = _transport.SendRequest(msg, _timeout);

            return ConvertToCustomMessage(replyMsg);
        }

        public void SendReplyMessage(CustomMessage customReplyMsg, CustomMessage customMsg)
        {
            Frwk.Message replyMsg = ConvertToFrwkMessage(customReplyMsg);
            Frwk.Message msg = ConvertToFrwkMessage(customMsg);


            _transport.SendReply(replyMsg, msg);

        }

        public bool Subscribe(string subject, CustomMessageReceivedEventHandler messageHandler)
        {
            if (_listeners.ContainsKey(subject))
                return false;

            Frwk.Listener listener = new Frwk.Listener(
                  _queue,
                    OnMessageReceivedEventHandler,
                    _transport,
                   subject,
                   messageHandler
                   );


            _listeners.Add(subject, listener);
            return true;
        }

        public void Dispatch()
        {
            foreach (KeyValuePair<string, Frwk.Listener> listener in _listeners)
            {
                ConsoleHelper.DisplayListenerStarted(listener.Key);
            }

            var dispacher = new Frwk.Dispatcher(_queue);
            dispacher.Join();
        }

        protected void OnMessageReceivedEventHandler(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = ConvertToCustomMessage(msg);

            Helper.FrwkOnMessageReceivedEventHandlerStarted(msg, customMsg);

            CustomMessageReceivedEventHandler handler = (CustomMessageReceivedEventHandler)args.Closure;

            if (handler != null)
            {
                CustomMessageReceivedEventArgs cmrArgs = new CustomMessageReceivedEventArgs(customMsg);
                handler(this, cmrArgs);
            }

            Helper.FrwkOnMessageReceivedEventHandlerCompleted();
        }

        private Frwk.Message ConvertToFrwkMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = new Frwk.Message();

            if (customMsg != null)
            {
                msg.SendSubject = customMsg.SendSubject;
                msg.ReplySubject = customMsg.ReplySubject;

                msg.AddField(new Frwk.MessageField("Name", customMsg.Name));
                msg.AddField(new Frwk.MessageField("Age", customMsg.Age));
                msg.AddField(new Frwk.MessageField("Department", customMsg.Department));
                msg.AddField(new Frwk.MessageField("Address", customMsg.Address));
            }

            return msg;
        }

        public CustomMessage ConvertToCustomMessage(Frwk.Message msg)
        {
            CustomMessage customMsg = new CustomMessage();

            if (msg != null)
            {
                customMsg.SendSubject = msg.SendSubject;
                customMsg.ReplySubject = msg.ReplySubject;

                customMsg.Name = (string)msg.GetField("Name").Value;
                customMsg.Age = (int)msg.GetField("Age").Value;
                customMsg.Department = (string)msg.GetField("Department").Value;
                customMsg.Address = (string)msg.GetField("Address").Value;
            }

            return customMsg;
        }
    }
}
