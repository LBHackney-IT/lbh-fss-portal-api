using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class OrganisationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
        [JsonPropertyName("reviewed_at")]
        public DateTime? ReviewedAt { get; set; }
        [JsonPropertyName("reviewer_message")]
        public string ReviewerMessage { get; set; }
        public string Status { get; set; }
        [JsonPropertyName("is_hackney_based")]
        public bool? IsHackneyBased { get; set; }
        [JsonPropertyName("is_registered_charity")]
        public bool? IsRegisteredCharity { get; set; }
        [JsonPropertyName("charity_number")]
        public string CharityNumber { get; set; }
        [JsonPropertyName("has_hc_or_col_grant")]
        public bool? HasHcOrColGrant { get; set; }
        [JsonPropertyName("has_hcvs_or_hg_or_ael_grant")]
        public bool? HasHcvsOrHgOrAelGrant { get; set; }
        [JsonPropertyName("is_tra_registered")]
        public bool? IsTraRegistered { get; set; }
        [JsonPropertyName("rsl_or_ha_association")]
        public string RslOrHaAssociation { get; set; }
        [JsonPropertyName("is_lottery_funded")]
        public bool? IsLotteryFunded { get; set; }
        [JsonPropertyName("lottery_funded_project")]
        public string LotteryFundedProject { get; set; }
        [JsonPropertyName("funding_other")]
        public string FundingOther { get; set; }
        [JsonPropertyName("has_child_support")]
        public bool? HasChildSupport { get; set; }
        [JsonPropertyName("has_child_safeguarding_lead")]
        public bool? HasChildSafeguardingLead { get; set; }
        [JsonPropertyName("child_safeguarding_lead_first_name")]
        public string ChildSafeguardingLeadFirstName { get; set; }
        [JsonPropertyName("child_safeguarding_lead_last_name")]
        public string ChildSafeguardingLeadLastName { get; set; }
        [JsonPropertyName("child_safeguarding_lead_training_month")]
        public string ChildSafeguardingLeadTrainingMonth { get; set; }
        [JsonPropertyName("child_safeguarding_lead_training_year")]
        public string ChildSafeguardingLeadTrainingYear { get; set; }
        [JsonPropertyName("has_adult_support")]
        public bool? HasAdultSupport { get; set; }
        [JsonPropertyName("has_adult_safeguarding_lead")]
        public bool? HasAdultSafeguardingLead { get; set; }
        [JsonPropertyName("adult_safeguarding_lead_first_name")]
        public string AdultSafeguardingLeadFirstName { get; set; }
        [JsonPropertyName("adult_safeguarding_lead_last_name")]
        public string AdultSafeguardingLeadLastName { get; set; }
        [JsonPropertyName("adult_safeguarding_lead_training_month")]
        public string AdultSafeguardingLeadTrainingMonth { get; set; }
        [JsonPropertyName("adult_safeguarding_lead_training_year")]
        public string AdultSafeguardingLeadTrainingYear { get; set; }
        [JsonPropertyName("has_enhanced_support")]
        public bool? HasEnhancedSupport { get; set; }
        [JsonPropertyName("is_local_offer_listed")]
        public bool? IsLocalOfferListed { get; set; }
    }
}
