namespace ServiceBus.Framework.Infrastructure
{
    public class Listener
    {

        private Queue _queue;
        private MessageReceivedEventHandler _messageReceivedEventHandler;
        private Transport _transport;
        private string _subject;
        private object _closure;

        public MessageReceivedEventHandler MessageReceivedEventHandler => _messageReceivedEventHandler;
        public object Closure => _closure;

        public Listener(Queue queue, MessageReceivedEventHandler messageReceivedEventHandler, Transport transport, string subject, object closure)
        {
            _queue = queue;
            _messageReceivedEventHandler = messageReceivedEventHandler;
            _transport = transport;
            _subject = subject;
            _closure = closure;

        }

    }
}
