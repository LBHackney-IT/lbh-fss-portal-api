
namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class ServiceTaxonomy
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? TaxonomyId { get; set; }
        public string Description { get; set; }

        public virtual Service Service { get; set; }
        public virtual Taxonomy Taxonomy { get; set; }
    }
}
