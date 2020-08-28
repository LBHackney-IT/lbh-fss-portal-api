using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        string CreateUser(UserCreateRequest createRequest);

        /// <summary>
        /// Atempt to login to the authentication gateway
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>the access token identifier (aka subscription ID)</returns>
        string LoginUser(string userName, string password);

        /// <summary>
        /// Confirms the verification code of the user on the authentication gateway
        /// </summary>
        bool ConfirmSignup(string emailAddress, string verificationCode);

        void ResendConfirmation(ConfirmationResendRequest confirmationResendRequest);

    }
}
