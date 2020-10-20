using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IServiceImageUseCase
    {
        Task ExecuteCreate(ServiceImageRequest request);
    }
}
