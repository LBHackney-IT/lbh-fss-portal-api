using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class AuthenticateUseCase : IAuthenticateUseCase
    {
        private readonly IAuthenticateGateway _authenticateGateway;
        private readonly ISessionsGateway _sessionsGateway;
        private readonly IUsersGateway _usersGateway;

        public AuthenticateUseCase(IAuthenticateGateway authenticateGateway,
                                   ISessionsGateway sessionsGateway,
                                   IUsersGateway usersGateway)
        {
            _authenticateGateway = authenticateGateway;
            _sessionsGateway = sessionsGateway;
            _usersGateway = usersGateway;
        }

        /// <summary>
        /// Logs the user in to the API
        /// </summary>
        public LoginUserResponse ExecuteLoginUser(LoginUserQueryParam loginParams)
        {
            if (string.IsNullOrWhiteSpace(loginParams.Username))
                throw new UseCaseException() { ApiErrorMessage = "Could not login as the username was invalid" };

            if (string.IsNullOrWhiteSpace(loginParams.Password))
                throw new UseCaseException() { ApiErrorMessage = "Could not login as the password was invalid" };

            var subId = _authenticateGateway.LoginUser(loginParams.Username, loginParams.EmailAddress);
            var user = _usersGateway.GetUserBySubId(subId);

            var loginResponse = CreateLoginSession(loginParams, user);
            return loginResponse;
        }

        LoginUserResponse CreateLoginSession(LoginUserQueryParam loginParams, UserDomain user)
        {
            var timestamp = DateTime.UtcNow;
            var sessionId = Guid.NewGuid().ToString();

            Sessions session = new Sessions()
            {
                IpAddress = loginParams.IpAddress,
                CreatedAt = timestamp,
                LastAccessAt = timestamp,
                UserId = user.Id,
                SessionId = sessionId
                //Payload = (?)
                //UserAgent = (?)
            };

            var savedSession = _sessionsGateway.AddSession(session);

            var res = new LoginUserResponse
            {
                AccessToken = savedSession.SessionId,
            };

            return res;
        }

        /// <summary>
        /// Logs the user out of the API
        /// </summary>
        public void ExecuteLogoutUser(LogoutUserQueryParam queryParam)
        {
            if (string.IsNullOrWhiteSpace(queryParam.AccessToken))
                throw new UseCaseException() { ApiErrorMessage = "the access_token parameter was empty or not supplied" };

            //idempotent!
            _sessionsGateway.RemoveSession(queryParam.AccessToken);
        }
    }
}
