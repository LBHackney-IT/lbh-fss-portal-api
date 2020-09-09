using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class ServiceRevision
    {
        public ServiceRevision()
        {
            ServiceLocations = new HashSet<ServiceLocation>();
        }

        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Telephone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Status { get; set; }
        public int? AuthorId { get; set; }
        public int? ReviewerUid { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReviewerMessage { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User Author { get; set; }
        public virtual User ReviewerU { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<ServiceLocation> ServiceLocations { get; set; }
    }
}
