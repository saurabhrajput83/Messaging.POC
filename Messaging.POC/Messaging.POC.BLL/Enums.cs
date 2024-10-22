using System.ComponentModel;

namespace Messaging.POC.BLL
{
    public enum MessagingTypes
    {
        [Description("TIBCO RV")]
        TIBCO_RV = 0,
        [Description("Service Bus")]
        Service_Bus = 1
    }

    public enum AppTypes
    {
        [Description("Publisher")]
        Publisher = 0,
        [Description("Receiver")]
        Receiver = 1
    }

    public enum ActionTypes
    {
        Send = 0,
        SendRequest = 1,
        SendReply = 2
    }

    public enum ListenerTypes
    {
        SendListener = 0,
        SendRequestListener = 1,
        SendReplyListener = 2
    }


}
