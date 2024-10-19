using Azure.Core;
using Azure.Messaging.ServiceBus;
using ServiceBus.Framework.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusQueueReceiver : IServiceBusReceiver, IAsyncDisposable
    {

        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private string _namespace_connection_string = string.Empty;
        private string _queue_name = string.Empty;

        public ServiceBusQueueReceiver(string namespace_connection_string, string queue_name)
        {
            _namespace_connection_string = namespace_connection_string;
            _queue_name = queue_name;

            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            _client = new ServiceBusClient(_namespace_connection_string, clientOptions);
            _processor = _client.CreateProcessor(_queue_name, new ServiceBusProcessorOptions());

        }


        public async Task Start(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler)
        {
            try
            {

                _processor.ProcessMessageAsync += messageHandler;
                _processor.ProcessErrorAsync += errorHandler;

                await _processor.StartProcessingAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Stop()
        {
            try
            {
                await _processor.StopProcessingAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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