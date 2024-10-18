using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public abstract class Transport
    {
        public virtual void Send(Message message)
        {

        }

        public virtual Message SendRequest(Message requestMessage, double timeout)
        {
            return null;
        }

        public virtual void SendReply(Message reply, Message request)
        {

        }

    }
}
