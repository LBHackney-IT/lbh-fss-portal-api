using System;

namespace LBHFSSPortalAPI.V1.Infrastructure
{
    public partial class ServiceLocation
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Uprn { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string NHSNeighbourhood { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Service Service { get; set; }
    }
}
