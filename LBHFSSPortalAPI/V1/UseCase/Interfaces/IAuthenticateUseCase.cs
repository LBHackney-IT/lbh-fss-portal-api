using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IAuthenticateUseCase
    {
        LoginResponse Execute(LoginUserQueryParam loginUserQueryParam);
    }
}
