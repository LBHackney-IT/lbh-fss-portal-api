namespace LBHFSSPortalAPI.V1.Domain
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string ResponseMessage { get; set; }
    }
}
