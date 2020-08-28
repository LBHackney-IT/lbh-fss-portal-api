using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ConfirmationResendRequest
    {
        public string Email { get; set; }
    }
}
