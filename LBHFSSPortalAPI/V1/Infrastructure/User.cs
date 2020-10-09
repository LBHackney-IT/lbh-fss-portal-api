using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class User
    {
        public User()
        {
            Organisations = new HashSet<Organisation>();
            Sessions = new HashSet<Session>();
            UserOrganisations = new HashSet<UserOrganisation>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string SubId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Organisation> Organisations { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<UserOrganisation> UserOrganisations { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
