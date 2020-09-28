using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class TaxonomyRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vocabulary { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        public int Weight { get; set; }
        public string Description { get; set; }
    }
}
