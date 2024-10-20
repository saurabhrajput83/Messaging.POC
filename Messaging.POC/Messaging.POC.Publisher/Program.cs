using Messaging.POC.BLL;
using Messaging.POC.BLL.Services;
using Messaging.POC.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Messaging.POC.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            IManager manager = new Manager();

            ConsoleHelper.StartApp(AppTypes.Publisher);

            while (flag)
            {
                ConsoleHelper.DisplayMessagingTypes();

                var line = Console.ReadLine().ToUpper();

                switch (line)
                {
                    case "A":
                        manager.GetPublisher(MessagingTypes.TIBCO_RV).Run();
                        break;
                    case "B":
                        manager.GetPublisher(MessagingTypes.Service_Bus).Run();
                        break;
                    case "X":
                        flag = false;
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
