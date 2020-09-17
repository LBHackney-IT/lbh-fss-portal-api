using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Taxonomy
    {
        public Taxonomy()
        {
            ServiceTaxonomies = new HashSet<ServiceTaxonomy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Vocabulary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ServiceTaxonomy> ServiceTaxonomies { get; set; }
    }
}
