using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class ServiceTaxonomy
    {
        public int? RevisionId { get; set; }
        public int? TaxonomyId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ServiceRevision Revision { get; set; }
        public virtual Taxonomy Taxonomy { get; set; }
    }
}
