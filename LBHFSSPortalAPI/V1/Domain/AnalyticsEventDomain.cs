using System;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class AnalyticsEventDomain
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual ServiceDomain Service { get; set; }
    }
}
