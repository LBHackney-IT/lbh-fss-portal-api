
namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class ConfirmUserResponse
    {
        /// <summary>
        /// The access_token cookie name to use in the response
        /// </summary>
        public const string AccessTokenName = "access_token";

        public string AccessTokenValue { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }
}
