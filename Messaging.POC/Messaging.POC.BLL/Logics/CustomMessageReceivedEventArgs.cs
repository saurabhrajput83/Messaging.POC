using Messaging.POC.BLL.DTOs;
using System;

namespace Messaging.POC.BLL.Logics
{
    public class CustomMessageReceivedEventArgs : EventArgs
    {
        internal CustomMessage _message;

        internal object _closure;

        public CustomMessage Message => _message;

        public object Closure => _closure;

        internal CustomMessageReceivedEventArgs(CustomMessage message, object closure)
        {
            _message = message;
            _closure = closure;
        }
    }
}
