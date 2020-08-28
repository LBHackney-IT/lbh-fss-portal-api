using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Taxonomies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Vocabulary { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
