using System;
using System.Collections.Generic;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class SynonymGroupDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<SynonymWordDomain> SynonymWords { get; set; }

    }
}
