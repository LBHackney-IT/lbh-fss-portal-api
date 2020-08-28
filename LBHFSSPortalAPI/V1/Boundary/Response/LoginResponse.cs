
namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class LoginUserResponse
    {
        /// <summary>
        /// The access_token cookie name to use in the response
        /// </summary>
        public const string AccessTokenName = "access_token";

        public string AccessToken { get; set; }
    }
}
