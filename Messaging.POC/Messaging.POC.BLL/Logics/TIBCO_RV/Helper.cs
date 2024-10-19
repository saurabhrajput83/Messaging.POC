using ServiceBus.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Helper
    {
        public static MessagingType GetMessagingType()
        {
            return MessagingType.TIBCO_RV;
        }

    }
}
