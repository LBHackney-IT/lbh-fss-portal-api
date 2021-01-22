using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class TaxonomyResponse
    {
        public int Id { get; set; }
        [JsonPropertyName("label")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Vocabulary { get; set; }
        [JsonPropertyName("vocabulary_id")]
        public int VocabularyId { get; set; }
        public int Weight { get; set; }
    }
}
