using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface IOrganisationsUseCase
    {
        OrganisationResponse ExecuteCreate(string accessToken, OrganisationRequest requestParams);
        OrganisationResponse ExecuteGet(int id);
        Task<OrganisationResponseList> ExecuteGet(OrganisationSearchRequest requestParams);
        OrganisationResponse ExecutePatch(int id, OrganisationRequest requestParams);
        void ExecuteDelete(int id, UserClaims userClaims);
    }
}
