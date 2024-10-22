using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using ServiceBus.Framework;
using ServiceBus.Framework.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frwk = ServiceBus.Framework.Infrastructure;

namespace Messaging.POC.BLL.Logics.Service_Bus
{

    public class Channel : IChannel
    {
        private Frwk.Transport _transport;
        private Frwk.Queue _queue;
        private double _timeout = 30000;
        private Dictionary<string, Frwk.Listener> _listeners;
        private ServiceBusReceiverManager _serviceBusReceiverManager;

        public Channel(Frwk.Transport transport)
        {
            _transport = transport;
            _queue = Frwk.Queue.Default;
            _listeners = new Dictionary<string, Frwk.Listener>();
            _serviceBusReceiverManager = new ServiceBusReceiverManager(Configs.NAMESPACE_CONNECTION_STRING, Configs.TOPIC_OR_QUEUE_NAME, Configs.SUBSCRIPTION_NAME);
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

            Task.Run(async () => await _serviceBusReceiverManager.StartListening(_listeners)).GetAwaiter().GetResult();
        }

        protected void OnMessageReceivedEventHandler(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = ConvertToCustomMessage(msg);

            Helper.FrwkOnMessageReceivedEventHandlerStarted(msg, customMsg);

            CustomMessageReceivedEventHandler handler = (CustomMessageReceivedEventHandler)args.Closure;

            if (handler != null)
            {
                CustomMessageReceivedEventArgs cmrArgs = new CustomMessageReceivedEventArgs(customMsg, args.Closure);
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

                customMsg.Name = Convert.ToString(msg.GetField("Name").Value);
                customMsg.Age = Convert.ToInt32(msg.GetField("Age").Value);
                customMsg.Department = Convert.ToString(msg.GetField("Department").Value);
                customMsg.Address = Convert.ToString(msg.GetField("Address").Value);
            }

            return customMsg;
        }
    }
}
