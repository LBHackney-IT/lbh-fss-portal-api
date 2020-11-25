using AutoFixture;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.Tests.V1.E2ETests
{
    public static class E2ETestHelpers
    {
        public static void ClearTable(DatabaseContext context)
        {
            var addedOrganisations = context.Organisations;
            context.Organisations.RemoveRange(addedOrganisations);
            var addedSessions = context.Sessions;
            context.Sessions.RemoveRange(addedSessions);
            context.SaveChanges();
        }
    }
}
