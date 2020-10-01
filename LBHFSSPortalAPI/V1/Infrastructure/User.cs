using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class User
    {
        public User()
        {
            Organizations = new HashSet<Organization>();
            Sessions = new HashSet<Session>();
            UserOrganizations = new HashSet<UserOrganization>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string SubId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<UserOrganization> UserOrganizations { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
