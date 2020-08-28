using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class LoginUserQueryParam
    {
        [FromQuery(Name = "username")]
        public string Username { get; set; }

        [FromQuery(Name = "emailaddress")]
        public string EmailAddress { get; set; }

        [FromQuery(Name = "password")]
        public string Password { get; set; }

        public string IpAddress { get; internal set; }
    }
}
