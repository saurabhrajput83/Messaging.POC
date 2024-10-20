using ServiceBus.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics.Service_Bus
{
    public class Helper
    {
        public static MessagingTypes GetMessagingType()
        {
            return MessagingTypes.Service_Bus;
        }

        public static ServiceBusTypes GetDefaultServiceBusType()
        {
            return ServiceBusTypes.Topic;
        }
    }
}
