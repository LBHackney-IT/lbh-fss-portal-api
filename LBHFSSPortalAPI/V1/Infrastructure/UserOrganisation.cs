using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class UserOrganisation
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual User User { get; set; }
    }
}
