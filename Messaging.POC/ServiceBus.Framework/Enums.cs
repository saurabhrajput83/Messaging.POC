using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework
{
    public enum ActionTypes
    {
        Send = 0,
        SendRequest = 1,
        SendReply
    }

    public enum ServiceBusType
    {
        [Description("Topic")]
        Topic = 0,
        [Description("Queue")]
        Queue = 1
    }
}
