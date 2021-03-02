using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class SynonymWordResponse
    {
        public int Id { get; set; }
        [JsonPropertyName("word")]
        public string Word { get; set; }
        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

    }
}
