using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IGetAllUsersUseCase
    {
        Task<UsersResponseList> Execute(UserQueryParam userQueryParam);
    }
}
