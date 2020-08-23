
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class LogoutUserQueryParam
    {
        [FromQuery(Name = "httponlycookie")]
        public string HttpOnlyCookie { get; set; }
    }
}
