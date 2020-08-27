using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;
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
    public class ConfirmUserUseCase : IConfirmUserUseCase
    {
        private IUsersGateway _usersGateway;
        private ISessionsGateway _sessionsGateway;
        private readonly IAuthenticateGateway _authenticateGateway;

        public ConfirmUserUseCase(IUsersGateway usersGateway,
            ISessionsGateway sessionsGateway, IAuthenticateGateway authenticateGateway)
        {
            _usersGateway = usersGateway;
            _sessionsGateway = sessionsGateway;
            _authenticateGateway = authenticateGateway;
        }

        public ConfirmUserResponse Execute(ConfirmUserQueryParam queryParam)
        {
            var response = new ConfirmUserResponse();

            if (_authenticateGateway.ConfirmSignup(queryParam.EmailAddress, queryParam.VerificationCode))
            {
                var user = _usersGateway.GetUser(queryParam.EmailAddress, UserStatus.Invited);

                if (user == null)
                {
                    user = _usersGateway.GetUser(queryParam.EmailAddress, UserStatus.Unverified);

                    if (user == null)
                    { 
                        // could not find user in either of the required states to confirm registration (invited/unverified)
                        throw new UseCaseException()
                        {
                            ApiErrorMessage = "User with the supplied email address not found in the required state of invited or unverified"
                        };
                    }
                }

                user.Status = UserStatus.Active;
                _usersGateway.SaveUser(user);
                response = CreateSession(queryParam, user);
            }
            else
            {
                throw new UseCaseException(){
                    ApiErrorMessage = "Could not validate user registration on the authentication gateway" };
            }

            return response;
        }

        public bool ConfirmUser(string emailAddress, string verificationCode)
        {
            bool success = false;
            try
            {
                success = _authenticateGateway.ConfirmSignup(emailAddress, verificationCode);
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
            }

            return success;
        }

        ConfirmUserResponse CreateSession(ConfirmUserQueryParam queryParam, UserDomain user)
        {
            var timestamp = DateTime.UtcNow;
            var sessionId = Guid.NewGuid().ToString();

            Sessions session = new Sessions()
            {
                IpAddress = queryParam.IpAddress,
                CreatedAt = timestamp,
                LastAccessAt = timestamp,
                UserId = user.Id,
                SessionId = sessionId
                //Payload = (?)
                //UserAgent = (?)
            };

            var savedSession = _sessionsGateway.AddSession(session);

            var res = new ConfirmUserResponse
            {
                AccessTokenValue = savedSession.SessionId,
                UserResponse = new UserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    CreatedAt = user.CreatedAt,
                    Status = user.Status,
                    SubId = user.SubId                    
                }
            };

            return res;
        }

        public void Resend(ConfirmationResendRequest confirmationResendRequest)
        {
            _authenticateGateway.ResendConfirmation(confirmationResendRequest);
        }
    }
}
