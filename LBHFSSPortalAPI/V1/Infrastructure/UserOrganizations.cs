using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class UserOrganizations
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual Users User { get; set; }
    }
}
