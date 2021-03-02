namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class SynonymWordSearchRequest
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Direction { get; set; }
    }
}
