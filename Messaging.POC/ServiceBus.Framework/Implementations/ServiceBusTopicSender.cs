using Azure.Messaging.ServiceBus;
using ServiceBus.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusTopicSender : IServiceBusSender, IAsyncDisposable
    {

        private ServiceBusClient _client;
        private ServiceBusSender _sender;


        public ServiceBusTopicSender(string namespace_connection_string, string topic_or_queue_name, string subscription_name)
        {
             
            _client = new ServiceBusClient(namespace_connection_string);
            _sender = _client.CreateSender(topic_or_queue_name);

        }


        public async Task Send(string subject, string body)
        {
            ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();


            // try adding a message to the batch

            ServiceBusMessage sbm = new ServiceBusMessage(body);
            sbm.Subject = subject;

            if (!messageBatch.TryAddMessage(sbm))
            {
                // if it is too large for the batch
                throw new Exception($"The message {body} is too large to fit in the batch.");
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
