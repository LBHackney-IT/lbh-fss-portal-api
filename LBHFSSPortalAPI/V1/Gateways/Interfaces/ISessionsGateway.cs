using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface ISessionsGateway
    {
        Session AddSession(Session session);
        void RemoveSessions(string accessToken);
        void RemoveSessions(int userId);
        Session GetSessionByToken(string token);
    }
}
