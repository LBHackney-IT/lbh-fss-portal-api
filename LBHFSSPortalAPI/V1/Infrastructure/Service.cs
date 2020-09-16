using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class Service
    {
        public Service()
        {
            ServiceLocations = new HashSet<ServiceLocation>();
            ServiceTaxonomies = new HashSet<ServiceTaxonomy>();
        }

        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Keywords { get; set; }
        public string ReferralLink { get; set; }
        public string ReferralEmail { get; set; }
        public int? ImageId { get; set; }

        public virtual File Image { get; set; }
        public virtual ICollection<ServiceLocation> ServiceLocations { get; set; }
        public virtual ICollection<ServiceTaxonomy> ServiceTaxonomies { get; set; }
    }
}
