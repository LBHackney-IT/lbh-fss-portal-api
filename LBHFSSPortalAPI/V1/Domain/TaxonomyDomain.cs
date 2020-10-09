using System;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class TaxonomyDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Vocabulary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
    }
}
