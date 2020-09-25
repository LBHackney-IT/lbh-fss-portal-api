namespace LBHFSSPortalAPI.V1.Domain
{
    public class ServiceTaxonomyDomain
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public int? TaxonomyId { get; set; }
        public string Description { get; set; }

        public ServiceDomain Service { get; set; }
        public TaxonomyDomain Taxonomy { get; set; }
    }
}
