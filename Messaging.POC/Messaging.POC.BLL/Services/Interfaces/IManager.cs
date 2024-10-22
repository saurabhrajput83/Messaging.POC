using Messaging.POC.BLL.Logics.Interfaces;

namespace Messaging.POC.BLL.Services.Interfaces
{
    public interface IManager
    {
        IPublisher GetPublisher(MessagingTypes messagingType);
        IReceiver GetReceiver(MessagingTypes messagingType);

    }
}
