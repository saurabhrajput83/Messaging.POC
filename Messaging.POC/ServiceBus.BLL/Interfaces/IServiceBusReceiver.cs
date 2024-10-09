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
        Task Start();

        Task Stop();
    }
}
