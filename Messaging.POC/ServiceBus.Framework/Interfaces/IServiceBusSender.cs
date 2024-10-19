using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Framework.Interfaces
{
    public interface IServiceBusSender
    {
        Task Send(string header, string body);
    }
}
