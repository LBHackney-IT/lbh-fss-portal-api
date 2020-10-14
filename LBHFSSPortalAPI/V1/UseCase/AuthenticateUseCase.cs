using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
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
            if (string.IsNullOrWhiteSpace(loginParams.Email))
                throw new UseCaseException() { UserErrorMessage = "Could not login as the email address was invalid" };

            if (string.IsNullOrWhiteSpace(loginParams.Password))
                throw new UseCaseException() { UserErrorMessage = "Could not login as the password was invalid" };

            var loginResult = _authenticateGateway.LoginUser(loginParams);
            if (!loginResult.Success)
                throw new UseCaseException() { UserErrorMessage = "Could not login as the email and/or password was invalid" };
            var user = _usersGateway.GetUserByEmail(loginParams.Email, UserStatus.Active);
            var loginResponse = CreateLoginSession(loginParams, user);

            return loginResponse;
        }

        public LoginUserResponse ExecuteFirstLogin(ResetPasswordQueryParams loginParams, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(loginParams.Email))
                throw new UseCaseException() { UserErrorMessage = "Could not login as the email address was invalid" };

            if (string.IsNullOrWhiteSpace(loginParams.Password))
                throw new UseCaseException() { UserErrorMessage = "Could not login as the password was invalid" };
            var loginResult = _authenticateGateway.ChangePassword(loginParams);
            if (loginResult == null)
                throw new UseCaseException() { UserErrorMessage = "Could not login as the email and/or password was invalid" };
            loginResult.IpAddress = ipAddress;
            var user = _usersGateway.GetUserByEmail(loginParams.Email, UserStatus.Invited);
            user.Status = UserStatus.Active;
            _usersGateway.UpdateUser(user);
            var loginResponse = CreateLoginSession(loginResult, user);
            return loginResponse;
        }

        LoginUserResponse CreateLoginSession(LoginUserQueryParam loginParams, UserDomain user)
        {
            var timestamp = DateTime.UtcNow;
            var sessionId = Guid.NewGuid().ToString();
            Console.WriteLine(loginParams.IpAddress);
            Console.WriteLine(user.Id);
            Session session = new Session()
            {
                IpAddress = loginParams.IpAddress,
                CreatedAt = timestamp,
                LastAccessAt = timestamp,
                UserId = user.Id,
                //Payload = (?)
                //UserAgent = (?)
            };

            var savedSession = _sessionsGateway.AddSession(session);

            var res = new LoginUserResponse
            {
                AccessToken = user.SubId,
            };

            return res;
        }

        /// <summary>
        /// Logs the user out of the API
        /// </summary>
        public void ExecuteLogoutUser(LogoutUserQueryParam queryParam)
        {
            if (string.IsNullOrWhiteSpace(queryParam.AccessToken))
                throw new UseCaseException() { UserErrorMessage = "the access_token parameter was empty or not supplied" };

            _sessionsGateway.RemoveSessions(queryParam.AccessToken);
        }
    }
}
