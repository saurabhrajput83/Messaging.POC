using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.BLL.Interfaces
{
    public interface IServiceBusSender
    {
        void Init();
        Task Send(string message);
    }
}
