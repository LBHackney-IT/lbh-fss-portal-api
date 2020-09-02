using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IDeleteUserRequestUseCase
    {
        bool Execute(UserDeleteRequest userDeleteRequest);
    }
}
