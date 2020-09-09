using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class Service
    {
        public Service()
        {
            ServiceRevisions = new HashSet<ServiceRevision>();
        }

        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public int RevisionId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ServiceRevision Revision { get; set; }
        public virtual ICollection<ServiceRevision> ServiceRevisions { get; set; }
    }
}
