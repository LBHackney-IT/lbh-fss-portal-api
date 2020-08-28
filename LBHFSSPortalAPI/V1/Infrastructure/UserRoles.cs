using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class UserRoles
    {
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users IdNavigation { get; set; }
        public virtual Roles Role { get; set; }
    }
}
