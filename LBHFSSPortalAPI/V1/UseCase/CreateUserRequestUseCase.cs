using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

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

            var userCreateResponse = new UserResponse
            {
                Email = createRequestData.EmailAddress,
                Name = createRequestData.Name,
                SubId = createdUserId
            };

            return userCreateResponse;
        }
    }
}
