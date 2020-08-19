using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public class AuthenticateUseCase : IAuthenticateUseCase
    {
        private readonly IAuthenticateGateway _authenticateGateway;

        public AuthenticateUseCase(IAuthenticateGateway authenticateGateway)
        {
            _authenticateGateway = authenticateGateway;
        }

        public LoginResponse Execute(LoginUserQueryParam loginUserQueryParam)
        {
            var loginResponse = _authenticateGateway.LoginUser(loginUserQueryParam);

            var userResponse = new LoginResponse()
            {
                Username = loginResponse.Username,
                Password = loginResponse.Password
            };

            return userResponse;
        }
    }
}
