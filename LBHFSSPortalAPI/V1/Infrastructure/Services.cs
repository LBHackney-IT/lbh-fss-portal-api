using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class Services
    {
        public Services()
        {
            ServiceRevisions = new HashSet<ServiceRevisions>();
        }

        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public int RevisionId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual ServiceRevisions Revision { get; set; }
        public virtual ICollection<ServiceRevisions> ServiceRevisions { get; set; }
    }
}
