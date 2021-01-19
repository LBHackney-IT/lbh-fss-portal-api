using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class AnalyticsResponse
    {
        public int Id { get; set; }
        [JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
