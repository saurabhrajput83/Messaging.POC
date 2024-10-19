using Messaging.POC.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics
{
    public delegate void CustomMessageReceivedEventHandler(object source, CustomMessageReceivedEventArgs args);
}
