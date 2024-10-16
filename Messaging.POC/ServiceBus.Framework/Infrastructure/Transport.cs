﻿using Newtonsoft.Json;
using ServiceBus.Framework.Implementations;
using ServiceBus.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public abstract class Transport
    {

        private ServiceBusQueueManager _serviceBusQueueManager = null;

        protected Transport(string namespace_connection_string, string queue_name)
        {
            _serviceBusQueueManager = new ServiceBusQueueManager(namespace_connection_string, queue_name);
        }

        public virtual void Send(Message message)
        {
            string messageStr = JsonConvert.SerializeObject(message);
            Task.Run(async () => await _serviceBusQueueManager.SendMessage(messageStr)).GetAwaiter().GetResult();
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
