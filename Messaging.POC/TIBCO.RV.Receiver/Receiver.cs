//using Newtonsoft.Json;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;

namespace TIBCO.RV.Receiver
{
    public class Receiver
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


                Transport transport = new NetTransport(service, network, daemon);
                Listener listener = new Listener(
                       TIBCO.Rendezvous.Queue.Default,
                        transport,
                        subject,
                        new object()
                        );
                listener.MessageReceived += new MessageReceivedEventHandler(listener_MessageReceived);

                var dispacher = new Dispatcher(listener.Queue);
                dispacher.Join();

                Console.WriteLine("Listener running..");
                Console.ReadKey();

                TIBCO.Rendezvous.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadKey();

            }
            catch (Exception)
            {
                throw;
            }
        }

        void listener_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            string msg = JsonConvert.SerializeObject(args.Message);
            Console.WriteLine("Message Received..");
            Console.WriteLine(msg);

        }
    }
}
