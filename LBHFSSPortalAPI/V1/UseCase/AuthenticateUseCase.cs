using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class AuthenticateUseCase : IAuthenticateUseCase
    {
        private readonly IAuthenticateGateway _authenticateGateway;

        public AuthenticateUseCase(IAuthenticateGateway authenticateGateway)
        {
            _authenticateGateway = authenticateGateway;
        }

        /// <summary>
        /// Logs the user in to the API
        /// </summary>
        public LoginUserResponse ExecuteLoginUser(LoginUserQueryParam loginUserQueryParam)
        {
            var loginResponse = _authenticateGateway.LoginUser(loginUserQueryParam);

            var userResponse = new LoginUserResponse()
            {
                Username = loginResponse.Username,
                Password = loginResponse.Password
            };

            return userResponse;
        }

        /// <summary>
        /// Logs the user out of the API
        /// </summary>
        public void ExecuteLogoutUser(LogoutUserQueryParam logoutUserQueryParam)
        {
            throw new System.NotImplementedException();
        }
    }
}
