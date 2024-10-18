using Messaging.POC.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Implementations
{
    public interface IManager
    {
        IPublisher GetPublisher(MessagingType messagingType);
        IReceiver GetReceiver(MessagingType messagingType);

    }
}
