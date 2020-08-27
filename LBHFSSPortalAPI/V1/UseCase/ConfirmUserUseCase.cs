using Amazon.CognitoIdentityProvider;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class ConfirmUserUseCase : IConfirmUserUseCase
    {
        private IAuthenticateGateway _authGateway;
        private IUsersGateway _usersGateway;

        public ConfirmUserUseCase(IAuthenticateGateway authGateway, IUsersGateway usersGateway)
        {
            _authGateway = authGateway;
            _usersGateway = usersGateway;
        }
        public UserResponse Execute(UserConfirmRequest confirmRequestData)
        {
            var userConfirmed = false;
            try
            {
                userConfirmed = _authGateway.ConfirmSignup(confirmRequestData);
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                return null;
            }

            var response = new UserResponse();
            if (userConfirmed)
            {
                response.SubId = "[to provide]";
            }
            return response;
        }

        public void Resend(ConfirmationResendRequest confirmationResendRequest)
        {
            _authGateway.ResendConfirmation(confirmationResendRequest);
        }
    }
}
