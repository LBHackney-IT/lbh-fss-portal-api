
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class LogoutUserQueryParam
    {
        [FromQuery(Name = "access_token")]
        public string AccessToken { get; set; }
    }
}
