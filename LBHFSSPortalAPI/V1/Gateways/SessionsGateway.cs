using System.Linq;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class SessionsGateway : ISessionsGateway
    {
        private readonly DatabaseContext _databaseContext;

        public SessionsGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public Session AddSession(Session session)
        {
            var savedSession = _databaseContext.Sessions.Add(session);
            _databaseContext.SaveChanges();
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

        public Session GetSessionByToken(string token)
        {
            var session = _databaseContext.Sessions
                .Include(s => s.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(s => s.Payload == token);
            return session;
        }

        public void RemoveSessions(int userId)
        {
            var sessions = _databaseContext.Sessions.Where(s => s.UserId == userId);
            RemoveAllSessions(sessions);
        }

        private void RemoveAllSessions(IQueryable<Session> sessions)
        {
            if (sessions.Any())
            {
                _databaseContext.Sessions.RemoveRange(sessions);
                _databaseContext.SaveChanges();
            }
        }
    }
}

