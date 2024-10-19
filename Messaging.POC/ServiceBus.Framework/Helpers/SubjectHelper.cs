using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Helpers
{
    public static class SubjectHelper
    {
        public static string CreateSubject(ActionTypes actionType, string sendSubject)
        {
            return actionType.ToString() + "-" + sendSubject;
        }

        public static void ParseSubject(string subject, out ActionTypes actionType, out string sendSubject)
        {
            int idx = subject.IndexOf("-");

            string actionTypeStr = subject.Substring(0, idx);
            sendSubject = subject.Substring(idx);

            Enum.TryParse<ActionTypes>(actionTypeStr, out actionType);

        }
    }
}
