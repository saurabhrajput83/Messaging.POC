using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Implementations.TIBCO_RV
{
    public class Channel : IChannel
    {
        private Transport _transport;
        private double _timeout = 5000;

        public Channel(Transport transport)
        {
            _transport = transport;
        }

        public void SendMessage(CustomMessage customMsg)
        {
            Message msg = ConvertToTibcoMessage(customMsg);

            _transport.Send(msg);
        }

        public CustomMessage SendRequestMessage(CustomMessage customMsg)
        {
            Message msg = ConvertToTibcoMessage(customMsg);

            Message replyMsg = _transport.SendRequest(msg, _timeout);

            return ConvertToCustomMessage(replyMsg);
        }

        public void SendReplyMessage(CustomMessage customReplyMsg, CustomMessage customMsg)
        {
            Message replyMsg = ConvertToTibcoMessage(customReplyMsg);
            Message msg = ConvertToTibcoMessage(customMsg);


            _transport.SendReply(replyMsg, msg);

        }

        private Message ConvertToTibcoMessage(CustomMessage customMsg)
        {
            Message msg = new Message();

            msg.SendSubject = customMsg.SendSubject;
            msg.ReplySubject = customMsg.ReplySubject;

            msg.AddField(new MessageField("Name", customMsg.Name));
            msg.AddField(new MessageField("Age", customMsg.Age));
            msg.AddField(new MessageField("Department", customMsg.Department));
            msg.AddField(new MessageField("Address", customMsg.Address));

            return msg;
        }

        public CustomMessage ConvertToCustomMessage(Message msg)
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
