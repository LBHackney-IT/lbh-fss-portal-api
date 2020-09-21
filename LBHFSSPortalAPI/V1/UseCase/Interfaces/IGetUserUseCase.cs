using LBHFSSPortalAPI.V1.Boundary.Response;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IGetUserUseCase
    {
        Task<UserResponse> Execute(int userId);
    }
}
