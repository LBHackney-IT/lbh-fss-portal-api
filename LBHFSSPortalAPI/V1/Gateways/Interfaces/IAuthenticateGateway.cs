using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        string CreateUser(UserCreateRequest createRequest);
        string AdminCreateUser(UserCreateRequest createRequest);
        bool ConfirmSignup(UserConfirmRequest confirmRequest);
        AuthenticationResult LoginUser(LoginUserQueryParam loginUserQueryParam);
        void ResendConfirmation(ConfirmationResendRequest confirmationResendRequest);
    }
}
