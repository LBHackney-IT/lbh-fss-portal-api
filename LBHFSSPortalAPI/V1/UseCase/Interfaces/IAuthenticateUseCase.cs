using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IAuthenticateUseCase
    {
        LoginUserResponse ExecuteLoginUser(LoginUserQueryParam loginParams);
        LoginUserResponse ExecuteFirstLogin(ResetPasswordQueryParams loginParams, string ipAddress);
        void ExecuteLogoutUser(LogoutUserQueryParam logoutUserQueryParam);
    }
}
