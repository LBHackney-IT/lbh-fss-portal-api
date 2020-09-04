using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

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

        public void RemoveSessions(string accessToken)
        {
            var user = _databaseContext.Users.FirstOrDefault(u => u.SubId == accessToken);

            if (user != null)
            {
                RemoveSessions(user.Id);
            }
        }

        public void RemoveSessions(int userId)
        {
            var sessions = _databaseContext.Sessions.Where(s => s.UserId == userId);
            RemoveAllSessions(sessions);
        }

        private void RemoveAllSessions(IQueryable<Sessions> sessions)
        {
            if (sessions.Any())
            {
                _databaseContext.Sessions.RemoveRange(sessions);
                _databaseContext.SaveChanges();
            }
        }
    }
}

