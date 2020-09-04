using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface ISessionsGateway
    {
        Sessions AddSession(Sessions session);
        void RemoveSessions(string accessToken);
        void RemoveSessions(int userId);
    }
}
