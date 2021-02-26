using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class SynonymGroupRequest
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
