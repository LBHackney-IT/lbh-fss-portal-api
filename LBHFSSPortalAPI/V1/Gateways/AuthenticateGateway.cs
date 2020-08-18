using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.CognitoIdentity;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class AuthenticateGateway : IAuthenticateGateway
    {
        private const string ClientId = "3ogiujoo72poudged2amgiefss";
        private const string PoolId = "7uam787ffful1egjdiqveaeaajvbnmssrud6v1lu16t0ma2i288";
        //private const string ClientSecret = "7uam787ffful1egjdiqveaeaajvbnmssrud6v1lu16t0ma2i288";

        public void CreateUser(LoginUserQueryParam loginUserQueryParam)
        {

            //CognitoAWSCredentials credentials = new CognitoAWSCredentials(
            //    accountId,        // Account number
            //    identityPoolId,   // Identity pool ID
            //    unAuthRoleArn,    // Role for unauthenticated users
            //    null,             // Role for authenticated users, not set
            //    region);

            //using (var s3Client = new AmazonS3Client(credentials))
            //{
            //    s3Client.ListBuckets();
            //}

            throw new System.NotImplementedException();
        }

        public LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam)
        {
            // TODO (MJC) Use the query params 'username' and 'password' to log into AWS Cognito
            // and store 'user pool token' (sub_id) in the portal api Users database 

            CognitoUserPool userPool = null;
            string accessToken = string.Empty;

            using (var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials()))
            {
                userPool = new CognitoUserPool(PoolId, ClientId, provider);

                CognitoUser user = new CognitoUser(loginUserQueryParam.EmailAddress, "clientID", userPool, provider);
                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
                {
                    Password = loginUserQueryParam.Password
                };

                AuthFlowResponse authResponse = user.StartWithSrpAuthAsync(authRequest).Result;
                accessToken = authResponse.AuthenticationResult.AccessToken;
            }

            return new LoginDomain();
        }
    }
}
