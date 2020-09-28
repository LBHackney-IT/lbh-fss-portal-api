using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class ServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("user_id")]
        public int? UserId { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("organisation_id")]
        public int? OrganisationId { get; set; }

        [JsonPropertyName("organisation_name")]
        public string OrganisationName { get; set; }

        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public string Description { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Keywords { get; set; }

        [JsonPropertyName("referral_link")]
        public string ReferralLink { get; set; }

        [JsonPropertyName("referral_email")]
        public string ReferralEmail { get; set; }

        public ImageResponse Image { get; set; }
        public List<LocationResponse> Locations { get; set; }
        public List<TaxonomyResponse> Categories { get; set; }
        public List<TaxonomyResponse> Demographics { get; set; }
    }
}
