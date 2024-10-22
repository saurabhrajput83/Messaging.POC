using System.Threading.Tasks;

namespace ServiceBus.Framework.Interfaces
{
    public interface IServiceBusSender
    {
        Task Send(ServiceBusActionTypes serviceBusActionType,  string subject, string body);
    }
}
