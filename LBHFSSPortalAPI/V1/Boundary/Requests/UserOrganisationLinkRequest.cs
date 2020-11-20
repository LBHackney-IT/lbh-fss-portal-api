using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserOrganisationLinkRequest
    {
        [JsonPropertyName("organisation_id")]
        public int OrganisationId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
