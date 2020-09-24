using System;

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
        public OrganizationDomain Organisation { get; set; }
    }
}
