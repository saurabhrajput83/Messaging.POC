using Messaging.POC.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;

namespace Messaging.POC.BLL
{
    public class Channel
    {
        private Transport _transport;

        public Channel(Transport transport)
        {
            _transport = transport;
        }

        public void SendMessage(CustomMessage customMsg)
        {
            Message msg = ConvertToTibcoMessage(customMsg);

            _transport.Send(msg);
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

        private CustomMessage ConvertToCustomMessage(Message msg)
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
