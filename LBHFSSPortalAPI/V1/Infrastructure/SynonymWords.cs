using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class SynonymWords
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int? GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual SynonymGroups Group { get; set; }
    }
}
