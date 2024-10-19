using Messaging.POC.BLL.DTOs;
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
        private Dictionary<string, Frwk.Listener> _listener;

        public Channel(Frwk.Transport transport)
        {
            _transport = transport;
            _queue = Frwk.Queue.Default;
            _listener = new Dictionary<string, Frwk.Listener>();
        }

        public void SendMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = ConvertToTibcoMessage(customMsg);

            _transport.Send(msg);
        }

        public CustomMessage SendRequestMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = ConvertToTibcoMessage(customMsg);

            Frwk.Message replyMsg = _transport.SendRequest(msg, _timeout);

            return ConvertToCustomMessage(replyMsg);
        }

        public void SendReplyMessage(CustomMessage customReplyMsg, CustomMessage customMsg)
        {
            Frwk.Message replyMsg = ConvertToTibcoMessage(customReplyMsg);
            Frwk.Message msg = ConvertToTibcoMessage(customMsg);


            _transport.SendReply(replyMsg, msg);

        }

        public bool Subscribe(string subject, CustomMessageReceivedEventHandler messageHandler)
        {
            if (_listener.ContainsKey(subject))
                return false;

            Frwk.Listener listener = new Frwk.Listener(
                  _queue,
                    OnMessageReceivedEventHandler,
                    _transport,
                   subject,
                   messageHandler
                   );


            _listener.Add(subject, listener);
            return true;
        }

        public void Dispatch()
        {
            var dispacher = new Frwk.Dispatcher(_queue);
            dispacher.Join();
        }

        protected void OnMessageReceivedEventHandler(object listener, Frwk.MessageReceivedEventArgs args)
        {

            Frwk.Message msg = args.Message;
            CustomMessage customMsg = ConvertToCustomMessage(msg);

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);


            Console.WriteLine($"\nOnMessageReceivedEventHandler Received..");
            Console.WriteLine($"Message: {msgStr}");
            Console.WriteLine($"CustomMessage: {customMsgStr}");

            CustomMessageReceivedEventHandler handler = (CustomMessageReceivedEventHandler)args.Closure;

            if (handler != null)
            {
                CustomMessageReceivedEventArgs cmrArgs = new CustomMessageReceivedEventArgs(customMsg);
                handler(this, cmrArgs);
            }

        }

        private Frwk.Message ConvertToTibcoMessage(CustomMessage customMsg)
        {
            Frwk.Message msg = new Frwk.Message();

            msg.SendSubject = customMsg.SendSubject;
            msg.ReplySubject = customMsg.ReplySubject;

            msg.AddField(new Frwk.MessageField("Name", customMsg.Name));
            msg.AddField(new Frwk.MessageField("Age", customMsg.Age));
            msg.AddField(new Frwk.MessageField("Department", customMsg.Department));
            msg.AddField(new Frwk.MessageField("Address", customMsg.Address));

            return msg;
        }

        public CustomMessage ConvertToCustomMessage(Frwk.Message msg)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.SendSubject = msg.SendSubject;
            customMsg.ReplySubject = msg.ReplySubject;

            customMsg.Name = (string)msg.GetField("Name").Value;
            customMsg.Age = (int)msg.GetField("Age").Value;
            customMsg.Department = (string)msg.GetField("Department").Value;
            customMsg.Address = (string)msg.GetField("Address").Value;

            return customMsg;
        }
    }
}
