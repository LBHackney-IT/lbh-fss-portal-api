using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserConfirmRequest
    {
        [FromQuery(Name = "verificationcode")]
        public string VerificationCode { get; set; }

        [FromQuery(Name = "emailaddress")]
        public string EmailAddress { get; set; }
        public string IpAddress { get; set; }
    }
}
