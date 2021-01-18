using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class TaxonomyRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("label")]
        public string Name { get; set; }
        [JsonPropertyName("vocabulary_id")]
        public int VocabularyId { get; set; }
        public int Weight { get; set; }
    }
}
