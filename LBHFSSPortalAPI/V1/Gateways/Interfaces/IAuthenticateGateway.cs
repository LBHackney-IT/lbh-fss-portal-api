using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        string CreateUser(UserCreateRequest createRequest);
        string AdminCreateUser(AdminCreateUserRequest createRequest);
        bool ConfirmSignup(UserConfirmRequest confirmRequest);
        AuthenticationResult LoginUser(LoginUserQueryParam loginUserQueryParam);
        void ResendConfirmation(ConfirmationResendRequest confirmationResendRequest);
        void ResetPassword(ResetPasswordQueryParams resetPasswordQueryParams);
        void ConfirmResetPassword(ResetPasswordQueryParams resetPasswordQueryParams);
        void ChangePassword(ResetPasswordQueryParams resetPasswordQueryParams);
        AuthenticationResult ChallengePassword(ResetPasswordQueryParams resetPasswordQueryParams);
        bool DeleteUser(string subId);
    }
}
