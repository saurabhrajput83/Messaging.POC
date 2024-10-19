using Messaging.POC.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.Logics
{
    public class CustomMessageReceivedEventArgs : EventArgs
    {
        internal CustomMessage message;

        //internal object closure;

        public CustomMessage Message => message;

        //public object Closure => closure;

        internal CustomMessageReceivedEventArgs(CustomMessage message)
        {
            this.message = message;
            //this.closure = closure;
        }
    }
}
