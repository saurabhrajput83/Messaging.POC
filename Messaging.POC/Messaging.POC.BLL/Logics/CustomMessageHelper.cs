﻿using Messaging.POC.BLL.DTOs;

namespace Messaging.POC.BLL.Logics
{
    public static class CustomMessageHelper
    {
        public static CustomMessage GetCustomMessage(string sendSubject, int counter)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.SendSubject = sendSubject;
            customMsg.Name = $"Saurabh {counter}";
            customMsg.Age = 39;
            customMsg.Department = $"I.T. {counter}";
            customMsg.Address = $"Whitefield {counter}";

            return customMsg;
        }

        public static CustomMessage GetCustomMessage(string name, int age, string department, string address, int counter)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.Name = $"{ name} {counter}";
            customMsg.Age = age;
            customMsg.Department = $"{ department} {counter}";
            customMsg.Address = $"{ address} {counter}";

            return customMsg;
        }
    }
}
