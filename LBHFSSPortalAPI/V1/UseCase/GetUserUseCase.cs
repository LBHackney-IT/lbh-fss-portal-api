using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUsersGateway _usersGateway;

        public GetUserUseCase(IUsersGateway usersGateway)
        {
            _usersGateway = usersGateway;
        }

        public async Task<UserResponse> Execute(int userId)
        {
            var userDomain = await _usersGateway.GetUserAsync(userId).ConfigureAwait(false);

            if (userDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not retrieve a user with an ID of '{userId}'",
                };

            return userDomain.ToResponse();
        }

        public UserResponse Execute(string accessKey)
        {
            var userDomain = _usersGateway.GetUserBySubId(accessKey);

            if (userDomain != null)
                return userDomain.ToResponse();

            return null;
        }
    }
}
