using Messaging.POC.BLL.DTOs;
using Newtonsoft.Json;
using System;
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
            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"Frwk OnMessageReceivedEventHandler Started..::");
            Console.WriteLine($"*********************************************************");

            string msgStr = JsonConvert.SerializeObject(msg);
            string customMsgStr = JsonConvert.SerializeObject(customMsg);

            Console.WriteLine($"\nFrwk Message Received..::");
            Console.WriteLine($"{msgStr}");

            Console.WriteLine($"\nCustom Message..::");
            Console.WriteLine($"{customMsgStr}");

        }

        public static void FrwkOnMessageReceivedEventHandlerCompleted()
        {

            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"Frwk OnMessageReceivedEventHandler Completed..::");
            Console.WriteLine($"*********************************************************");

        }

    }
}
