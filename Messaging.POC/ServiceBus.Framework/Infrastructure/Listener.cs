using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Listener
    {
        public Listener(Queue queue, Transport transport, string subject, object closure)
        {
        }

        public event MessageReceivedEventHandler MessageReceived;
    }
}
