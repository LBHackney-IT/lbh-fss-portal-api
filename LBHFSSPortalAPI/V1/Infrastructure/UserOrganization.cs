using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class UserOrganization
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
    }
}
