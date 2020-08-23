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
    public class ConfirmUserRegUseCase : IConfirmUserRegUseCase
    {
        private IUsersGateway _usersGateway;
        private ISessionsGateway _sessionsGateway;
        private readonly IAuthenticateGateway _authenticateGateway;

        public ConfirmUserRegUseCase(IUsersGateway usersGateway,
            ISessionsGateway sessionsGateway, IAuthenticateGateway authenticateGateway)
        {
            _usersGateway = usersGateway;
            _sessionsGateway = sessionsGateway;
            _authenticateGateway = authenticateGateway;
        }

        public ConfirmUserResponse Execute(ConfirmUserQueryParam queryParam)
        {
            var response = new ConfirmUserResponse();

            if (_authenticateGateway.ConfirmUser(queryParam.EmailAddress,
                                                 queryParam.VerificationCode)) // verification code == sub_id(?)
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
                Email = user.Email,
                Name = user.Name
            };

            return res;
        }

        //Sessions CreateSession(ConfirmUserQueryParam queryParam, UserDomain user)
        //{
        //    var timestamp = DateTime.UtcNow;
        //    var sessionId = Guid.NewGuid().ToString();

        //    Sessions session = new Sessions()
        //    {
        //        IpAddress = queryParam.IpAddress,
        //        CreatedAt = timestamp,
        //        LastAccessAt = timestamp,
        //        UserId = user.Id,
        //        SessionId = sessionId
        //        //Payload =
        //        //UserAgent = 
        //    };

        //    var savedSession = _sessionsGateway.AddSession(session);
        //    return savedSession;
        //}
    }
}
