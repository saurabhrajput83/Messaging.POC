using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frwk = ServiceBus.Framework.Infrastructure;

namespace Messaging.POC.BLL.Logics.Service_Bus
{
    public class Channel : IChannel
    {
        private Frwk.Transport _transport;
        private double _timeout = 5000;

        public Channel(Frwk.Transport transport)
        {
            _transport = transport;
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

            customMsg.Name = Convert.ToString(msg.GetField("Name").Value);
            customMsg.Age = Convert.ToInt32(msg.GetField("Age").Value);
            customMsg.Department = Convert.ToString(msg.GetField("Department").Value);
            customMsg.Address = Convert.ToString(msg.GetField("Address").Value);

            return customMsg;
        }
    }
}
