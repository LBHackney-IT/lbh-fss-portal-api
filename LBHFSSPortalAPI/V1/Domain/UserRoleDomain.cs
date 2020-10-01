using System;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class UserRoleDomain
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }

        public RoleDomain Role { get; set; }
        public UserDomain User { get; set; }
    }
}
