using Messaging.POC.BLL.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL
{
    public class ConsoleHelper
    {

        public static void StartApp(AppTypes appType)
        {
            string appTypeStr = appType.ToString();

            Console.WriteLine($"\n{appTypeStr} started running..");
        }

        public static void StartApp(MessagingTypes messagingType, AppTypes appType)
        {
            string messagingTypeStr = messagingType.ToString();
            string appTypeStr = appType.ToString();


            Console.WriteLine($"\n{messagingTypeStr} {appTypeStr} started running..");
        }

        public static void StopApp(AppTypes appType)
        {
        }



        public static void ExitApp()
        {
            Console.WriteLine("\nPress any key to exit..");
            Console.ReadLine();
            Console.WriteLine("Exiting..");
        }

        public static void DisplayActions()
        {
            Console.WriteLine("Press 1 to test Send mtd, 2 to test SendRequest mtd, 3 to test SendReply mtd, or x to exit");
        }

        public static void StartAction(MessagingTypes messagingType, ActionTypes actionType)
        {
            string messagingTypeStr = messagingType.ToString();
            string actionTypeStr = actionType.ToString();
            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"{messagingTypeStr} {actionTypeStr} Started..");
            Console.WriteLine($"*********************************************************");
        }

        public static void CompleteAction(MessagingTypes messagingType, ActionTypes actionType)
        {
            string messagingTypeStr = messagingType.ToString();
            string actionTypeStr = actionType.ToString();
            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"{messagingTypeStr} {actionTypeStr} Completed..");
            Console.WriteLine($"*********************************************************");
        }

        public static void StartListener(MessagingTypes messagingType, ListenerTypes listenerType, string subject)
        {
            string messagingTypeStr = messagingType.ToString();
            string listenerTypeStr = listenerType.ToString();
            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"{messagingTypeStr} Custom {listenerTypeStr} for {subject} Started..");
            Console.WriteLine($"*********************************************************");

        }
        public static void CompleteListener(MessagingTypes messagingType, ListenerTypes listenerType, string subject)
        {
            string messagingTypeStr = messagingType.ToString();
            string listenerTypeStr = listenerType.ToString();
            Console.WriteLine("\n");
            Console.WriteLine($"*********************************************************");
            Console.WriteLine($"{messagingTypeStr} Custom {listenerTypeStr} for {subject} Completed..");
            Console.WriteLine($"*********************************************************");
        }

        public static void DisplayMessagingTypes()
        {
            Console.WriteLine("\nPress a to test TIBCO RV, b to test Service Bus, or x to exit..");
        }

        public static void DisplayCustomRequestMessage(CustomMessage requestMsg)
        {
            string requestMsgStr = JsonConvert.SerializeObject(requestMsg);

            Console.WriteLine($"\nCustom Request Message send..::");
            Console.WriteLine($"{requestMsgStr}");

        }

        public static void DisplayListenerCustomRequestMessage(CustomMessage requestMsg)
        {
            string requestMsgStr = JsonConvert.SerializeObject(requestMsg);

            Console.WriteLine($"\nCustom Request Message Received..::");
            Console.WriteLine($"{requestMsgStr}");

        }

        public static void DisplayCustomResponseMessage(CustomMessage responseMsg)
        {
            string responseMsgStr = JsonConvert.SerializeObject(responseMsg);

            Console.WriteLine($"\nCustom Response Message Received..::");
            Console.WriteLine($"{responseMsgStr}");

        }

        public static void DisplayTimeDifference(DateTime dtStartTime)
        {
            DateTime dtEndTime = DateTime.Now;

            double milliseconds = (dtEndTime - dtStartTime).TotalMilliseconds;

            Console.WriteLine($"\nTime taken:: {milliseconds} ms");
        }

        public static void DisplayListenerStarted(string listener)
        {
            Console.WriteLine($"\nStarted listening for {listener}..");
        }
    }
}
