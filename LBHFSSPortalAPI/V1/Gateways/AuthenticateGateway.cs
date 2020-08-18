using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
//using Amazon.CognitoIdentity;
//using Amazon.S3;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class AuthenticateGateway : IAuthenticateGateway
    {
        public void CreateUser(LoginUserQueryParam loginUserQueryParam)
        {
            throw new System.NotImplementedException();
        }

        public LoginDomain LoginUser(LoginUserQueryParam loginUserQueryParam)
        {
            // TODO (MJC) Use the query params 'username' and 'password' to log into AWS Cognito
            // and store 'user pool token' (sub_id) in the portal api Users database 

            //+++
            // Amazon Cognito Credentials Provider
            //---

            //CognitoAWSCredentials credentials = new CognitoAWSCredentials(
            //                accountId,        // Account number
            //                identityPoolId,   // Identity pool ID
            //                unAuthRoleArn,    // Role for unauthenticated users
            //                null,             // Role for authenticated users, not set
            //                region);

            //using (var s3Client = new Amazon.S3.AmazonS3Client(credentials))
            //{
            //    s3Client.ListBuckets();
            //}

            throw new System.NotImplementedException();
        }
    }
}
