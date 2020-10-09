using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class File
    {
        public File()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
