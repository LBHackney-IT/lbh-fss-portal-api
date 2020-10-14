using System.Linq;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class SessionsGateway : BaseGateway, ISessionsGateway
    {
        public SessionsGateway(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Session AddSession(Session session)
        {
            var savedSession = Context.Sessions.Add(session);
            SaveChanges();
            return savedSession.Entity;
        }

        public Session GetSessionByToken(string token)
        {
            var session = Context.Sessions
                .Include(s => s.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(s => s.Payload == token);
            return session;
        }

        public void RemoveSessions(string accessToken)
        {
            var sessions = Context.Sessions.Where(s => s.Payload == accessToken);

            if (sessions.Any())
            {
                Context.Sessions.RemoveRange(sessions);
                SaveChanges();
            }
        }

        public void RemoveSessions(int userId)
        {
            var sessions = Context.Sessions.Where(s => s.UserId == userId);

            if (sessions.Any())
            {
                Context.Sessions.RemoveRange(sessions);
                SaveChanges();
            }
        }
    }
}

