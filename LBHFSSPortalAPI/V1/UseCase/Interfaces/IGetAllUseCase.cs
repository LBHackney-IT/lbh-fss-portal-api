using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IGetAllUseCase
    {
        //ResponseObjectList Execute();

        UsersResponseList Execute(UserQueryParam userQueryParam);
    }
}
