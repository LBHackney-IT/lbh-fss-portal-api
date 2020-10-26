using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ServiceLocationRequest
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Uprn { get; set; }
        [JsonPropertyName("address_1")]
        public string Address1 { get; set; }
        [JsonPropertyName("address_2")]
        public string Address2 { get; set; }
        public string City { get; set; }

        [JsonPropertyName("state_province")]
        public string StateProvince { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
