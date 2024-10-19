using Messaging.POC.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static CustomMessage GetCustomMessage(string name, int age, string department, string address)
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.Name = name;
            customMsg.Age = age;
            customMsg.Department = department;
            customMsg.Address = address;

            return customMsg;
        }
    }
}
