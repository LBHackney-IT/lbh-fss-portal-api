using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
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
        void AdminChangePassword(ResetPasswordQueryParams resetPasswordQueryParams);
        LoginUserQueryParam ChangePassword(ResetPasswordQueryParams loginUserQueryParam);
        AuthenticationResult ChallengePassword(ResetPasswordQueryParams resetPasswordQueryParams);
        bool DeleteUser(string subId);
        public string GetUserStatus(string email);
    }
}
