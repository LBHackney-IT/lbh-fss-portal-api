using System;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class UserOrganizationDomain
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual OrganizationDomain Organization { get; set; }
        public virtual UserDomain User { get; set; }
    }
}
