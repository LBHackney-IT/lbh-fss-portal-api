using System;
using System.Collections.Generic;
using System.Net;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Infrastructure;
using AdminCreateUserRequest = Amazon.CognitoIdentityProvider.Model.AdminCreateUserRequest;
using LBHFSSPortalAPI.V1.Exceptions;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class AuthenticateGateway : IAuthenticateGateway
    {
        private static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.EUWest2;
        private ConnectionInfo _connectionInfo;
        private AmazonCognitoIdentityProviderClient _provider;
        public AuthenticateGateway(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
            _provider =
                new AmazonCognitoIdentityProviderClient(_connectionInfo.AccessKeyId, _connectionInfo.SecretAccessKey, Region);
        }

        public string AdminCreateUser(Boundary.Requests.AdminCreateUserRequest createRequest)
        {
            AdminCreateUserRequest adminCreateUserRequest = new AdminCreateUserRequest
            {
                UserPoolId = _connectionInfo.UserPoolId,
                Username = createRequest.Email,
                DesiredDeliveryMediums = new List<string> { "EMAIL" }
            };
            try
            {
                var response = _provider.AdminCreateUserAsync(adminCreateUserRequest).Result;
                return response.User.UserStatus;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }
        public string CreateUser(UserCreateRequest createRequest)
        {
            SignUpRequest signUpRequest = new SignUpRequest
            {
                ClientId = _connectionInfo.ClientId,
                Username = createRequest.Email,
                Password = createRequest.Password
            };
            try
            {
                SignUpResponse response = _provider.SignUpAsync(signUpRequest).Result;
                return response.UserSub;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }

        public bool ConfirmSignup(UserConfirmRequest confirmRequest)
        {
            ConfirmSignUpRequest signUpRequest = new ConfirmSignUpRequest
            {
                ClientId = _connectionInfo.ClientId,
                Username = confirmRequest.Email,
                ConfirmationCode = confirmRequest.Code
            };
            try
            {
                ConfirmSignUpResponse response = _provider.ConfirmSignUpAsync(signUpRequest).Result;
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }

        public void ResendConfirmation(ConfirmationResendRequest confirmationResendRequest)
        {
            ResendConfirmationCodeRequest resendConfirmationCodeRequest = new ResendConfirmationCodeRequest()
            {
                ClientId = _connectionInfo.ClientId,
                Username = confirmationResendRequest.Email
            };
            try
            {
                _provider.ResendConfirmationCodeAsync(resendConfirmationCodeRequest).Wait();
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }

        public AuthenticationResult LoginUser(LoginUserQueryParam loginUserQueryParam)
        {
            InitiateAuthRequest iaRequest = new InitiateAuthRequest
            {
                ClientId = _connectionInfo.ClientId,
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
            };
            iaRequest.AuthParameters.Add("USERNAME", loginUserQueryParam.Email);
            iaRequest.AuthParameters.Add("PASSWORD", loginUserQueryParam.Password);
            InitiateAuthResponse authResp = new InitiateAuthResponse();
            var authResult = new AuthenticationResult();
            try
            {
                authResp = _provider.InitiateAuthAsync(iaRequest).Result;
                authResult.AccessToken = authResp.AuthenticationResult.AccessToken;
                authResult.IdToken = authResp.AuthenticationResult.IdToken;
                authResult.RefreshToken = authResp.AuthenticationResult.RefreshToken;
                authResult.TokenType = authResp.AuthenticationResult.TokenType;
                authResult.ExpiresIn = authResp.AuthenticationResult.ExpiresIn;
                authResult.Success = true;
            }
            catch (AggregateException e)
            {
                e.Handle((x) =>
                {
                    if (x is NotAuthorizedException)  // This we know how to handle.
                    {
                        Console.WriteLine("Invalid credentials provided.");
                        authResult.Success = false;
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });
            }
            return authResult;
        }

        public void ResetPassword(ResetPasswordQueryParams resetPasswordQueryParams)
        {
            ForgotPasswordRequest fpRequest = new ForgotPasswordRequest
            {
                ClientId = _connectionInfo.ClientId,
                Username = resetPasswordQueryParams.Email
            };
            _provider.ForgotPasswordAsync(fpRequest).Wait();
        }

        public void ConfirmResetPassword(ResetPasswordQueryParams resetPasswordQueryParams)
        {
            ConfirmForgotPasswordRequest cfpRequest = new ConfirmForgotPasswordRequest
            {
                ClientId = _connectionInfo.ClientId,
                Username = resetPasswordQueryParams.Email,
                Password = resetPasswordQueryParams.Password,
                ConfirmationCode = resetPasswordQueryParams.Code
            };
            _provider.ConfirmForgotPasswordAsync(cfpRequest).Wait();
        }

        public void ChangePassword(ResetPasswordQueryParams resetPasswordQueryParams)
        {
            AdminSetUserPasswordRequest adminSetUserPasswordRequest = new AdminSetUserPasswordRequest
            {
                Password = resetPasswordQueryParams.Password,
                Username = resetPasswordQueryParams.Email,
                UserPoolId = _connectionInfo.UserPoolId,
                Permanent = true
            };
            var response = _provider.AdminSetUserPasswordAsync(adminSetUserPasswordRequest).Result;
        }

        public AuthenticationResult ChallengePassword(ResetPasswordQueryParams resetPasswordQueryParams)
        {
            RespondToAuthChallengeRequest authChallengeRequest = new RespondToAuthChallengeRequest
            {
                ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
                ChallengeResponses = new Dictionary<string, string>
                {
                    {"NEW_PASSWORD", resetPasswordQueryParams.Password},
                    {"USERNAME", resetPasswordQueryParams.Email}
                },
                ClientId = _connectionInfo.ClientId,
                Session = resetPasswordQueryParams.Session
            };
            var authResult = new AuthenticationResult();
            try
            {
                var authResp = _provider.RespondToAuthChallengeAsync(authChallengeRequest).Result;
                authResult.AccessToken = authResp.AuthenticationResult.AccessToken;
                authResult.IdToken = authResp.AuthenticationResult.IdToken;
                authResult.RefreshToken = authResp.AuthenticationResult.RefreshToken;
                authResult.TokenType = authResp.AuthenticationResult.TokenType;
                authResult.ExpiresIn = authResp.AuthenticationResult.ExpiresIn;
                authResult.Success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return authResult;
        }

        public bool DeleteUser(string email)
        {
            AdminDeleteUserRequest adminDeleteUserRequest = new AdminDeleteUserRequest
            {
                Username = email,
                UserPoolId = _connectionInfo.UserPoolId
            };
            try
            {
                var response = _provider.AdminDeleteUserAsync(adminDeleteUserRequest).Result;
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
