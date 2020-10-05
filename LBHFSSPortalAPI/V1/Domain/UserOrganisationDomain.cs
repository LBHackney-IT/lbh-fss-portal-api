using System;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class UserOrganisationDomain
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual OrganisationDomain Organisation { get; set; }
        public virtual UserDomain User { get; set; }
    }
}
