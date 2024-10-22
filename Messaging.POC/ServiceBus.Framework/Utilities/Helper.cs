using System;

namespace ServiceBus.Framework.Utilities
{
    public class Helper
    {
        public static string GetInboxName()
        {
            string inbox = $"_INBOX.{Guid.NewGuid().ToString()}.{GetTime()}";
            return inbox;
        }

        public static long GetTime()
        {
            DateTime JanFirst1970 = new DateTime(1970, 1, 1);
            return (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
        }

        public static string CreateSubject(ServiceBusActionTypes actionType, string sendSubject)
        {
            return actionType.ToString() + "-" + sendSubject;
        }

        public static void ParseSubject(string subject, out ServiceBusActionTypes actionType, out string sendSubject)
        {
            int idx = subject.IndexOf("-");

            string actionTypeStr = subject.Substring(0, idx);
            sendSubject = subject.Substring(idx + 1);

            Enum.TryParse<ServiceBusActionTypes>(actionTypeStr, out actionType);

        }
    }
}
