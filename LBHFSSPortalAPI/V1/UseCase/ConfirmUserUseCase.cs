using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
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

        public UserResponse Execute(UserConfirmRequest confirmRequest)
        {
            var response = new UserResponse();

            if (_authenticateGateway.ConfirmSignup(confirmRequest))
            {
                var user = _usersGateway.GetUserByEmail(confirmRequest.Email, UserStatus.Invited);

                if (user == null)
                {
                    user = _usersGateway.GetUserByEmail(confirmRequest.Email, UserStatus.Unverified);

                    if (user == null)
                    {
                        // could not find user in either of the required states to confirm registration (invited/unverified)
                        throw new UseCaseException()
                        {
                            UserErrorMessage = "User with the supplied email address not found in the required state of invited or unverified"
                        };
                    }
                }
                _usersGateway.SetDefaultRole(user);
                _usersGateway.SetUserStatus(user, UserStatus.Active);
                response = user.ToResponse();
            }
            else
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not validate user registration on the authentication gateway"
                };
            }

            return response;
        }

        public void Resend(ConfirmationResendRequest confirmationResendRequest)
        {
            _authenticateGateway.ResendConfirmation(confirmationResendRequest);
        }

        public void Resend(int userId)
        {
            var userDomain = _usersGateway.GetUserById(userId);

            if (userDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"A user with the supplied ID of '{userId}' could not be found"
                };

            _authenticateGateway.ResendConfirmation(new ConfirmationResendRequest { Email = userDomain.Email });
        }
    }
}
