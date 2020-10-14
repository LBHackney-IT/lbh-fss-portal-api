using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Enums;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        Task SendMessage(NotifyMessageTypes messageType, string[] addresses);
    }
}
