using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class UserDomain
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string SubId { get; set; }

        public List<UserRoleDomain> UserRoles { get; set; }

        public List<OrganisationDomain> Organisations { get; set; }
    }
}
