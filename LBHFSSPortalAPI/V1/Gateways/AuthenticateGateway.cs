using System;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class AuthenticateGateway : IAuthenticateGateway
    {
        private static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.EUWest2;
        private ConnectionInfo _connectionInfo;

        public AuthenticateGateway(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }
        public string CreateUser(UserCreateRequest createRequest)
        {
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient(_connectionInfo.AccessKeyId, _connectionInfo.SecretAccessKey, Region);
            SignUpRequest signUpRequest = new SignUpRequest
            {
                ClientId = _connectionInfo.ClientId,
                Username = createRequest.Email,
                Password = createRequest.Password
            };
            try
            {
                SignUpResponse response = provider.SignUpAsync(signUpRequest).Result;
                return response.UserSub;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }

        public LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam)
        {
            // TODO (MJC) Use the query params 'username' and 'password' to log into AWS Cognito
            // and store 'user pool token' (sub_id) in the portal api Users database
            //
            // CognitoUserPool userPool = null;
            // string accessToken = string.Empty;
            //
            // using (var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials()))
            // {
            //     userPool = new CognitoUserPool(PoolId, AppClientId, provider);
            //
            //     CognitoUser user = new CognitoUser(loginUserQueryParam.EmailAddress, "clientID", userPool, provider);
            //     InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            //     {
            //         Password = loginUserQueryParam.Password
            //     };
            //
            //     AuthFlowResponse authResponse = user.StartWithSrpAuthAsync(authRequest).Result;
            //     accessToken = authResponse.AuthenticationResult.AccessToken;
            // }

            return new LoginDomain();
        }
    }
}
