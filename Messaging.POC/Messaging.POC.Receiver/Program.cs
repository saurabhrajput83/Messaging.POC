using Messaging.POC.BLL;
using Messaging.POC.BLL.Services;
using Messaging.POC.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Messaging.POC.Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            IManager manager = new Manager();

            ConsoleHelper.StartApp(AppTypes.Receiver);

            while (flag)
            {
                ConsoleHelper.DisplayMessagingTypes();

                var line = Console.ReadLine().ToUpper();

                switch (line)
                {
                    case "A":
                        manager.GetReceiver(MessagingTypes.TIBCO_RV).Run();
                        break;
                    case "B":
                        manager.GetReceiver(MessagingTypes.Service_Bus).Run();
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
