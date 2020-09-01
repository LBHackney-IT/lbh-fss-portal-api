using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserConfirmRequest
    {
        public string Code { get; set; }

        public string Email { get; set; }

        public string IpAddress { get; set; }
    }
}
