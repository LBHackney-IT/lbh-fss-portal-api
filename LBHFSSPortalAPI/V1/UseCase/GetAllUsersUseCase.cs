using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IUsersGateway _usersGateway;

        public GetAllUsersUseCase(IUsersGateway usersGateway)
        {
            _usersGateway = usersGateway;
        }

        public UsersResponseList Execute(UserQueryParam userQueryParam)
        {
            var users = _usersGateway.GetAllUsers().ToResponse();

            var usersList =  new UsersResponseList
            {
                Users = users
            };

            return usersList;
        }
    }
}
