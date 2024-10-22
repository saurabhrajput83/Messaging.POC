using System.Configuration;

namespace Messaging.POC.BLL
{
    public class Configs
    {
        public static string SERVICE = ConfigurationManager.AppSettings["service"];
        public static string NETWORK = ConfigurationManager.AppSettings["network"];
        public static string DAEMON = ConfigurationManager.AppSettings["daemon"];

        public static string NAMESPACE_CONNECTION_STRING = ConfigurationManager.AppSettings["namespace_connection_string"];
        public static string TOPIC_OR_QUEUE_NAME = ConfigurationManager.AppSettings["topic_or_queue_name"];
        public static string SUBSCRIPTION_NAME = ConfigurationManager.AppSettings["subscription_name"];

        public static string SENDMESSAGESUBJECT = ConfigurationManager.AppSettings["sendMessageSubject"];
        public static string SENDREQUESTMESSAGESUBJECT = ConfigurationManager.AppSettings["sendRequestMessageSubject"];
        public static string SENDREPLYMESSAGESUBJECT = ConfigurationManager.AppSettings["sendReplyMessageSubject"];

    }
}