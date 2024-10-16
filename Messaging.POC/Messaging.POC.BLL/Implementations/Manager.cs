﻿using Messaging.POC.BLL;
using Messaging.POC.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Implementations
{
    public class Manager : IManager
    {
        public IPublisher GetPublisher(MessagingType messagingType)
        {
            IPublisher publisher = null;
            if (messagingType == MessagingType.TIBCO_RV)
                publisher = new TIBCO_RV.Publisher();
            else if (messagingType == MessagingType.Service_Bus)
                publisher = new Service_Bus.Publisher();

            return publisher;
        }

        public IReceiver GetReceiver(MessagingType messagingType)
        {
            IReceiver receiver = null;
            if (messagingType == MessagingType.TIBCO_RV)
                receiver = new TIBCO_RV.Receiver();
            else if (messagingType == MessagingType.Service_Bus)
                receiver = new Service_Bus.Receiver();

            return receiver;
        }
    }
}
