using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IUpdateUserRequestUseCase
    {
        bool Execute(int currentUserId, UserUpdateRequest userUpdateRequest);
    }
}
