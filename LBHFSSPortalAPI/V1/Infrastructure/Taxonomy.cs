using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Taxonomy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Vocabulary { get; set; }
        public int Weight { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}