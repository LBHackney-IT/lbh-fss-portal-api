using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserUpdateRequest
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Roles { get; set; }
        public string Password { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("organisation_id")]
        public int? OrganisationId { get; set; }
    }
}
