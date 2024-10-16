﻿using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Implementations.TIBCO_RV
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

            customMsg.Name = (string)msg.GetField("Name").Value;
            customMsg.Age = (int)msg.GetField("Age").Value;
            customMsg.Department = (string)msg.GetField("Department").Value;
            customMsg.Address = (string)msg.GetField("Address").Value;

            return customMsg;
        }
    }
}
