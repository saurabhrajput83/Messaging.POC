using Messaging.POC.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Interfaces
{
    public interface IChannel
    {
        void SendMessage(CustomMessage customMsg);
        CustomMessage SendRequestMessage(CustomMessage customMsg);
        void SendReplyMessage(CustomMessage customReplyMsg, CustomMessage customMsg);

    }
}
