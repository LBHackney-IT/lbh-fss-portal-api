using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class UserOrganizations
    {
        public int? Id { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users IdNavigation { get; set; }
        public virtual Organizations Organization { get; set; }
    }
}
