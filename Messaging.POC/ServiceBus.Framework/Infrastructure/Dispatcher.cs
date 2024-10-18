using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public class Dispatcher
    {
        private Queue _queue = null;


        public Dispatcher(Queue queue)
        {
            _queue = queue;
        }

        public void Join()
        {
        }
    }
}
