using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class User
    {
        public User()
        {
            ServiceRevisionsAuthor = new HashSet<ServiceRevision>();
            ServiceRevisionsReviewerU = new HashSet<ServiceRevision>();
            Sessions = new HashSet<Session>();
            UserOrganizations = new HashSet<UserOrganization>();
        }

        public int Id { get; set; }
        public string SubId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }

        public virtual ICollection<ServiceRevision> ServiceRevisionsAuthor { get; set; }
        public virtual ICollection<ServiceRevision> ServiceRevisionsReviewerU { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<UserOrganization> UserOrganizations { get; set; }
    }
}
