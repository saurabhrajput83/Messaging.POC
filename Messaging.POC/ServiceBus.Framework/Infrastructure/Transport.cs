using ServiceBus.Framework.Implementations;
using ServiceBus.Framework.Utilities;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Infrastructure
{
    public abstract class Transport
    {

        private ServiceBusSenderManager _serviceBusSenderManager = null;

        protected Transport(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
            _serviceBusSenderManager = new ServiceBusSenderManager(namespace_connection_string, topic_or_queue_name, subscription_name);

        }

        public virtual void StartListening()
        {
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

        public string CreateInbox(Message reply, Message request)
        {
            string inboxName = Helper.GetInboxName();
            return inboxName;
        }

    }
}
