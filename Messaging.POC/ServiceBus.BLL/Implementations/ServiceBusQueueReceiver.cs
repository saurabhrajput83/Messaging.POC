using Azure.Core;
using Azure.Messaging.ServiceBus;
using ServiceBus.BLL.Interfaces;
using System.Diagnostics;

namespace ServiceBus.BLL.Implementations
{
    public class ServiceBusQueueReceiver : IServiceBusReceiver, IAsyncDisposable
    {

        ServiceBusClient _client;
        ServiceBusProcessor _processor;
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

            _client = new ServiceBusClient(_namespace_connection_string, clientOptions);
            _processor = _client.CreateProcessor(_queue_name, new ServiceBusProcessorOptions());
        }

        public async Task Start()
        {
            try
            {

                _processor.ProcessMessageAsync += MessageHandler;
                _processor.ProcessErrorAsync += ErrorHandler;

                await _processor.StartProcessingAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Stop()
        {
            await _processor.StopProcessingAsync();
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

        public async ValueTask DisposeAsync()
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            if (_processor != null)
                await _processor.DisposeAsync();
            if (_client != null)
                await _client.DisposeAsync();
        }
    }
}