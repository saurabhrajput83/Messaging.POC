using Azure.Core;
using Azure.Messaging.ServiceBus;
using System.Diagnostics;

namespace ServiceBus.BLL
{
    public class ServiceBusQueueReceiver
    {

        ServiceBusClient client;
        ServiceBusProcessor processor;
        private string _namespace_connection_string;
        private string _queue_name;

        public ServiceBusQueueReceiver(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

        }


        public void Init()
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            client = new ServiceBusClient(_namespace_connection_string, clientOptions);
            processor = client.CreateProcessor(_queue_name, new ServiceBusProcessorOptions());
        }

        public async Task Start()
        {
            try
            {

                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Stop()
        {
            await processor.StopProcessingAsync();
        }

        public async ValueTask DisposeAsync()
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            if (processor != null)
                await processor.DisposeAsync();
            if (client != null)
                await client.DisposeAsync();
        }


        // handle received messages
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}