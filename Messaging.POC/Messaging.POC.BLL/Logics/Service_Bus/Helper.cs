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
        public static ServiceBusType GetDefaultServiceBusType()
        {
            return ServiceBusType.Topic;
        }
    }
}
