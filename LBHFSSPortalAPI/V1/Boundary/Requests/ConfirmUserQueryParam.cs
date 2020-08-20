namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ConfirmUserQueryParam
    { 
        public string Name { get; set; }

        public string VerificationCode { get; set; }
        public string EmailAddress { get; set; }
    }
}
