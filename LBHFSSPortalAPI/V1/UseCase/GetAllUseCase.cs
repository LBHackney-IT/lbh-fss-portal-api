using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetAllClaimantsUseCase
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IExampleGateway _gateway;
        public GetAllUseCase(IExampleGateway gateway)
        {
            _gateway = gateway;
        }

        public ResponseObjectList Execute()
        {
            return new ResponseObjectList { ResponseObjects = _gateway.GetAll().ToResponse() };
        }

        public UsersResponseList Execute(UserQueryParam userQueryParam)
        {
            // TODO: Implement the below (MJC)

            //var residents = _residentGateway.GetResidents(rqp.FirstName, rqp.LastName).ToResponse();

            //return new ResidentResponseList
            //{
            //    Residents = residents
            //};

            throw new System.NotImplementedException();
        }
    }
}
