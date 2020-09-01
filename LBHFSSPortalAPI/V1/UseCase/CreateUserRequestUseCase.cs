using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System;
using LBHFSSPortalAPI.V1.Infrastructure;

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
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                return null;
            }

            SaveNewUser(createRequestData, createdUserId);

            var userCreateResponse = new UserResponse
            {
                Email = createRequestData.Email,
                Name = createRequestData.Name,
                SubId = createdUserId
            };

            return userCreateResponse;
        }

        public UserResponse AdminExecute(UserCreateRequest createRequestData)
        {
            string createdUserId = null;
            try
            {
                createdUserId = _authGateway.AdminCreateUser(createRequestData);
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                return null;
            }

            if (createdUserId != null)
            {
                var user = new Users
                {
                    SubId = createdUserId,
                    Name = createRequestData.Name,
                    Email = createRequestData.Email
                };
            }

            var userCreateResponse = new UserResponse
            {
                Email = createRequestData.Email,
                Name = createRequestData.Name,
                SubId = createdUserId
            };
            return userCreateResponse;
        }

        private void SaveNewUser(UserCreateRequest createRequestData, string createdUserId)
        {
            var user = _usersGateway.GetUser(createRequestData.Email, UserStatus.Invited);

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

                _usersGateway.SaveUser(user);
            }
        }
    }
}
