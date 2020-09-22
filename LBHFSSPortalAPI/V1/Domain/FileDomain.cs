using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class FileDomain
    {
        public FileDomain()
        {
            Services = new HashSet<ServiceDomain>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<ServiceDomain> Services { get; set; }
    }
}
