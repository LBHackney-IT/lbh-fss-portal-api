using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class CreateServiceUseCase : ICreateServiceUseCase
    {
        public Task<ServiceResponse> Execute(AddServiceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
