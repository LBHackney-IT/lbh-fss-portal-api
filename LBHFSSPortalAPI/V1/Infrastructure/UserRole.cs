using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class UserRole
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
