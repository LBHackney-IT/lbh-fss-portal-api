using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class Sessions
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Payload { get; set; }
        public DateTime? LastAccessAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
