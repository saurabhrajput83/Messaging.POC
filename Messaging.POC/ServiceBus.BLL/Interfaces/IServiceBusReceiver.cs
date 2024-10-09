using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.BLL.Interfaces
{
    public interface IServiceBusReceiver
    {
        void Init();
        Task Start(Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errorHandler);

        Task Stop();
    }
}
