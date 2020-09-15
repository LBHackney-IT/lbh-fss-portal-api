using System;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class SynonymGroup
    {
        public SynonymGroup()
        {
            SynonymWords = new HashSet<SynonymWord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<SynonymWord> SynonymWords { get; set; }
    }
}
