namespace ServiceBus.Framework.Infrastructure
{
    public class Listener
    {
        private Queue _queue;
        private MessageReceivedEventHandler _messageReceivedEventHandler;
        private Transport _transport;
        private string _subject;
        private object _closure;

        public Listener(Queue queue, Transport transport, string subject, object closure)
        {
            _queue = queue;
            _transport = transport;
            _subject = subject;
            _closure = closure;
        }

        public Listener(Queue queue, MessageReceivedEventHandler messageReceivedEventHandler, Transport transport, string subject, object closure)
        {
            _queue = queue;
            _messageReceivedEventHandler = messageReceivedEventHandler;
            _transport = transport;
            _subject = subject;
            _closure = closure;

        }

        ~Listener()
        {
        }

        public string Subject => _subject;
        public Transport Transport => _transport;
        public Queue Queue => _queue;
        public object Closure => _closure;
        public MessageReceivedEventHandler OnMessageReceivedEventHandler => _messageReceivedEventHandler;

        public virtual void Destroy()
        {

        }

    }
}
