using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public delegate void MessageReceivedEventHandler(object listener, MessageReceivedEventArgs messageReceivedEventArgs);
}
