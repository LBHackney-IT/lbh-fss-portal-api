using System;
using System.Collections.Generic;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class SynonymWordDomain
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int? GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual SynonymGroup Group { get; set; }
    }
}
