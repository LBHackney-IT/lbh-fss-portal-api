using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        string CreateUser(UserCreateRequest createRequest);
        bool ConfirmSignup(UserConfirmRequest confirmRequest);
        LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam);
    }
}
