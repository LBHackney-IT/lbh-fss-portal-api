using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class UserRole
    {
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual Role Role { get; set; }
    }
}
