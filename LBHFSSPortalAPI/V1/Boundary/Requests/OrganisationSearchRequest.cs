namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class OrganisationSearchRequest
    {
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Direction { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
