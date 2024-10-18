using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;

namespace TIBCO.RV.Publisher
{
    public class Publisher
    {
        public void Run()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();
                var subject = "ME.TEST";
                string service = ConfigurationManager.AppSettings["service"];
                string network = ConfigurationManager.AppSettings["network"];
                string daemon = ConfigurationManager.AppSettings["daemon"];


                var transport = new NetTransport(service, network, daemon);
                Console.WriteLine("Server running..");
                Console.WriteLine("Press x to exit or any other key to send message");
                
                while (true)
                {
                    var m = new Message();
                    m.SendSubject = subject;
                    m.AddField("Test", "TestValue");
                    transport.Send(m);
                    var line = Console.ReadLine();
                    if (line.ToUpper().Equals("X"))
                        break;
                }

                TIBCO.Rendezvous.Environment.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
