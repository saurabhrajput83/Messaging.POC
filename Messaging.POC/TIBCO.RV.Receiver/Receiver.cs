//using Newtonsoft.Json;
using Messaging.POC.BLL;
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
        private string _sendMessageSubject = "ME.TEST";
        private string _service = ConfigurationManager.AppSettings["service"];
        private string _network = ConfigurationManager.AppSettings["network"];
        private string _daemon = ConfigurationManager.AppSettings["daemon"];
        private Transport _transport;
        private Channel _channel;

        public void Run()
        {
            try
            {
                TIBCO.Rendezvous.Environment.Open();

                _transport = new NetTransport(_service, _network, _daemon);
                _channel = new Channel(_transport);

                Listener listener = new Listener(
                       TIBCO.Rendezvous.Queue.Default,
                        _transport,
                        _sendMessageSubject,
                        new object()
                        );
                listener.MessageReceived += new MessageReceivedEventHandler(Receiver_MessageReceived);

                var dispacher = new Dispatcher(listener.Queue);
                dispacher.Join();

                Console.WriteLine("Receiver started running..");
                Console.ReadLine();

                TIBCO.Rendezvous.Environment.Close();
                Console.WriteLine("Exiting..");
                Console.ReadKey();

            }
            catch (Exception)
            {
                throw;
            }
        }

        void Receiver_MessageReceived(object listener, MessageReceivedEventArgs args)
        {

            string msg = JsonConvert.SerializeObject(args.Message);
            Console.WriteLine("Message Received..");
            Console.WriteLine($"Message: {msg}");
            Console.WriteLine(msg);

        }
    }
}
