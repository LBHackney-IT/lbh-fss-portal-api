using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class TaxonomyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Vocabulary { get; set; }
        public int Weight { get; set; }

        [JsonPropertyName("service_description")]
        public string ServiceDescription { get; set; }
    }
}
