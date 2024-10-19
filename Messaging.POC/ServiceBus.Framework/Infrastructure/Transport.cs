using Newtonsoft.Json;
using ServiceBus.Framework.Implementations;
using ServiceBus.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public abstract class Transport
    {

        private ServiceBusSenderManager _serviceBusSenderManager = null;

        protected Transport(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            _serviceBusSenderManager = new ServiceBusSenderManager(namespace_connection_string, topic_or_queue_name, subscription_name);
            Task.Run(async () => await _serviceBusSenderManager.StartListening()).GetAwaiter().GetResult();
        }

        public virtual void Send(Message message)
        {
            Task.Run(async () => await _serviceBusSenderManager.SendMessage(message)).GetAwaiter().GetResult();
        }

        public virtual Message SendRequest(Message requestMessage, double timeout)
        {
            return Task.Run(async () => await _serviceBusSenderManager.SendRequestMessage(requestMessage, timeout)).GetAwaiter().GetResult();
        }

        public virtual void SendReply(Message reply, Message request)
        {
            Task.Run(async () => await _serviceBusSenderManager.SendReplyMessage(reply, request)).GetAwaiter().GetResult();
        }

    }
}
