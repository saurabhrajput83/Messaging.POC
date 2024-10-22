using Messaging.POC.BLL.DTOs;

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
