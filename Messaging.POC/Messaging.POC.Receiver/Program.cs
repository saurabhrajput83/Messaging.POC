using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.POC.BLL.Implementations;
using Messaging.POC.BLL;

namespace Messaging.POC.Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            IManager manager = new Manager();

            while (flag)
            {
                Console.WriteLine("Press a to test TIBCO RV, b to test Service Bus, and x to exit..");
                var line = Console.ReadLine().ToUpper();


                switch (line)
                {

                    case "A":
                        manager.GetReceiver(MessagingType.TIBCO_RV).Run();
                        break;
                    case "B":
                        manager.GetReceiver(MessagingType.Service_Bus).Run();
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
