using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL
{
    public enum MessagingType
    {
        [Description("TIBCO RV")]
        TIBCO_RV = 0,
        [Description("Service Bus")]
        Service_Bus = 1
    }

    
}
