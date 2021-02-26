using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class SynonymGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
        public List<SynonymWordResponse> SynonymWords { get; set; }
    }
}
