using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IConfirmUserUseCase
    {
        UserResponse Execute(UserConfirmRequest queryParam);
        void Resend(ConfirmationResendRequest confirmationResendRequest);
        void Resend(int userId);
    }
}
