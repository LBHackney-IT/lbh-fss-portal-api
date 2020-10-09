using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public class SynonymWord
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int? GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual SynonymGroup Group { get; set; }
    }
}
