using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Interfaces
{
    public interface IServiceBusReceiver
    {
        Task Start(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler);

        Task Stop();
    }
}
