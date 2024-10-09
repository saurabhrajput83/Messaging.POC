using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.BLL
{
    public class ServiceBusQueueSender : IAsyncDisposable
    {

        ServiceBusClient _client;
        ServiceBusSender _sender;
        private string _namespace_connection_string;
        private string _queue_name;

        public ServiceBusQueueSender(string namespace_connection_string, string queue_name)
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
            _sender = _client.CreateSender(_queue_name);
        }

        public async Task Send(string message)
        {
            using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();


            // try adding a message to the batch
            if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {message}")))
            {
                // if it is too large for the batch
                throw new Exception($"The message {message} is too large to fit in the batch.");
            }


            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await _sender.SendMessagesAsync(messageBatch);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async ValueTask DisposeAsync()
        {

            if (_sender != null)
                await _sender.DisposeAsync();
            if (_sender != null)
                await _client.DisposeAsync();
        }
    }
}
