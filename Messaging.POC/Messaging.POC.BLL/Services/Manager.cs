using Messaging.POC.BLL;
using Messaging.POC.BLL.Logics.Interfaces;
using Messaging.POC.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logics = Messaging.POC.BLL.Logics;

namespace Messaging.POC.BLL.Services
{
    public class Manager : IManager
    {
        public IPublisher GetPublisher(MessagingTypes messagingType)
        {
            IPublisher publisher = null;
            if (messagingType == MessagingTypes.TIBCO_RV)
                publisher = new Logics.TIBCO_RV.Publisher();
            else if (messagingType == MessagingTypes.Service_Bus)
                publisher = new Logics.Service_Bus.Publisher();

            return publisher;
        }

        public IReceiver GetReceiver(MessagingTypes messagingType)
        {
            IReceiver receiver = null;
            if (messagingType == MessagingTypes.TIBCO_RV)
                receiver = new Logics.TIBCO_RV.Receiver();
            else if (messagingType == MessagingTypes.Service_Bus)
                receiver = new Logics.Service_Bus.Receiver();

            return receiver;
        }
    }
}
