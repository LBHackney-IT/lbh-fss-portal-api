
namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class LoginUserQueryParam
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string IpAddress { get; internal set; }
    }
}
