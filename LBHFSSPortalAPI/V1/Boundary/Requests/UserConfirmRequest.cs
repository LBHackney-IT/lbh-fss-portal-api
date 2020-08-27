using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserConfirmRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
