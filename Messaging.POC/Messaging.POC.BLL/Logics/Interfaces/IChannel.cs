using Messaging.POC.BLL.DTOs;
using Messaging.POC.BLL.Logics.Service_Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics.Interfaces
{
    public interface IChannel
    {
        void SendMessage(CustomMessage customMsg);
        CustomMessage SendRequestMessage(CustomMessage customMsg);
        void SendReplyMessage(CustomMessage customReplyMsg, CustomMessage customMsg);

        bool Subscribe(string subject, CustomMessageReceivedEventHandler messageHandler);

        void Dispatch();

    }
}
