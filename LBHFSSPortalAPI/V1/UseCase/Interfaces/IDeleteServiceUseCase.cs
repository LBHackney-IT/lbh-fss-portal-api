using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IDeleteServiceUseCase
    {
        Task Execute(int serviceId, UserClaims userClaims);
    }
}
