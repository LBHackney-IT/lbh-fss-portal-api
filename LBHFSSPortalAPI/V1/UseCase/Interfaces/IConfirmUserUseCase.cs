using Amazon.CognitoIdentityProvider.Model;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IConfirmUserUseCase
    {
        UserResponse Execute(UserConfirmRequest confirmRequestData);
        void Resend(ConfirmationResendRequest confirmationResendRequest);
    }
}