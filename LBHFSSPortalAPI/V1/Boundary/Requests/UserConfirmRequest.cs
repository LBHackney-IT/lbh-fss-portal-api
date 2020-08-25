using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserConfirmRequest
    {
        public string UserName { get; set; }
        public string VerificationCode { get; set; }
    }
}
