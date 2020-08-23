using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class SessionsGateway : ISessionsGateway
    {
        private readonly DatabaseContext _databaseContext;

        public SessionsGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public Sessions AddSession(Sessions session)
        {
            var savedSession = _databaseContext.Sessions.Add(session);
            int rows = _databaseContext.SaveChanges(); //log number of rows saved?
            return savedSession.Entity;
        }
    }
}
