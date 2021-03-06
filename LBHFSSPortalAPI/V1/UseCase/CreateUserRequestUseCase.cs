using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;
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
using LBHFSSPortalAPI.V1.Handlers;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class CreateUserRequestUseCase : ICreateUserRequestUseCase
    {
        private IAuthenticateGateway _authGateway;
        private IUsersGateway _usersGateway;

        public CreateUserRequestUseCase(IAuthenticateGateway authGateway, IUsersGateway usersGateway)
        {
            _authGateway = authGateway;
            _usersGateway = usersGateway;
        }

        public UserResponse Execute(UserCreateRequest createRequestData)
        {
            string createdUserId = null;
            try
            {
                createdUserId = _authGateway.CreateUser(createRequestData);
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return null;
            }

            var userDomain = SaveNewUser(createRequestData, createdUserId);

            var userCreateResponse = new UserResponse
            {
                Id = userDomain.Id,
                CreatedAt = userDomain.CreatedAt,
                Email = createRequestData.Email,
                Name = createRequestData.Name,
                SubId = createdUserId,
                Status = userDomain.Status
            };

            return userCreateResponse;
        }

        public UserResponse AdminExecute(AdminCreateUserRequest createRequestData)
        {
            UserResponse response = null;
            string subId;

            // check for currently active user with the same email address (prevents 2 active
            // users with the same email address in the database which can cause problems
            // elsewhere, e.g. 'login user')
            var user = _usersGateway.GetUserByEmail(createRequestData.Email, UserStatus.Active);

            if (user != null)
                throw new UseCaseException()
                {
                    UserErrorMessage = "An active user with the supplied email address is already registered"
                };

            try
            {
                subId = _authGateway.AdminCreateUser(createRequestData);
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                return null;
            }

            if (subId != null)
            {
                var userDomain = _usersGateway.AddUser(createRequestData, subId);

                if (userDomain != null)
                    response = userDomain.ToResponse();
                response.SetPasswordRequired = true;
            }

            return response;
        }

        private UserDomain SaveNewUser(UserCreateRequest createRequestData, string createdUserId)
        {
            var user = _usersGateway.GetUserByEmail(createRequestData.Email, UserStatus.Invited);

            if (user == null)
            {
                var createdAt = DateTime.UtcNow;

                user = new UserDomain()
                {
                    CreatedAt = createdAt,
                    Email = createRequestData.Email,
                    Name = createRequestData.Name,
                    Status = UserStatus.Unverified,
                    SubId = createdUserId
                };

                return _usersGateway.AddUser(user);
            }

            return null;
        }
    }
}
