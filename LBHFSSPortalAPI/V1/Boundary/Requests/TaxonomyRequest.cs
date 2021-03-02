using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class TaxonomyRequest
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        [JsonPropertyName("vocabulary_id")]
        public int VocabularyId { get; set; }
        public int Weight { get; set; }
    }
}
