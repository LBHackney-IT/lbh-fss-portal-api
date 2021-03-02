using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    public interface ISynonymsUseCase
    {
        SynonymGroupResponse ExecuteCreate(string accessToken, SynonymGroupRequest requestParams);
        SynonymsResponse ExecuteUpdate(string accessToken, SynonymUpdateRequest requestParams);
    }
}
