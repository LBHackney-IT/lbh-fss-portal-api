using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ServiceRequest
    {
        public string Name { get; set; }

        [JsonPropertyName("organisation_id")]
        public int? OrganisationId { get; set; }

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

        public virtual List<ServiceLocationRequest> Locations { get; set; }
        public virtual List<TaxonomyRequest> Categories { get; set; }
        public virtual List<int> Demographics { get; set; }
    }
}
