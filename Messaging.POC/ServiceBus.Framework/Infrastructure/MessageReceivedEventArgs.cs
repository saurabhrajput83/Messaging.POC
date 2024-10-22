using System;

namespace ServiceBus.Framework.Infrastructure
{
    public class MessageReceivedEventArgs : EventArgs
    {
        internal Message _message;

        internal object _closure;

        public Message Message => _message;

        public object Closure => _closure;

        internal MessageReceivedEventArgs(Message message, object closure)
        {
            this._message = message;
            this._closure = closure;
        }
    }
}
