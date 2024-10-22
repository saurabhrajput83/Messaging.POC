using System.ComponentModel;

namespace ServiceBus.Framework
{
    public enum ServiceBusActionTypes
    {
        Send = 0,
        SendRequest = 1,
        SendReply
    }

    public enum ServiceBusAppTypes
    {
        [Description("Publisher")]
        Publisher = 0,
        [Description("Receiver")]
        Receiver = 1
    }
}
