namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ResetPasswordQueryParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Session { get; set; }
    }
}
