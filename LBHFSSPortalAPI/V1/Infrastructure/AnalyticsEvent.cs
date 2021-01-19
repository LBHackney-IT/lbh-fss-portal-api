using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class AnalyticsEvent
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual Service Service { get; set; }
    }
}
