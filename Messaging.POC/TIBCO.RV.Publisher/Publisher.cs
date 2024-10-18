using Messaging.POC.BLL;
using Messaging.POC.BLL.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using TIBCO.Rendezvous;

namespace TIBCO.RV.Publisher
{
    public class Publisher
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
                Console.WriteLine("Publisher started running..");
                bool flag = true;

                while (flag)
                {
                    Console.WriteLine("Press 1 for Send, and x to exit");
                    var line = Console.ReadLine().ToUpper();


                    switch (line)
                    {

                        case "X":
                            flag = false;
                            break;
                        case "1":
                            SendMessage();
                            break;
                        default:
                            break;
                    }

                }

                TIBCO.Rendezvous.Environment.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMessage()
        {
            CustomMessage customMsg = new CustomMessage();

            customMsg.SendSubject = _sendMessageSubject;
            customMsg.Name = "Saurabh";
            customMsg.Age = 35;
            customMsg.Department = "I.T.";
            customMsg.Address = "Whitefield";

            _channel.SendMessage(customMsg);
        }

    }
}
