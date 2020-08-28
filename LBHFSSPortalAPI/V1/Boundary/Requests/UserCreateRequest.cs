using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserCreateRequest
    {
        [FromQuery(Name = "emailaddress")]
        public string EmailAddress { get; set; }

        [FromQuery(Name = "password")]
        public string Password { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
