﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Utilities
{
    public class ConsoleHelper
    {
        public static void StartServiceBusSenderSendMessagesAsync(string actionType, string subject, string body)
        {
            Console.WriteLine("\n");
            Console.WriteLine("*********************************************************");
            Console.WriteLine("ServiceBusSender SendMessagesAsync started..");
            Console.WriteLine("Action Type::");
            Console.WriteLine(actionType);
            Console.WriteLine("Subject::");
            Console.WriteLine(subject);
            Console.WriteLine("Body::");
            Console.WriteLine(body);
            Console.WriteLine("*********************************************************");

        }


        public static void CompleteServiceBusSenderSendMessagesAsync()
        {
            Console.WriteLine("\n");
            Console.WriteLine("*********************************************************");
            Console.WriteLine($"ServiceBusSender SendMessagesAsync completed..");
            Console.WriteLine("*********************************************************");
        }

        public static void StartProcessMessageHandler(string appType, string actionType, string subject, string body)
        {
            Console.WriteLine("\n");
            Console.WriteLine("*********************************************************");
            Console.WriteLine($"Service Bus ProcessMessageHandler started for {appType}..");
            Console.WriteLine("Action Type::");
            Console.WriteLine(actionType);
            Console.WriteLine("Subject::");
            Console.WriteLine(subject);
            Console.WriteLine("Body::");
            Console.WriteLine(body);
            Console.WriteLine("*********************************************************");

        }


        public static void CompleteProcessMessageHandler(string appType)
        {
            Console.WriteLine("\n");
            Console.WriteLine("*********************************************************");
            Console.WriteLine($"Service Bus ProcessMessageHandler completed for {appType}..");
            Console.WriteLine("*********************************************************");
        }

        public static void ProcessErrorHandler(string appType, Exception ex)
        {
            Console.WriteLine("\n");
            Console.WriteLine("*********************************************************");
            Console.WriteLine($"Service Bus ProcessErrorHandler for {appType}..");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("*********************************************************");

        }

    }
}
