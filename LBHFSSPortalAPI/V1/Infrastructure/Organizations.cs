using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Organizations
    {
        public Organizations()
        {
            Services = new HashSet<Services>();
            UserOrganizations = new HashSet<UserOrganizations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Services> Services { get; set; }
        public virtual ICollection<UserOrganizations> UserOrganizations { get; set; }
    }
}
