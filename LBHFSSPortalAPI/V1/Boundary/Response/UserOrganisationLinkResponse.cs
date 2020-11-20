using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class UserOrganisationLinkResponse
    {
        public int Id { get; set; }
        [JsonPropertyName("organisation_id")]
        public int OrganisationId { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
