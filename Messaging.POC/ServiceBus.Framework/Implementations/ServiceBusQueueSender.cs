﻿using Azure.Messaging.ServiceBus;
using ServiceBus.Framework.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Implementations
{
    public class ServiceBusQueueSender : IServiceBusSender, IAsyncDisposable
    {

        private ServiceBusClient _client;
        private ServiceBusSender _sender;

        public ServiceBusQueueSender(string namespace_connection_string, string topic_or_queue_name)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            _client = new ServiceBusClient(namespace_connection_string, clientOptions);
            _sender = _client.CreateSender(topic_or_queue_name);

        }


        public async Task Send(ServiceBusActionTypes serviceBusActionType, string subject, string body)
        {
            ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();
            string actionType = serviceBusActionType.ToString();

            // try adding a message to the batch

            ServiceBusMessage sbm = new ServiceBusMessage(body);
            sbm.Subject = subject;
            sbm.ApplicationProperties["ActionType"] = actionType;

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
