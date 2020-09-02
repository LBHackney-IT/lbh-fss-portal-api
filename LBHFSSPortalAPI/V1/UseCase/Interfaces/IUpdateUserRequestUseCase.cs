using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IUpdateUserRequestUseCase
    {
        UserResponse Execute(int currentUserId, UserUpdateRequest userUpdateRequest);
    }
}
