using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IUserOrganisationUseCase
    {
        UserOrganisationResponse ExecuteCreate(UserOrganisationRequest requestParams);
        void ExecuteDelete(int userId);
    }
}
