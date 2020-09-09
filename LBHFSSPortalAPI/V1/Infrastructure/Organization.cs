using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Organization
    {
        public Organization()
        {
            Services = new HashSet<Service>();
            UserOrganizations = new HashSet<UserOrganization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<UserOrganization> UserOrganizations { get; set; }
    }
}
