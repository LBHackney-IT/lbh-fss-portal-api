using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class SynonymGroups
    {
        public SynonymGroups()
        {
            SynonymWords = new HashSet<SynonymWords>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<SynonymWords> SynonymWords { get; set; }
    }
}
