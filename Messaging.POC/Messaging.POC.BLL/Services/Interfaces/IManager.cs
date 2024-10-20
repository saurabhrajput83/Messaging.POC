using Messaging.POC.BLL.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Services.Interfaces
{
    public interface IManager
    {
        IPublisher GetPublisher(MessagingTypes messagingType);
        IReceiver GetReceiver(MessagingTypes messagingType);

    }
}
