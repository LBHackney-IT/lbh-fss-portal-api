using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class AddServiceRequest
    {
        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; }
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

        [JsonPropertyName("image_id")]
        public int? ImageId { get; set; }

        public virtual ICollection<ServiceLocationRequest> Locations { get; set; }
        public virtual ICollection<TaxonomyRequest> Categories { get; set; }
    }
}
