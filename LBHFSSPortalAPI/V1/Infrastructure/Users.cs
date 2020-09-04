using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Users
    {
        public Users()
        {
            ServiceRevisionsAuthor = new HashSet<ServiceRevisions>();
            ServiceRevisionsReviewerU = new HashSet<ServiceRevisions>();
            Sessions = new HashSet<Sessions>();
            UserOrganizations = new HashSet<UserOrganizations>();
        }

        public int Id { get; set; }
        public string SubId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }

        public virtual ICollection<ServiceRevisions> ServiceRevisionsAuthor { get; set; }
        public virtual ICollection<ServiceRevisions> ServiceRevisionsReviewerU { get; set; }
        public virtual ICollection<Sessions> Sessions { get; set; }
        public virtual ICollection<UserOrganizations> UserOrganizations { get; set; }
    }
}
