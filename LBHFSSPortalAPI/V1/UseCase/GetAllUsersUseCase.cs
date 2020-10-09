using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IUsersGateway _usersGateway;

        public GetAllUsersUseCase(IUsersGateway usersGateway)
        {
            _usersGateway = usersGateway;
        }

        public async Task<UsersResponseList> Execute(UserQueryParam userQueryParam)
        {
            var users = await _usersGateway.GetAllUsers(userQueryParam).ConfigureAwait(false);

            var usersList = new UsersResponseList
            {
                Users = users.ToResponse()
            };

            return usersList;
        }
    }
}
