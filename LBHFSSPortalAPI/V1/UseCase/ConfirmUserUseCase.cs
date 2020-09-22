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
                var user = _usersGateway.GetUser(confirmRequest.Email, UserStatus.Invited);

                if (user == null)
                {
                    user = _usersGateway.GetUser(confirmRequest.Email, UserStatus.Unverified);

                    if (user == null)
                    {
                        // could not find user in either of the required states to confirm registration (invited/unverified)
                        throw new UseCaseException()
                        {
                            UserErrorMessage = "User with the supplied email address not found in the required state of invited or unverified"
                        };
                    }
                }

                user.Status = UserStatus.Active;
                _usersGateway.UpdateUser(user);
                response = CreateSession(confirmRequest, user);
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

        UserResponse CreateSession(UserConfirmRequest queryParam, UserDomain user)
        {
            var timestamp = DateTime.UtcNow;
            var sessionId = Guid.NewGuid().ToString();

            Session session = new Session()
            {
                IpAddress = queryParam.IpAddress,
                CreatedAt = timestamp,
                LastAccessAt = timestamp,
                UserId = user.Id,
                //Payload = (?)
                //UserAgent = (?)
            };

            var savedSession = _sessionsGateway.AddSession(session);

            var res = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                Status = user.Status,
                SubId = user.SubId
            };

            return res;
        }

        public void Resend(ConfirmationResendRequest confirmationResendRequest)
        {
            _authenticateGateway.ResendConfirmation(confirmationResendRequest);
        }
    }
}
