using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IDeleteServiceUseCase
    {
        Task Execute(int serviceId);
    }
}
