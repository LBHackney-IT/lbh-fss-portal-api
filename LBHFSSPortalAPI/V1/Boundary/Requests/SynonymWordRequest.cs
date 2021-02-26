using System;
using System.Text.Json.Serialization;


namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class SynonymWordRequest
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }
        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
