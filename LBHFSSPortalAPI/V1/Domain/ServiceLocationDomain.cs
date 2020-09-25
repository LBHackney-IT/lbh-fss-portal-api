namespace LBHFSSPortalAPI.V1.Domain
{
    public class ServiceLocationDomain
    {
        public decimal? Latitude { get; internal set; }
        public decimal? Longitude { get; internal set; }
        public int? Uprn { get; internal set; }
        public string Address1 { get; internal set; }
        public string City { get; internal set; }
        public string StateProvince { get; internal set; }
        public string PostalCode { get; internal set; }
        public string Country { get; internal set; }
    }
}