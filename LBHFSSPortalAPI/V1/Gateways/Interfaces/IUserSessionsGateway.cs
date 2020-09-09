using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface ISessionsGateway
    {
        Session AddSession(Session session);
        void RemoveSessions(string accessToken);
        void RemoveSessions(int userId);
    }
}
