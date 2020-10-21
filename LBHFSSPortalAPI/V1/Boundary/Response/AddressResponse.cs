using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class AddressResponse
    {
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("uprn")]
        public long UPRN { get; set; }
        [JsonPropertyName("address_1")]
        public string Address1 { get; set; }
        [JsonPropertyName("address_2")]
        public string Address2 { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("state_province")]
        public string StateProvince { get; set; }
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}
