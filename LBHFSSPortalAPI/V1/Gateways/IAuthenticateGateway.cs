using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IAuthenticateGateway
    {
        void CreateUser(LoginUserQueryParam loginUserQueryParam);
        LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam);
    }
}
