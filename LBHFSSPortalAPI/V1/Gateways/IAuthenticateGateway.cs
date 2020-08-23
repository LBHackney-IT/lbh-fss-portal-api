using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        string CreateUser(UserCreateRequest createRequest);
        LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam);

        /// <summary>
        /// Confirms the verification code of the user on the authentication gateway
        /// </summary>
        bool ConfirmUser(string emailAddress, string status);
    }
}
