using Messaging.POC.BLL.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frwk = TIBCO.Rendezvous;

namespace Messaging.POC.BLL.Logics.TIBCO_RV
{
    public class Helper
    {
        public static MessagingTypes GetMessagingType()
        {
            return MessagingTypes.TIBCO_RV;
        }

        public static void FrwkOnMessageReceivedEventHandlerStarted(Frwk.Message msg, CustomMessage customMsg)
        {

            Console.WriteLine($"\nFrwk OnMessageReceivedEventHandler Started..::");

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine($"\nFrwk Message Received..::");
            Console.WriteLine($"{msgStr}");

            Console.WriteLine($"\nCustom Message..::");
            Console.WriteLine($"{customMsgStr}");

        }

    }
}
