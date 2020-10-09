using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class AdminCreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public List<string> Roles { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("organisation_id")]
        public int? OrganisationId { get; set; }
    }
}
