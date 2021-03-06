using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System;
using LBHFSSPortalAPI.V1.Handlers;

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
                throw new UseCaseException() { UserErrorMessage = loginResult.ResponseMessage == null ? "Could not login as the email and/or password was invalid" : loginResult.ResponseMessage };
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
            _usersGateway.SetUserStatus(user, UserStatus.Active);
            var loginResponse = CreateLoginSession(loginResult, user);
            return loginResponse;
        }

        LoginUserResponse CreateLoginSession(LoginUserQueryParam loginParams, UserDomain user)
        {
            var timestamp = DateTime.UtcNow;
            var sessionId = Guid.NewGuid().ToString();

            LoggingHandler.LogInfo(loginParams.IpAddress);
            LoggingHandler.LogInfo(user.Id.ToString());

            Session session = new Session()
            {
                IpAddress = loginParams.IpAddress,
                CreatedAt = timestamp,
                LastAccessAt = timestamp,
                UserId = user.Id,
                Payload = sessionId,
            };

            _sessionsGateway.AddSession(session);

            return new LoginUserResponse() { AccessToken = sessionId };
        }

        /// <summary>
        /// Logs the user out of the API
        /// </summary>
        public void ExecuteLogoutUser(string accessToken)
        {
            _sessionsGateway.RemoveSessions(accessToken);
        }

        public void ExecutePasswordRecovery(ResetPasswordQueryParams queryParams)
        {
            try
            {
                _authenticateGateway.ResetPassword(queryParams);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw new UseCaseException() { UserErrorMessage = "Unable to execute password recovery" };
            }
        }

        public void ExecutePasswordRecoveryConfirmation(ResetPasswordQueryParams queryParams)
        {
            try
            {
                _authenticateGateway.ConfirmResetPassword(queryParams);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw new UseCaseException() { UserErrorMessage = "Unable to execute password recovery confirmation" };
            }
        }
    }
}
