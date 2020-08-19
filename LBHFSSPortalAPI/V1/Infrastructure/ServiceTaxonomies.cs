using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class ServiceTaxonomies
    {
        public int? RevisionId { get; set; }
        public int? TaxonomyId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ServiceRevisions Revision { get; set; }
        public virtual Taxonomies Taxonomy { get; set; }
    }
}
